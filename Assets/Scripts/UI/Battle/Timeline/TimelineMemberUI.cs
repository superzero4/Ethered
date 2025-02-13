using BattleSystem;
using Common.Visuals;
using UnitSystem;
using UnitSystem.Actions.Bases;
using UnityEngine;

namespace UI.Battle
{
    public class TimelineMemberUI : MonoBehaviour, IVisualInformationUI
    {
        [SerializeField] private InfoUI _unitUI;
        [SerializeField] private InfoUI _actionUI;
        [SerializeField] private InfoUI[] _targetUI;

        public void SetInfo(VisualInformations info)
        {
            _actionUI.SetInfo(info);
        }

        public void SetAction(Action a)
        {
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
            for(;i<_targetUI.Length;i++)
                _targetUI[i].gameObject.SetActive(false);
        }
        public void SetAction(Unit origin, IActionInfo action, IBattleElement target)
        {
        }
    }
}