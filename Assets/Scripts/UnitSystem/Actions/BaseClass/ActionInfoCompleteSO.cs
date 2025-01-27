using System;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

namespace UnitSystem.Actions.Bases
{
    //it still requires to be extended to create specific specific behaviours but they'll inerhit the partialimplementation as fields and the Scriptable object structure
    /// <summary>
    /// This class serves as main Implementation for Action whom phase and target constraint can be modified for each one, in a scriptable
    /// </summary>
    [Serializable]
    public abstract class ActionInfoCompleteSO : ActionInfoBaseSO, IActionInfo
    {
        [SerializeField] private EPhase _originPhase;
        [Tooltip("Will mostly be a one element lits, target will be valid if it matches at least one of the target definitions (For instance an ability could have a different reach depending on the target's phase)")]
        [SerializeField] private List<TargetDefinition> _target;
        public override EPhase OriginPhase => _originPhase;
        public override IEnumerable<TargetDefinition> Target => _target;
    }
}