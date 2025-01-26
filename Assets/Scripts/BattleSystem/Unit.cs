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
        [SerializeField] private UnitEvent _onUnitMoves;

        public Unit(UnitInfo info, ETeam team, Vector2Int position, EPhase phase)
        {
            _info = info;
            _team = team;
            _position = new PositionData(position, phase);
        }

        public void Move(Vector2Int newPosition, EPhase newPhase) => Move(new PositionData(newPosition, newPhase));

        public void Move(PositionData newPosition)
        {
            var oldPosition = _position;
            _position = newPosition;
            _onUnitMoves.Invoke(new UnitEventData { unit = this, oldPosition = oldPosition });
        }

        public PositionData Position
        {
            get => _position;
        }

        public ETeam Team => _team;

        public VisualInformations VisualInformations => _info.VisualInformations;

        public UnitEvent OnUnitMoves => _onUnitMoves;

        public EPhase Phase => ((IBattleElement)(this)).Phase;
    }
}