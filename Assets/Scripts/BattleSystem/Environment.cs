using System;
using UI.Battle;
using UnityEngine;

namespace BattleSystem
{
    [Serializable]
    public struct Environment : IBattleElement
    {
        public Environment(EPhase phase, Vector2Int position)
        {
            _position = new PositionData(position, phase);
            _team = ETeam.None;
            _visualInformations = new VisualInformations();
        }

        [SerializeField]
        private VisualInformations _visualInformations;
        [SerializeField]
        private PositionData _position;
        [SerializeField]
        private ETeam _team;

        public VisualInformations VisualInformations => _visualInformations;

        public PositionData Position
        {
            get => _position;
            set => _position = value;
        }
        

        public ETeam Team => _team;
    }
}