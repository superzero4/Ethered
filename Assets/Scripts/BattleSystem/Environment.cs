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
            _phase = phase;
            _position = position;
            _team = ETeam.None;
            _visualInformations = new VisualInformations();
        }

        [SerializeField]
        private VisualInformations _visualInformations;
        [SerializeField]
        private EPhase _phase;
        [SerializeField]
        private Vector2Int _position;
        private ETeam _team;

        public VisualInformations VisualInformations => _visualInformations;

        public EPhase Phase
        {
            get => _phase;
            set => _phase = value;
        }

        public Vector2Int Position
        {
            get => _position;
            set => _position = value;
        }

        public ETeam Team => _team;
    }
}