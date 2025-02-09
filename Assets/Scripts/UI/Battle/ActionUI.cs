using NaughtyAttributes;
using UnitSystem.Actions.Bases;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Battle
{
    public class ActionUI : ClickableUI
    {
        [SerializeReference] [ReadOnly] private IActionInfo _action;
        protected override void Awake()
        {
            base.Awake();
        }

        public void SetAction(IActionInfo action)
        {
            _action = action;
            SetInfo(action);
        }
        
    }
}