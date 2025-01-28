using System;
using System.Collections.Generic;
using System.Linq;
using UI.Battle;
using UnitSystem;
using UnitSystem.Actions.Bases;
using UnityEngine;
using OriginType = UnitSystem.Unit;//IBattleElement;
namespace BattleSystem
{
    public class Action : IComparable<Action>
    {
        [SerializeField] private OriginType _origin;
        [SerializeField] private TargetCollection _targets;
        [SerializeField] private IActionInfo _info;

        public Action(OriginType origin, IActionInfo info)
        {
            _origin = origin;
            _info = info;
        }

        public void Execute()
        {
            _info.Execute(_origin, _targets);
        }

        // ReSharper disable SimplifyConditionalTernaryExpression
        public bool CanExecute(Battle.Tilemap map)
        {
            return (IsReady ?  _info.CanExecuteOnMap(_origin, _targets, map) : false);
        }
        // ReSharper restore SimplifyConditionalTernaryExpression

        private bool IsReady => _origin != null && _targets != null;

        public bool TrySetTarget(OriginType origin, params IBattleElement[] target)
        {
            if (_info.IsValidTarget(origin, target))
            {
                _origin = origin;
                _targets = new TargetCollection(target.ToList());
                return true;
            }
            else
            {
                _origin = null;
                _targets = null;
                return false;
            }
        }

        public int CompareTo(Action other)
        {
            throw new NotImplementedException(
                "In case we ever need some kind of speed or priority system, we could implement it here as a comparable (used in other parts of the code, that will fail if we decide to use them and use a priority system without implementing that)");
        }
    }
}