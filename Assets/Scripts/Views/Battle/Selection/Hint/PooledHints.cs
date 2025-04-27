using System;
using System.Collections.Generic;
using BattleSystem;
using Common;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Views.Battle.Selection
{
    [Serializable]
    public class PooledHints : MonoBehaviour, IHints
    {
        [Header("Settings")] [SerializeField, Tooltip("Not implemented yet")]
        private bool _phased;


        private Grid _grid;

        [SerializeField] private SelectionHint _prefab;
        [SerializeField] private Transform _parent;

        [FormerlySerializedAs("_memberPool")] [SerializeReference]
        private Pool<SelectionHint> _hints;

        private int _count;

        public void Init(int count, Grid grid)
        {
            _grid = grid;
            _hints = new(_prefab, count, _parent == null ? _grid.transform : _parent);
            Reset();
        }

        public void Reset()
        {
            _count = 0;
            _hints.Reset();
            foreach (var hint in _hints.Elements)
            {
                hint.Deactivate();
            }
        }

        public void HintMultiple(IEnumerable<PositionData> positions, PositionData? main = default)
        {
            var positionList = new List<PositionData>(positions);
            if (main != null)
                positionList.Add(main.Value);
            _hints?.SetElements(positionList,
                (position, hint) =>
                {
                    hint.Level = main.HasValue && position == main.Value ? 2 : 1;
                    var pos = _grid.PhasedCellToWorld(position);
                    hint.transform.position = pos;
                });
        }
    }
}