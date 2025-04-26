using System;
using System.Collections.Generic;
using Common;
using Common.Events.Combat;
using Common.Events.UserInterface;
using Common.Visuals;
using UnitSystem;
using UnitSystem.Actions.Bases;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Action = BattleSystem.Action;
using ActionEvent = Common.Events.Combat.ActionEvent;

namespace UI.Battle
{
    public class TimelineMemberUI : MonoBehaviour, IVisualInformationUI, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private InfoUI _unitUI;
        [SerializeField] private InfoUI _actionUI;
        [SerializeField] private Image _next;
        [SerializeField] private InfoUI _targetUIPrefab;
        [SerializeField] private ActionEvent _actionEvent = new();
        private Pool<InfoUI> _targetUIPool;
        private Action _action;

        public void Awake()
        {
            _targetUIPool = new(_targetUIPrefab, 5, _targetUIPrefab.transform.parent);
        }

        public ActionEvent ActionEvent => _actionEvent;

        public bool IsLast
        {
            get { return !_next.isActiveAndEnabled; }
            set { _next.gameObject.SetActive(!value); }
        }

        public void SetInfo(VisualInformations? info, IEnumerable<IIcon.IconText> additionalInformations)
        {
            _actionUI.SetInfo(info, additionalInformations);
        }

        public void SetAction(Action a, bool isLast = false)
        {
            _action = a;
            _unitUI.SetInfo(a.Origin);
            var visualOverride = a.Info.VisualInformations;
            visualOverride.ClearDescription();
            _actionUI.SetInfo(visualOverride, a.Info.IconTexts);
            _targetUIPool.SetElements(a.TargetsEnumerable, (target, targetUI) => targetUI.SetInfo(target));
            _next.gameObject.SetActive(!isLast);
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