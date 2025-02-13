using System.Collections.Generic;
using BattleSystem;
using UnitSystem;
using UnitSystem.Actions.Bases;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Views.Battle.Selection
{
    [System.Serializable]
    public class SelectionState
    {
        [SerializeField] private Unit _origin;
        [SerializeReference] private Action _action;
        public bool CanSelectUnit => _origin == null;
        public bool CanSelectAction => !CanSelectUnit && _action == null;
        public bool CanSelectTarget => !CanSelectUnit && !CanSelectAction;
        public SelectionState()
        {
            Reset();
        }

        public void Reset()
        {
            _origin = null;
            _action = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="force">We'll set selected unit even if another one is already selected, without having to perform a reset</param>
        public void SetUnit(Unit unit, bool force = true)
        {
            Assert.IsTrue(force || CanSelectUnit, "Unit is already set and not forced");
            _origin = unit;
        }

        public void SetAction(IActionInfo action)
        {
            Assert.IsTrue(CanSelectAction, "Unit is not set before trying to set action");
            Assert.IsTrue(action != null && _origin.Info.Actions != null && _origin.Info.Actions.Contains(action),
                $"Action is incorrect => Unit doesn't have action {action} in list {_origin.Info.Actions}");
            _action = new Action(_origin, action);
        }

        public bool AppendTarget(Unit target)
        {
            Assert.IsTrue(CanSelectTarget, $"Unit {_origin} or Action {_action} is not set before trying to set targets");
            return _action.TryAppendTarget(_origin, target);
        }
        
        public Action Confirm()
        {
            Assert.IsTrue(_origin != null, "Unit is not set");
            Assert.IsTrue(_action != null, "Action is not set");
            Assert.IsTrue(_action != null && _action.HasTargets, "Target is not set or empty");
            return _action;
        }

        public void SelectActionIfValid(IActionInfo action)
        {
            if (this.CanSelectAction)
                this.SetAction(action);
            else
            {
                Debug.LogWarning("SELECTION Action not selected: " + action);
            }
        }
    }
}