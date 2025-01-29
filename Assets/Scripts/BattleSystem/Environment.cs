using System;
using UI.Battle;
using UnityEngine;

namespace BattleSystem
{
    [Serializable]
    public struct Environment : IBattleElement
    {
        public Environment(EPhase phase, Vector2Int position, EAllowedMovement movementAllowed)
        {
            _position = new PositionData(position, phase);
            _team = ETeam.None;
            _movementAllowed = movementAllowed;
            _visualInformations = new VisualInformations();
        }

        [SerializeField] private VisualInformations _visualInformations;
        [SerializeField] private PositionData _position;
        [SerializeField] private ETeam _team;

        [SerializeField] private EAllowedMovement _movementAllowed;
        public EAllowedMovement allowedMovement => _movementAllowed;
        public VisualInformations VisualInformations => _visualInformations;

        public PositionData Position
        {
            get => _position;
            set => _position = value;
        }


        public ETeam Team => _team;

        //By default environment isn't destructible or damageable
        public int CurrentHealth => 0;

        public int MaxHealth => 0;

        public void TakeDamage(int damage)
        {
            return;
        }
    }
}