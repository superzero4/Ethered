using System;
using System.Collections.Generic;
using BattleSystem;
using UI.Battle;
using UnityEngine;

namespace UnitSystem.Actions.Bases
{
    //it still requires to be extended to create specific specific behaviours but they'll inerhit the partialimplementation as fields and the Scriptable object structure
    /// <summary>
    /// This class serves as most versatile Implementation for Action Info even if not required, it implements it as a Scriptable Object with one field for VisualInformation it still requires to be extended to provide for targets and phase constraint and to create specific specific behaviours
    /// </summary>
    public abstract class ActionInfoBaseSO : ScriptableObject, IActionInfo
    {
        [SerializeField] private VisualInformations _visualInformations;
        public virtual VisualInformations VisualInformations => _visualInformations;

        public abstract EPhase OriginPhase { get; }
        public abstract IEnumerable<TargetDefinition> Target { get; }
        public abstract bool CanExecuteOnMap(Unit origin, TargetCollection targets, Battle.Tilemap map);
        public abstract void Execute(Unit origin, TargetCollection targetCollection);
    }
    /// <summary>
    /// This class serves as main Implementation for Action whom phase and target constraint can be modified for each one, in a scriptable
    /// </summary>
    [Serializable]
    public abstract class ActionInfoCompleteSO : ActionInfoBaseSO, IActionInfo
    {
        [SerializeField] private EPhase _originPhase;
        [Tooltip("Will mostly be a one element lits, target will be valid if it matches at least one of the target definitions (For instance an ability could have a different reache depending on the target's phase)")]
        [SerializeField] private List<TargetDefinition> _target;
        
        public override EPhase OriginPhase => _originPhase;
        public override IEnumerable<TargetDefinition> Target => _target;
    }
}