using System.Collections.Generic;
using UnitSystem.Actions.Bases;
using UnityEngine;

namespace UnitSystem.Actions.Bases
{
    [CreateAssetMenu(fileName = "SkillSet", menuName = "Actions/ActionCollection")]
    public class ActionCollection : ScriptableObject
    {
        [SerializeField] private string _archetypeName;
        [SerializeField] private ActionInfoBaseSO[] _actions;
        public IEnumerable<IActionInfo> Actions => _actions;
        public new string name => string.IsNullOrEmpty(_archetypeName) ? base.name : _archetypeName;
    }
}