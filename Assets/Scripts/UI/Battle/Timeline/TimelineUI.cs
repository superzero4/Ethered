using System;
using Common;
using Common.Events;
using Common.Events.Combat;
using Common.Events.UserInterface;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace UI.Battle
{
    public class TimelineUI : MonoBehaviour, IReset
    {
        [SerializeField] private TimelineMemberUI _memberPrefab;
        private DynamicHideAndShow<TimelineMemberUI> _memberPool;
        private UnityAction<ActionEventData> _onHover;
        public void Initialize(UnityAction<ActionEventData> onHover)
        {
            _memberPool = new DynamicHideAndShow<TimelineMemberUI>(_memberPrefab, 10, transform);
            _onHover = onHover;
        }

        public void OnTimelineMemberInserted(TimelineEventData t)
        {
            if (t.InsertIndex.HasValue)
            {
                var member = _memberPool.At(t.InsertIndex.Value);
                member.gameObject.SetActive(true);
                member.SetAction(t.Action);
                member.ActionEvent.AddListener(_onHover);
            }
            else
            {
                _memberPool.Reset();
            }
        }

        public void Reset()
        {
            _memberPool.Reset();
        }
    }
}