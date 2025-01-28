using System;
using BattleSystem;
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
            OtherAlly = 2,
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
            Assert.IsTrue(origin.Position.Phase.IsOnlyOnOnePhase());
            EPhase phase = _phase.ToPhase(origin.Position.Phase);
            if (targets.Length > _nbTargets)
            {
                return false;
            }

            foreach (var target in targets)
            {
                if (target.Position.DistanceTo(origin.Position) > _range)
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
                switch (_targetType)
                {
                    case TargetType.Self:
                        if (!isSelf) return false;
                        break;
                    case TargetType.OtherAlly:
                        if (isSelf || !isAlly) return false;
                        break;
                    case TargetType.AnyAlly:
                        if (!isAlly) return false;
                        break;
                    case TargetType.AnyEnemy:
                        if (isAlly) return false;
                        break;
                    case TargetType.Ground:
                        if (isUnit) return false;
                        break;
                    case TargetType.AnyUnit:
                        if (!isUnit) return false;
                        break;
                    //TODO implement the all vs any logic
                    //case TargetType.AllEnemies:
                    //    if (origin.Team == target.Team) return false;
                    //    break;
                    //case TargetType.AllOtherAllies:
                    //    if (origin.Team == target.Team) return false;
                    //    break;
                    //case TargetType.AllAllies:
                    //    if (origin.Team != target.Team) return false;
                    //    break;
                    case TargetType.Anything:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return true;
        }
    }
}