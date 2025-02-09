using System.Collections.Generic;
using UnitSystem.Actions.Bases;
using UnityEngine;

namespace UnitSystem.Actions.Bases
{
    [CreateAssetMenu(fileName = "SkillSet", menuName = "Actions/ActionCollection")]
    public class ActionCollection : ScriptableObject
    {
        [SerializeField] private ActionInfoBaseSO[] _actions;
        public IEnumerable<IActionInfo> Actions => _actions;
    }
}