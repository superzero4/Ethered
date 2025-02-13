using System.Collections.Generic;
using UnitSystem;
using UnityEngine;
using UnityEngine.Assertions;
using Common.Visuals;
using UnityEngine.Serialization;

namespace UI.Battle
{
    public class UnitUI : MonoBehaviour, IVisualInformationUI
    {
        [SerializeField] private InfoUI _unitUI;
        [FormerlySerializedAs("_actions")] [SerializeField] private List<ActionUI> _actionUIs;

        public IEnumerable<ActionUI> ActionUIs => _actionUIs;
        //public void Init()
        //{
        //    foreach(var action in _actions)
        //        action.Init();
        //}

        public void SetUnit(Unit unit)
        {
            var unitInfo = unit?.Info;
            (this as IVisualInformationUI).SetIcon(unitInfo);
            if (unitInfo == null || unitInfo.Actions == null)
            {
                HideActionPanelsFrom(0);
                return;
            }


            Assert.IsTrue(unitInfo.Actions.Count <= _actionUIs.Count,
                $"{_actionUIs.Count} ui action panel references (and probably UI design itself) isn't enough to display all of the {unitInfo.Actions.Count} unit actions)");

            int i = 0;
            for (;
                 i < unitInfo.Actions.Count;
                 i++)
            {
                var actionUI = _actionUIs[i];
                actionUI.gameObject.SetActive(true);
                var action = unitInfo.Actions[i];
                actionUI.SetAction(action, action.CouldUnitExecute(unit));
            }

            HideActionPanelsFrom(i);
        }

        private void HideActionPanelsFrom(int i)
        {
            for (; i < _actionUIs.Count; i++)
                _actionUIs[i].gameObject.SetActive(false);
        }

        public void SetInfo(VisualInformations info)
        {
            _unitUI.SetInfo(info);
        }
        public void ResetActionUIs(ActionUI except = null)
        {
            foreach (var actionUI in _actionUIs)
            {
                if (actionUI == except)
                    continue;
                actionUI.Reset();
            }
        }
    }
}