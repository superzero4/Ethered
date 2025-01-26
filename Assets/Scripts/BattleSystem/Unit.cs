using BattleSystem;
using Common.Events;
using UI.Battle;
using UnityEngine;

namespace UnitSystem
{
    public class Unit : IBattleElement
    {
        [SerializeField] private UnitInfo _info;
        private EPhase _phase;
        private Vector2Int _position;
        private ETeam _team;
        [SerializeField] private UnitEvent _onUnitChange;
        public Unit(UnitInfo info, ETeam team, Vector2Int position, EPhase phase)
        {
            _info = info;
            _team = team;
            _position = position;
            _phase = phase;
        }
        private void RaiseUpdate()
        {
            _onUnitChange?.Invoke(this);
        }

        public EPhase Phase
        {
            get => _phase;
            set
            {
                _phase = value;
                RaiseUpdate();
            }
        }

        public Vector2Int Position
        {
            get => _position;
            set
            {
                _position = value;
                RaiseUpdate();
            }
        }

        public ETeam Team => _team;

        public VisualInformations VisualInformations => _info.VisualInformations;
    }
}