using BattleSystem;
using Common.Events.Combat;
using Common.Events.UserInterface;
using Common.Visuals;
using UnitSystem;
using UnitSystem.Actions.Bases;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Battle
{
    public class TimelineMemberUI : MonoBehaviour, IVisualInformationUI, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private InfoUI _unitUI;
        [SerializeField] private InfoUI _actionUI;
        [SerializeField] private InfoUI[] _targetUI;
        [SerializeField] private UnityEngine.UI.Selectable _selectable;
        private Action _action;
        [SerializeField] private ActionEvent _actionEvent = new();

        public ActionEvent ActionEvent => _actionEvent;

        public void SetInfo(VisualInformations info, params IIcon.IconText[] additionalInformations)
        {
            _actionUI.SetInfo(info, additionalInformations);
        }

        public void SetAction(Action a)
        {
            _action = a;
            _unitUI.SetInfo(a.Origin);
            _actionUI.SetInfo(a.Info);
            int i = 0;
            //TODO refactor this in dedicated DynamicDisplay component with pooling, to also be used with the variable number of actions
            foreach (var target in a.TargetsEnumerable)
            {
                _targetUI[i].gameObject.SetActive(true);
                _targetUI[i].SetInfo(target);
                i++;
            }

            for (; i < _targetUI.Length; i++)
                _targetUI[i].gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_action == null)
                return;
            _actionEvent.Invoke(new ActionEventData
            {
                action = _action
            });
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _actionEvent.Invoke(new ActionEventData
            {
                action = null
            });
        }
    }
}