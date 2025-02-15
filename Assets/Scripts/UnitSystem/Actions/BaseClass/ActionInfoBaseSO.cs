using System.Collections.Generic;
using BattleSystem;
using BattleSystem.TileSystem;
using Common.Visuals;
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
        public abstract IEnumerable<TargetDefinition> PossibleTargets { get; }

        public abstract int NbTargets { get; }

        public abstract bool CanExecuteOnMap(Unit origin, TargetCollection targets, Tilemap map);
        public abstract void Execute(Unit origin, TargetCollection targetCollection);
    }
}