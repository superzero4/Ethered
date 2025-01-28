using System.Collections.Generic;
using BattleSystem;
using UI.Battle;
using UnitSystem.Actions.Bases;
using UnityEngine;

namespace UnitSystem
{
    
    [System.Serializable]
    public class UnitInfo : IIcon
    {
        [SerializeField,Range(1,100)] private int _maxHealth;
        [SerializeField] private List<IActionInfo> _actions;
        
        [SerializeField] private VisualInformations _visualInformations;
        public VisualInformations VisualInformations => _visualInformations;
        public int MaxHealth => _maxHealth;
    }
}