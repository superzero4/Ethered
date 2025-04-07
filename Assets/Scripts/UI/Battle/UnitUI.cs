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
        [SerializeReference] private Pool<ActionUI> _pool;

        public ActionUI[] ActionUIRead => _actionUIs.ToArray();

        public void Initialize()
        {
            _pool = new Pool<ActionUI>(_actionUIs);
        }

        //TODO implement grayScale by refactoring logic used for action, make it also valid for unit icons
        public void SetUnit(Unit unit, bool displayAction, bool greyPortrait)
        {
            var unitInfo = unit?.Info;
            (this as IVisualInformationUI).SetIcon(unitInfo);
            if (unitInfo == null || unitInfo.Actions == null || !displayAction)
            {
                _pool.Reset();
                return;
            }
            
            _pool.SetElements(unit.Info.Actions,
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