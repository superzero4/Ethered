using System;
using System.Collections.Generic;
using BattleSystem;
using Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace Views.Battle.Selection
{
    [Serializable]
    public class PooledHints : MonoBehaviour, IHints
    {
        [Header("Settings")] [SerializeField, Tooltip("Not implemented yet")]
        private bool _phased;

        [Header("References")] [SerializeField]
        private Grid _grid;

        [SerializeField] private SelectionHint _prefab;
        [SerializeField] private Transform _parent;
        [FormerlySerializedAs("_memberPool")] [SerializeReference] private Pool<SelectionHint> _hints;
        private int _count;

        private void Awake()
        {
            _hints = new(_prefab, 4, _parent);
            Clear();
        }

        public void Clear()
        {
            _count = 0;
            _hints.Reset();
            foreach (var hint in _hints.Elements)
            {
                hint.Deactivate();
            }
        }

        public void Hint(PositionData s)
        {
            HintMultiple(new[] { s });
        }

        public void HintMultiple(IEnumerable<PositionData> positions)
        {
            _hints.SetElements(positions,
                (position, hint) =>
                {
                    hint.Level = 1;
                    var pos = _grid.PhasedCellToWorld(position);
                    hint.transform.position = pos;
                });
        }
    }
}