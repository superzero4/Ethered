using System;
using System.Collections.Generic;
using BattleSystem;
using BattleSystem.TileSystem;
using Common.Events;
using Common.Events.Combat;
using Common.Visuals;
using UnityEngine;

namespace UnitSystem
{
    [Serializable]
    public class Unit : IBattleElement
    {
        [SerializeReference] private UnitInfo _info;
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
        public void Move(PathWrapper newPosition)
        {
            var eventData = new UnitMovementData() { unit = this, path = newPosition };
            _position = newPosition.Path[^1];
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

        //TODO implement that using the unit info and upgrade system if it reveals to be used, calling code use this even if it's currently a constant value
        public int ActionsPerTurn => 1;


        void IHealth.TakeDamageUncapped(int damage, IBattleElement source)
        {
            var data = new UnitHitData()
                { unit = this, oldHealth = _currentHealth, direction = _position.Position - source.Position.Position };
            _currentHealth -= damage;
            _onUnitHealthChange?.Invoke(data);
        }
    }
}