using System.Linq;
using Common.Events.Combat;
using NaughtyAttributes;
using UI.Battle;
using UnityEngine;
using Views.Battle.Selection;

namespace Views.Battle
{
    public class TimelineView : MonoBehaviour
    {
        [SerializeField] private GameObject _timelineInfoHints;
        [SerializeField] [ReadOnly] private IHints _timelineHints;

        public void Init(TimelineUI timelineUI, BattleSystem.Battle battle)
        {
            _timelineHints = _timelineInfoHints.GetComponent<IHints>();
            timelineUI.Initialize(OnHoverAction);
            battle.OnTimelineActionAdded.AddListener(timelineUI.OnTimelineMemberInserted);
        }

        public void OnHoverAction(ActionEventData arg0)
        {
            var action = arg0.action;
            if (action != null)
                _timelineHints.HintMultiple(action.TargetsEnumerable.Select(t => t.Position)
                    .Append(action.Origin.Position));
            else
                _timelineHints.Reset();
        }
    }
}