using System;
using BattleSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnitSystem
{
    [Serializable]
    public struct TargetDefinition
    {
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
            throw new NotImplementedException("TODO from range, phase ant target type");
        }
    }
}