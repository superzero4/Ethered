using BattleSystem;
using Common.Events;
using UI.Battle;
using UnityEngine;

namespace UnitSystem
{
    public class Unit : IBattleElement
    {
        [SerializeField] private UnitInfo _info;
        private PositionData _position;
        private ETeam _team;
        [SerializeField] private UnitMovementEvent _onUnitMoves;
        [SerializeField] private UnitHealthEvent _onUnitHealthChange;
        private int _currentHealth;

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

        public VisualInformations VisualInformations => _info.VisualInformations;

        public UnitMovementEvent OnUnitMoves => _onUnitMoves;
        public UnitHealthEvent OnUnitHealthChange => _onUnitHealthChange;

        public int CurrentHealth => _currentHealth;

        public int MaxHealth => _info.MaxHealth;

        public void TakeDamage(int damage)
        {
            var data = new UnitHealthData() { unit = this, oldHealth = _currentHealth };
            _currentHealth -= damage;
            _onUnitHealthChange?.Invoke(data);
        }
    }
}