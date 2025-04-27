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
        [SerializeField] [ReadOnly] private IHints _timelineHints;

        public void Init(TimelineUI timelineUI, BattleSystem.Battle battle, IHints hints)
        {
            _timelineHints = hints;
            timelineUI.Initialize(OnHoverAction);
            battle.OnTimelineActionAdded.AddListener(timelineUI.OnTimelineMemberInserted);
        }

        public void OnHoverAction(ActionEventData arg0)
        {
            var action = arg0.action;
            if (action != null)
                _timelineHints.HintMultiple(action.TargetsEnumerable.Select(t => t.Position), action.Origin.Position);
            else
                _timelineHints.Reset();
        }
    }
}