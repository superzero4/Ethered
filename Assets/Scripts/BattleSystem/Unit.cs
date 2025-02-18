using System;
using BattleSystem;
using Common.Events;
using Common.Visuals;
using UnityEngine;

namespace UnitSystem
{
    [Serializable]
    public class Unit : IBattleElement
    {
        [SerializeField] private UnitInfo _info;
        [SerializeField] private PositionData _position;
        [SerializeField] private ETeam _team;
        [SerializeField] private UnitMovementEvent _onUnitMoves;
        [SerializeField] private UnitHealthEvent _onUnitHealthChange;
        [SerializeField] private int _currentHealth;
        private int _health;
        public UnitInfo Info => _info;

        public Unit(UnitInfo info, ETeam team, Vector2Int position, EPhase phase)
        {
            _info = info;
            _team = team;
            _currentHealth = MaxHealth;
            _position = new PositionData(position, phase);
            _onUnitMoves = new UnitMovementEvent();
            _onUnitHealthChange = new UnitHealthEvent();
        }

        public void Move(Vector2Int newPosition, EPhase newPhase) => Move(new PositionData(newPosition, newPhase));

        public void Move(PositionData newPosition)
        {
            var eventData = new UnitMovementData() { unit = this, oldPosition = _position };
            _position = newPosition;
            _onUnitMoves?.Invoke(eventData);
        }

        public PositionData Position
        {
            get => _position;
        }

        public ETeam Team => _team;

        public EAllowedMovement allowedMovement => EAllowedMovement.Cross;

        public VisualInformations VisualInformations => _info.VisualInformations;

        public UnitMovementEvent OnUnitMoves => _onUnitMoves;
        public UnitHealthEvent OnUnitHealthChange => _onUnitHealthChange;
        public IHealth HealthInfo => this;
        int IHealth.CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = value;
        }

        public int MaxHealth => _info.MaxHealth;



        void IHealth.TakeDamageUncapped(int damage, IBattleElement source)
        {
            var data = new UnitHitData()
                { unit = this, oldHealth = _currentHealth, direction = _position.Position - source.Position.Position };
            _currentHealth -= damage;
            _onUnitHealthChange?.Invoke(data);
        }
    }
}