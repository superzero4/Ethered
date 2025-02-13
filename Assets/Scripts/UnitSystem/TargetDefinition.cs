using System;
using BattleSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace UnitSystem
{
    [Serializable]
    public struct TargetDefinition
    {
        [SerializeField, Range(1, 10)] private int _nbTargets;
        [SerializeField, Range(0, 10)] private int _range;
        [SerializeField] private TargetType _targetType;
        [SerializeField] private ERelativePhase _phase;

        public TargetDefinition(ERelativePhase phase, int nbTargets, int range, TargetType targetType)
        {
            _phase = phase;
            _nbTargets = nbTargets;
            _range = range;
            _targetType = targetType;
        }

        [Flags]
        public enum TargetType
        {
            None = 0,
            Self = 1,
            AnyOtherAlly = 2,
            AnyAlly = 3,
            AnyEnemy = 4,
            Ground = 8,
            AnyUnit = 7,
            Anything = 15,
            AllEnemies = 16,
            AllOtherAllies = 32,
            AllAllies = 33
        }

        public bool IsTargetPhaseValid(EPhase phase, IBattleElement target)
        {
            return phase.HasFlag(target.Phase);
        }

        public bool IsValidTarget(IBattleElement origin, params IBattleElement[] targets)
        {
            //TODO IMPORTANT : in the selection system ensure the origin is selected in a way with only one phase, or a speace specifier/override when passing a Unit with a dual phase position
            Assert.IsTrue(origin.Position.Phase.IsOnlyOnOnePhase());
            EPhase phase = _phase.ToPhase(origin.Position.Phase);
            if (targets.Length > _nbTargets)
            {
                return false;
            }

            if (!CheckForAllTargets(origin, targets, phase)) return false;
            return true;
        }

        private bool CheckForAllTargets(IBattleElement origin, IBattleElement[] targets, EPhase phase)
        {
            foreach (var target in targets)
            {
                if (target == null || target.Position.DistanceTo(origin.Position) > _range)
                {
                    return false;
                }

                if (!IsTargetPhaseValid(phase, target))
                {
                    return false;
                }

                bool isSelf = origin == target;
                bool isAlly = origin.Team == target.Team;
                bool isUnit = !target.IsGround;
                //TODO implement concrete logic all vs anything and find a batter way than this order dependant if forest
                if (_targetType.HasFlag(TargetType.Anything))
                {
                    return true;
                }

                if (isUnit)
                {
                    if (_targetType.HasFlag(TargetType.AnyUnit))
                        return true;
                    if (isAlly)
                    {
                        if (_targetType.HasFlag(TargetType.AnyAlly))
                            return true;
                        else if (_targetType.HasFlag(TargetType.AllAllies))
                            return true;
                        if (isSelf)
                        {
                            if (_targetType.HasFlag(TargetType.Self))
                                return true;
                        }
                        else
                        {
                            if (_targetType.HasFlag(TargetType.AnyOtherAlly))
                                return true;
                            if (_targetType.HasFlag(TargetType.AllOtherAllies))
                                return true;
                        }
                    }
                    else
                    {
                        if (_targetType.HasFlag(TargetType.AnyEnemy))
                            return true;
                        else if (_targetType.HasFlag(TargetType.AllEnemies))
                            return true;
                    }
                }
                else
                {
                    if (_targetType.HasFlag(TargetType.Ground))
                        return true;
                }
            }

            return false;
        }
    }
}