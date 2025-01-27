using System.Collections.Generic;
using BattleSystem;
using UI.Battle;
using UnityEngine;

namespace UnitSystem.Actions.Bases
{
    /// <summary>
    /// This class serves as most versatile Implementation for Action Info even if not required, it implements it as a Scriptable Object with one field for VisualInformation it still requires to be extended to provide for targets and phase constraint and to create specific specific behaviours
    /// </summary>
    public abstract class ActionInfoBaseSO : ScriptableObject, IActionInfo
    {
        [SerializeField] private VisualInformations _visualInformations;
        public virtual VisualInformations VisualInformations => _visualInformations;

        public abstract EPhase OriginPhase { get; }
        public abstract EPhase TargetPhase(EPhase currentPhase);
        public abstract IEnumerable<TargetDefinition> Target { get; }
        public abstract bool CanExecuteOnMap(Unit origin, TargetCollection targets, Battle.Tilemap map);
        public abstract void Execute(Unit origin, TargetCollection targetCollection);
    }
}