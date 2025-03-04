using System;
using System.Collections.Generic;
using System.Linq;
using BattleSystem.TileSystem;
using NUnit.Framework;
using UI.Battle;
using UnitSystem;
using UnitSystem.Actions.Bases;
using UnityEngine;
using OriginType = UnitSystem.Unit; //IBattleElement;

namespace BattleSystem
{
    public class Action : IComparable<Action>
    {
        //Intermediate class to define a group of targets, used in the abstract methods, we use intermediate class in case we need to use another data structure or have middle logic
        [SerializeField] private OriginType _origin;
        [SerializeField] private TargetCollection _targets;
        [SerializeField] private IActionInfo _info;
        public bool HasTargets => _targets != null && _targets.Count > 0;
        public IEnumerable<IBattleElement> TargetsEnumerable => _targets.Targets;

        [Obsolete(
            "Private setter, targets shouldn't be accessed nor modiified directly, use methods that try to set them instead",
            true)]
        private TargetCollection Targets
        {
            set => _targets = value;
            get => _targets;
        }


        public Action(OriginType origin, IActionInfo info)
        {
            _origin = origin;
            _info = info;
            _targets = new TargetCollection(_info.NbTargets);
        }

        public void Execute()
        {
            _info.Execute(_origin, _targets);
        }

        // ReSharper disable SimplifyConditionalTernaryExpression
        public bool CanExecute(Tilemap map)
        {
            return (IsReady ? _info.CanExecuteOnMap(_origin, _targets, map) : false);
        }
        // ReSharper restore SimplifyConditionalTernaryExpression

        private bool IsReady => _origin != null && _targets != null;

        public OriginType Origin => _origin;

        public IActionInfo Info => _info;

        public bool AreTargetsFull
        {
            get { return _targets.Count >= _info.NbTargets; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="targets"></param>
        /// <returns>True if at least one of the target was succesfuly appended</returns>
        public bool TryAppendTargets(Unit origin, params IBattleElement[] targets)
        {
            Assert.IsTrue(_targets != null, "Targets should be initialized before trying to append to them");
            Assert.IsTrue(_origin != null || origin != null,
                "Origin should be initialized before trying to append to them");
            Assert.IsTrue((_origin == null && origin != null) || origin == _origin,
                "Origin should be the same as the one used to initialize the action");
            if (_origin == null)
                _origin = origin;
            bool flag = false;
            foreach (var t in targets)
            {
                if (_targets.Count < _info.NbTargets)
                {
                    if (_info.AreTargetsValid(_origin, t))
                    {
                        _targets.Add(t);
                        if (!flag) flag = true;
                    }
                }
            }

            //Debug.Log("Target Appending target size : " + _targets.Count);
            return flag;
        }

        [Obsolete(
            "Use TryAppendTargets instead, this would return true if at least only one target sueccesfuly appended but this might not be what you want")]
        public bool TrySetTarget(OriginType origin, params IBattleElement[] target)
        {
            _targets = new TargetCollection(_info.NbTargets);
            _origin = origin;
            if (TryAppendTargets(_origin, target))
                return true;
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