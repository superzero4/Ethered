using NaughtyAttributes;
using UnitSystem.Actions.Bases;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Battle
{
    public class ActionUI : HighlightUI<IActionInfo>
    {
        [ReadOnly] private IActionInfo _action;

        protected override void Clicked(IActionInfo args)
        {
            base.Clicked(args);
        }

        protected override void AfterAwake()
        {
            base.AfterAwake();
        }

        protected override IActionInfo GetArgs()
        {
            return _action;
        }

        public void SetAction(IActionInfo action)
        {
            _action = action;
            SetInfo(action);
        }
    }
}