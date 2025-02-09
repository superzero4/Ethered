using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BattleSystem;
using Common.Events;
using NaughtyAttributes;
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
        [SerializeField] private SelectionHint _selectionHint;
        [SerializeField] private SelectionEvent _onHoverChanges = new();
        [SerializeField] private SelectionEvent _onSelectionUpdates = new();

        [SerializeField] [ReadOnly] private Selectable _lastSelectable;

        [SerializeField] [ReadOnly] private RaycastHit[] _results;
        [SerializeField] [ReadOnly] private Dictionary<GameObject, Selectable> _selectables;

        public SelectionEvent OnHoverChanges => _onHoverChanges;

        public SelectionEvent OnSelectionUpdates => _onSelectionUpdates;

        public PhaseSelector Phase => _phase;

        public void Initialize()
        {
            _results = new RaycastHit[4];
            //We have a quick mapping from a gameObject to it's selectable component without the need of a GetComponent on every selection
            Dictionary<GameObject, Selectable> dictionary = new Dictionary<GameObject, Selectable>();
            foreach (Selectable selectable in FindObjectsByType<Selectable>(FindObjectsSortMode.None))
            {
                dictionary.Add(selectable.gameObject, selectable);
            }
            _selectables = dictionary;
            _lastSelectable = dictionary.First().Value;
            _selectionHint.Hint(_lastSelectable, true);
            _phase.Initialize(EPhase.Normal);
            _selectionMask = _phase.GetLayerMask();
            StartCoroutine(CheckSelection());
        }

        private void Update()
        {
            if (_results != null && Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.Return))
            {
                _onSelectionUpdates.Invoke(_lastSelectable.Selection);
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
                    _selectionHint.Hint(selectable, true);
                    _onHoverChanges.Invoke(selectable.Selection);
                }
            }
        }
    }
}