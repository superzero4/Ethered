using System.Collections.Generic;
using BattleSystem;
using UI.Battle;
using UnityEngine;

namespace UnitSystem
{
    
    [System.Serializable]
    public class UnitInfo : IIcon
    {
        [SerializeField] private List<IActionInfo> _actions;
        
        [SerializeField] private VisualInformations _visualInformations;
        public VisualInformations VisualInformations => _visualInformations;
    }
}