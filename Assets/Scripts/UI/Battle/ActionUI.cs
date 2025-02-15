using Common;
using NaughtyAttributes;
using UnitSystem.Actions.Bases;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Battle
{
    public class ActionUI : ClickableUI<IActionInfo>, IReset
    {
        [SerializeField] private HighlightUI _highlight;
        [ReadOnly] private IActionInfo _action;
        private bool _isActive;

        protected override void Clicked(IActionInfo args)
        {
            if(_isActive)
                _highlight.Highlight();
        }

        protected override void AfterAwake()
        {
            base.AfterAwake();
        }

        protected override IActionInfo GetArgs()
        {
            return _action;
        }

        public void SetAction(IActionInfo action, bool isActive)
        {
            _action = action;
            SetInfo(action);
            _isActive = isActive;
            _highlight.CanHighlight = _isActive;
            if (!_isActive)
            {
                _image.sprite = action.VisualInformations.GrayScale;
            }
        }

        public void Reset()
        {
            _highlight.Reset();
        }
    }
}