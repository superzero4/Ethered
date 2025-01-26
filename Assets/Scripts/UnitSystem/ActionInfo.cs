using System.Collections.Generic;
using BattleSystem;
using UI.Battle;
using UnityEngine;

namespace UnitSystem
{
    [System.Serializable]
    public abstract class ActionInfo : IIcon
    {
        [SerializeField] private EPhase _originPhase;
        [SerializeField] private TargetDefinition _target;
        public TargetDefinition Target => _target;

        public void Execute(IBattleElement origin, IBattleElement target)
        {
            Execute(origin, new TargetCollection(target));
        }

        [SerializeField] private VisualInformations _visualInformations;
        public VisualInformations VisualInformations => _visualInformations;

        public abstract void Execute(IBattleElement origin, TargetCollection targetCollection);
        public abstract bool CanExecute(IBattleElement origin, TargetCollection targets, Battle.Tilemap map);
    }
}