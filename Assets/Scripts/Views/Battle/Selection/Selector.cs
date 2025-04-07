using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BattleSystem;
using Common;
using Common.Events.UserInteraction;
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

        [Header("References")] [SerializeField]
        private Camera _camera;

        [SerializeField] private PhaseSelector _phase;


        [Header("Events")] [SerializeField] private SelectionEvent _hoverChanged = new();

        [SerializeField] private SelectionEvent _selectionUpdated = new();

        [SerializeField] private ResetEvent _reseted = new();

        [Header("ReadOnly")] [SerializeField] [ReadOnly]
        private Selectable _current;

        [SerializeReference, ReadOnly] int previousSelectedLevel;

        [InfoBox("Will find all Hints available in scene on startup and use them")] [SerializeReference] [ReadOnly]
        private IHints _hints;

        [SerializeField] [ReadOnly] private int _hintLevel = 0;

        private RaycastHit[] _results;
        private Dictionary<GameObject, Selectable> _selectables;

        public SelectionEvent HoverChanged => _hoverChanged;

        public SelectionEvent SelectionUpdated => _selectionUpdated;

        public PhaseSelector Phase => _phase;
        public IHints Hints => _hints;

        public bool ShowHints
        {
            get => _hintLevel > 0;
            set { _hintLevel = value ? 2 : 0; }
        }


        private void AddResetableElement(IReset resetable) => _reseted.AddListener(resetable.Reset);

        public void AddResetables(params IReset[] resetable)
        {
            foreach (var resetable1 in resetable)
            {
                AddResetableElement(resetable1);
            }
        }


        public void Initialize(IEnumerable<Selectable> selectables)
        {
            ShowHints = true;
            //We have a quick mapping from a gameObject to it's selectable component without the need of a GetComponent on every selection
            _selectables = new(selectables.Select(s => new KeyValuePair<GameObject, Selectable>(s.gameObject, s)));
            _hints = new TileHints(_selectables.Values);
            _results = new RaycastHit[4];
            _current = _selectables.First().Value;
            RaiseCurrentHover();
            _phase.Initialize(EPhase.Normal);
            _selectionMask = _phase.GetLayerMask();
            StartCoroutine(CheckSelection());
            Reset();
        }

        private void Update()
        {
            if (_current != null && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) ||
                                     Input.GetKeyDown(KeyCode.Return)))
                _selectionUpdated.Invoke(_current.Selection);

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
                Reset();
        }

        private IEnumerator CheckSelection(float delay = 0.016f)
        {
            while (true)
            {
                CastRays();
                yield return new WaitForSeconds(delay);
            }
        }


        private void CastRays()
        {
            int result = Physics.RaycastNonAlloc(_camera.ScreenPointToRay(Input.mousePosition), _results,
                Mathf.Infinity, _selectionMask);
            //Possible result = 0 => we don't enter
            for (int i = 0; i < result; i++)
            {
                var selectable = _selectables[_results[i].transform.gameObject];
                if ((selectable != _current && _phase.Contains(selectable.Tile.Phase)))
                {
                    if (ShowHints)
                    {
                        _current.Hint.Level = previousSelectedLevel;
                        _current = selectable;
                        previousSelectedLevel = _current.Hint.Level;
                        _current.Hint.Level = _hintLevel;
                        RaiseCurrentHover();
                    }
                }
            }
        }

        public void RaiseCurrentHover()
        {
            _hoverChanged.Invoke(_current.Selection);
        }

        public void Reset()
        {
            _hints.Clear();
            //_hints.ActivateNew();
            ShowHints = true;
            previousSelectedLevel = 0;
            RaiseCurrentHover();
            _reseted.Invoke();
        }
    }
}