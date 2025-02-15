using System.Collections.Generic;
using BattleSystem;
using BattleSystem.TileSystem;
using Common;
using Common.Events;
using JetBrains.Annotations;
using UnitSystem;
using UnitSystem.Actions.Bases;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Views.Battle.Selection
{
    [System.Serializable]
    public class SelectionState : IReset
    {
        [SerializeReference] private Unit _origin;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="environment"></param>
        /// <returns>True if either unit or environment in selection was considered a correct target</returns>
        public bool AppendTarget(SelectionEventData selection)
        {
            Assert.IsTrue(CanSelectTarget,
                $"Unit {_origin} or Action {_action} is not set before trying to set targets");
            //Action could either target the environment or the potentally null unit on itself, we pass both, each action will filter them individually and add them to target list if they are valid targets (potentially both or none)
            return _action.TryAppendTargets(_origin, selection.unit, selection.environment);
        }

        [CanBeNull]
        public Action Confirm()
        {
            //Assert.IsTrue(_origin != null, "Unit is not set");
            //Assert.IsTrue(_action != null, "Action is not set");
            //Assert.IsTrue(_action != null && _action.HasTargets, "Target is not set or empty");
            return _action;
        }

        public void SelectActionIfValid(IActionInfo action)
        {
            if (this.CanSelectAction)
            {
                if (action.CouldUnitExecute(_origin))
                {
                    this.SetAction(action);
                    //return true;
                }
            }
            else
            {
                _action = null;
                Debug.LogWarning("SELECTION Action not selected: " + action);
                //return false;
            }
        }
    }
}