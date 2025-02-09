using System.Collections.Generic;
using UnitSystem;
using UnityEngine;
using UnityEngine.Assertions;
using Common.Visuals;

namespace UI.Battle
{
    public class UnitUI : MonoBehaviour, IVisualInformationUI
    {
        [SerializeField] private InfoUI _unitUI;
        [SerializeField] private List<ActionUI> _actions;

        //public void Init()
        //{
        //    foreach(var action in _actions)
        //        action.Init();
        //}

        public void SetUnit(UnitInfo unitInfo)
        {
            (this as IVisualInformationUI).SetIcon(unitInfo);
            if (unitInfo == null || unitInfo.Actions == null)
            {
                HideActionPanelsFrom(0);
                return;
            }
            Assert.IsTrue(unitInfo.Actions.Count <= _actions.Count,
                $"{_actions.Count} ui action panel references (and probably UI design itself) isn't enough to display all of the {unitInfo.Actions.Count} unit actions)");
            int i = 0;
            for (; i < unitInfo.Actions.Count; i++)
            {
                _actions[i].gameObject.SetActive(true);
                _actions[i].SetAction(unitInfo.Actions[i]);
            }
            HideActionPanelsFrom(i);
        }

        private void HideActionPanelsFrom(int i)
            {
                for (; i < _actions.Count; i++)
                    _actions[i].gameObject.SetActive(false);
            }

            public void SetInfo(VisualInformations info)
            {
                _unitUI.SetInfo(info);
            }
        }
    }