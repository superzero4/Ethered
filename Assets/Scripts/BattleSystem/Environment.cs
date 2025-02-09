using System;
using Common.Visuals;
using UnityEngine;

namespace BattleSystem
{
    [Serializable]
    public class Environment : IBattleElement
    {
        
        public Environment(EnvironmentInfo info, PositionData position)
        {
            this._position = position;
            this._info = info;
        }
        public Environment(EnvironmentInfo info) : this(info, new PositionData(Vector2Int.zero, EPhase.Normal))
        {
        }
        public Environment(EnvironmentInfo info, EPhase phase, Vector2Int position) : this(info,
            new PositionData(position, phase))
        {
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

        public EnvironmentInfo Info => _info;

        void IHealth.TakeDamageUncapped(int damage)
        {
            return;
        }
    }
}