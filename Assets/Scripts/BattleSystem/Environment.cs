using System;
using UI.Battle;
using UnityEngine;

namespace BattleSystem
{
    [Serializable]
    public class Environment : IBattleElement
    {
        public Environment(EnvironmentInfo info, EPhase phase, Vector2Int position)
        {
            _position = new PositionData(position, phase);
            this._info = info;
        }

        [SerializeField] private EnvironmentInfo _info;
        [SerializeField] private PositionData _position;

        public EAllowedMovement allowedMovement => _info.AllowedMovement;
        public VisualInformations VisualInformations => _info.VisualInformations;

        public PositionData Position
        {
            get => _position;
            set => _position = value;
        }

        public ETeam Team => ETeam.None;

        int IHealth.CurrentHealth
        {
            get => 0;
            set { return; }
        }

        public int MaxHealth => 0;

        void IHealth.TakeDamageUncapped(int damage)
        {
            return;
        }
    }
}