using System;
using BattleSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnitSystem
{
    [Serializable]
    public struct TargetDefinition
    {
        public TargetDefinition(EPhase phase, int nbTargets, int range, TargetType targetType)
        {
            _phase = phase;
            _nbTargets = nbTargets;
            _range = range;
            _targetType = targetType;
        }

        [Flags]
        public enum TargetType
        {
            None = 0, Self=1, OtherAlly=2, AnyAlly = 3, AnyEnemy=4, Ground=8, AnyUnit=7, Anything = 15, AllEnemies = 16, AllOtherAllies = 32, AllAllies = 33
        }
        [Obsolete("Might be used but as far as we constrain starting phase and target phase we actually don't need it except we want lot attack starting from any phase but hitting only a relative phase (same or opposite) but we can actually implement that with the targetDefinition list",true)]
        [Flags]
        public enum TargetPhase
        {
            None = 0, Same = 1, Opposite = 2, All = Same | Opposite
        }
        [SerializeField] private EPhase _phase;
        [SerializeField,Range(1,10)]
        private int _nbTargets;
        [SerializeField,Range(0,10)]
        private int _range;
        [SerializeField]
        private TargetType _targetType;
        public bool IsValidTarget(IBattleElement origin, params IBattleElement[] targets)
        {
            if (targets.Length > _nbTargets)
            {
                return false;
            }
            foreach (var target in targets)
            {
                if(target.Position.DistanceTo(origin.Position) > _range)
                {
                    return false;
                }
                if (!_phase.HasFlag(target.Phase))
                {
                    return false;
                }
                bool isSelf = origin == target;
                bool isAlly = origin.Team == target.Team;
                bool isUnit = !target.IsGround;
                switch(_targetType)
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