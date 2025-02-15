using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BattleSystem;
using Common;
using Common.Events;
using NaughtyAttributes;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;
using ReadOnly = NaughtyAttributes.ReadOnlyAttribute;

namespace Views.Battle.Selection
{
    public class Selector : MonoBehaviour
    {
        private LayerMask _selectionMask;
        [SerializeField] private Camera _camera;
        [SerializeField] private PhaseSelector _phase;

        [SerializeField] [ReadOnly] private Selectable _lastSelectable;
        
        [InfoBox("Will find all Hints available in scene on startup and use them")] [SerializeReference] [ReadOnly]
        private SelectionHintManager _hints;

        [FormerlySerializedAs("_onHoverChanges")] [SerializeField]
        private SelectionEvent _onHoverChanged = new();

        [FormerlySerializedAs("_onSelectionUpdates")] [SerializeField]
        private SelectionEvent _selectionUpdated = new();

        [SerializeField] private ResetEvent _reseted = new();


        [SerializeField] [ReadOnly] private RaycastHit[] _results;
        [SerializeField] [ReadOnly] private Dictionary<GameObject, Selectable> _selectables;
        private bool _updateHint = true;

        public SelectionEvent OnHoverChanged => _onHoverChanged;

        public SelectionEvent SelectionUpdated => _selectionUpdated;

        public PhaseSelector Phase => _phase;

        private void AddResetableElement(IReset resetable) => _reseted.AddListener(resetable.Reset);

        public void AddResetables(params IReset[] resetable)
        {
            foreach (var resetable1 in resetable)
            {
                AddResetableElement(resetable1);
            }
        }

        public SelectionHintManager Hints => _hints;

        public bool UpdateHint
        {
            get { return _updateHint; }
            set
            {
                _updateHint = value;
            }
        }

        public void Initialize()
        {
            _updateHint = true;
            var hints = FindObjectsByType<SelectionHint>(FindObjectsSortMode.None);
            Assert.IsTrue(hints != null && hints.Length >= 1);
            _hints = new SelectionHintManager(hints);
            _results = new RaycastHit[4];
            //We have a quick mapping from a gameObject to it's selectable component without the need of a GetComponent on every selection
            Dictionary<GameObject, Selectable> dictionary = new Dictionary<GameObject, Selectable>();
            foreach (Selectable selectable in FindObjectsByType<Selectable>(FindObjectsSortMode.None))
            {
                dictionary.Add(selectable.gameObject, selectable);
            }

            _selectables = dictionary;
            _lastSelectable = dictionary.First().Value;
            RaiseCurrentHover();
            _phase.Initialize(EPhase.Normal);
            _selectionMask = _phase.GetLayerMask();
            StartCoroutine(CheckSelection());
            Reset();
        }

        private void Update()
        {
            if (_lastSelectable != null && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) ||
                                            Input.GetKeyDown(KeyCode.Return)))
            {
                _selectionUpdated.Invoke(_lastSelectable.Selection);
            }

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
            {
                Reset();
            }
        }

        private IEnumerator CheckSelection(float delay = 0.016f)
        {
            while (true)
            {
                Cast();
                yield return new WaitForSeconds(delay);
            }
        }


        private void Cast()
        {
            int result = Physics.RaycastNonAlloc(_camera.ScreenPointToRay(Input.mousePosition), _results,
                Mathf.Infinity, _selectionMask);
            //Possible result = 0 => we don't enter
            for (int i = 0; i < result; i++)
            {
                var selectable = _selectables[_results[i].transform.gameObject];
                if ((selectable != _lastSelectable && _phase.Contains(selectable.Tile.Phase)))
                {
                    _lastSelectable = selectable;
                    RaiseCurrentHover();
                }
            }
        }

        public void RaiseCurrentHover()
        {
            _onHoverChanged.Invoke(_lastSelectable.Selection);
            if (_updateHint)
                _hints.Hint(_lastSelectable, true);
        }

        public void Reset()
        {
            _hints.Clear();
            _hints.ActivateNew();
            UpdateHint = true;
            RaiseCurrentHover();
            _reseted.Invoke();
        }
    }
}