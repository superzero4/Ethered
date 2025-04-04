using System;
using System.Collections.Generic;
using System.Linq;
using UnitSystem.Actions.Bases;
using Common.Visuals;
using UnityEngine;

namespace UnitSystem
{
    [System.Serializable]
    public class UnitInfo : IIcon
    {
        [SerializeField, Range(1, 100)] private int _maxHealth;
        [SerializeField] private int _armor;
        private List<IActionInfo> _actions;
        [SerializeField] private ActionCollection _actionCollection;

        [SerializeField] private VisualInformations _visualInformations;

        public VisualInformations VisualInformations
        {
            get => _visualInformations;
            set => _visualInformations = value;
        }

        public int MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value;
        }

        public int Armor
        {
            get => _armor;
            set => _armor = value;
        }

        //We build the action list the first time we try to access it, we depend only on the IActioninfo now and not on the scriptable we use to provide thos
        /// <summary>
        /// This return the action list interface, it can be modified suppressed or added to change the action set of character, the base set is the one specified by it's SkillSet
        /// </summary>
        public List<IActionInfo> Actions => _actions==null || _actions.Count == 0 ? _actions = _actionCollection.Actions.ToList() : _actions;

        private ActionCollection Collection
        {
            get => _actionCollection;
            [Obsolete("This field is only for serialization and initialisation, ignored after Action has been consulted one, use the action getter and modify directly the list",error:true)]
            set => _actionCollection = value;
        }
        
        public UnitInfo()
        {
            _actions = new List<IActionInfo>();
        }
    }
}