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
        [SerializeField]
        private EPhase _phase;
        [SerializeField,Range(1,10)]
        private int _nbTargets;
        [SerializeField,Range(0,10)]
        private int _range;
        [SerializeField]
        private TargetType _targetType;
        public bool IsValidTarget(IBattleElement origin, params IBattleElement[] targets)
        {
            throw new NotImplementedException("TODO");
        }
    }
}