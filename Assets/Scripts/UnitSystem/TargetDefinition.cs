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
        public TargetDefinition(int nbTargets, int range, TargetType targetType)
        {
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

        [Obsolete(
            "Might be used but as far as we constrain starting phase and target phase we actually don't need it except we want lot attack starting from any phase but hitting only a relative phase (same or opposite) but we can actually implement that with the targetDefinition list")]
        

        [SerializeField, Range(1, 10)] private int _nbTargets;
        [SerializeField, Range(0, 10)] private int _range;
        [SerializeField] private TargetType _targetType;

        public bool IsTargetPhaseValid(ERelativePhase relativePhase, EPhase originPhase, IBattleElement target)
        {
            return IsTargetPhaseValid(ConvertRelativePhase(relativePhase, originPhase), target);
        }

        public static EPhase ConvertRelativePhase
            (ERelativePhase relativePhase, EPhase originPhase)
        {
            EPhase phase = EPhase.None;
            switch (relativePhase)
            {
                case ERelativePhase.None:
                    phase = EPhase.None;
                    break;
                case ERelativePhase.Same:
                    phase = originPhase;
                    break;
                case ERelativePhase.Opposite:
                    phase = phase == EPhase.Both ? EPhase.Both : originPhase ^ EPhase.Both;
                    break;
                case ERelativePhase.All:
                    phase = EPhase.Both;
                    break;
            }

            return phase;
        }

        public bool IsTargetPhaseValid(EPhase phase, IBattleElement target)
        {
            return phase.HasFlag(target.Phase);
        }

        public bool IsValidTarget(IBattleElement origin, EPhase phase, params IBattleElement[] targets)
        {
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
                if(!IsTargetPhaseValid(phase, target))
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