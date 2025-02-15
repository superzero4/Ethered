using System.Collections.Generic;
using Common;
using UnitSystem;
using UnityEngine;
using UnityEngine.Assertions;
using Common.Visuals;
using UnitSystem.Actions.Bases;
using UnityEngine.Serialization;

namespace UI.Battle
{
    public class UnitUI : MonoBehaviour, IVisualInformationUI
    {
        [SerializeField] private List<ActionUI> _actionUIs;
        [SerializeField] private InfoUI _unitUI;
        private DynamicHideAndShow<ActionUI> _dynamicHideAndShow;

        public IEnumerable<ActionUI> ActionUIs => _actionUIs;

        public void Initialize()
        {
            _dynamicHideAndShow = new DynamicHideAndShow<ActionUI>(_actionUIs);
        }

        public void SetUnit(Unit unit)
        {
            var unitInfo = unit?.Info;
            (this as IVisualInformationUI).SetIcon(unitInfo);
            if (unitInfo == null || unitInfo.Actions == null)
            {
                _dynamicHideAndShow.Reset();
                return;
            }

            _dynamicHideAndShow.SetPanels(unit.Info.Actions,
                (action, actionUI) => { actionUI.SetAction(action, action.CouldUnitExecute(unit)); });
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