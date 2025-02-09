using System;
using UI.Battle;
using UnityEngine;
using UnityEngine.Serialization;

namespace BattleSystem
{
    [Serializable]
    public struct EnvironmentInfo : IIcon
    {
        public EnvironmentInfo(VisualInformations visualInformations, EAllowedMovement allowedMovement)
        {
            _visualInformations = visualInformations;
            this._allowedMovement = allowedMovement;
        }

        [SerializeField] private VisualInformations _visualInformations;
        public VisualInformations VisualInformations => _visualInformations;

        public EAllowedMovement AllowedMovement => _allowedMovement;

        [FormerlySerializedAs("_movementAllowed")] [SerializeField]
        private EAllowedMovement _allowedMovement;
    }
}