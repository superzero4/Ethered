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
        [SerializeField, Range(0, 10)] private int _range;
        [SerializeField] private TargetType _targetType;
        [SerializeField] private ERelativePhase _phase;

        public TargetDefinition(ERelativePhase phase, int range, TargetType targetType)
        {
            _phase = phase;
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
        
        public bool AreValidTargets(IBattleElement origin, params IBattleElement[] targets)
        {
            EPhase phase = _phase.ToPhase(origin.Position.Phase);
            foreach (var target in targets)
            {
                if (IsValidTarget(origin, phase, target, out bool checkForAllTargets)) return checkForAllTargets;
            }

            return false;
        }

        public bool IsValidTarget(IBattleElement origin, EPhase phase, IBattleElement target, out bool checkForAllTargets)
        {
            if (target == null || target.Position.DistanceTo(origin.Position) > _range)
            {
                checkForAllTargets = false;
                return false;
            }

            if (!IsTargetPhaseValid(phase, target))
            {
                checkForAllTargets = false;
                return true;
            }

            bool isSelf = origin == target;
            bool isAlly = origin.Team == target.Team;
            bool isUnit = !target.IsGround;
            //TODO implement concrete logic all vs anything and find a batter way than this order dependant if forest
            if (_targetType.HasFlag(TargetType.Anything))
            {
                checkForAllTargets = true;
                return true;
            }

            if (isUnit)
            {
                if (_targetType.HasFlag(TargetType.AnyUnit))
                {
                    checkForAllTargets = true;
                    return true;
                }

                if (isAlly)
                {
                    if (_targetType.HasFlag(TargetType.AnyAlly))
                    {
                        checkForAllTargets = true;
                        return true;
                    }
                    else if (_targetType.HasFlag(TargetType.AllAllies))
                    {
                        checkForAllTargets = true;
                        return true;
                    }

                    if (isSelf)
                    {
                        if (_targetType.HasFlag(TargetType.Self))
                        {
                            checkForAllTargets = true;
                            return true;
                        }
                    }
                    else
                    {
                        if (_targetType.HasFlag(TargetType.AnyOtherAlly))
                        {
                            checkForAllTargets = true;
                            return true;
                        }

                        if (_targetType.HasFlag(TargetType.AllOtherAllies))
                        {
                            checkForAllTargets = true;
                            return true;
                        }
                    }
                }
                else
                {
                    if (_targetType.HasFlag(TargetType.AnyEnemy))
                    {
                        checkForAllTargets = true;
                        return true;
                    }
                    else if (_targetType.HasFlag(TargetType.AllEnemies))
                    {
                        checkForAllTargets = true;
                        return true;
                    }
                }
            }
            else
            {
                if (_targetType.HasFlag(TargetType.Ground))
                {
                    checkForAllTargets = true;
                    return true;
                }
            }
            checkForAllTargets = false;
            return false;
        }
    }
}