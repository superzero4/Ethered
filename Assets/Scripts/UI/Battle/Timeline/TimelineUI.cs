using System;
using Common;
using Common.Events;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace UI.Battle
{
    public class TimelineUI : MonoBehaviour, IReset
    {
        [SerializeField] private TimelineMemberUI _memberPrefab;
        private DynamicHideAndShow<TimelineMemberUI> _memberPool;

        public void Initialize()
        {
            _memberPool = new DynamicHideAndShow<TimelineMemberUI>(_memberPrefab, 10, transform);
        }

        public void OnTimelineMemberInserted(TimelineEventData t)
        {
            if (t.InsertIndex.HasValue)
            {
                var member = _memberPool.At(t.InsertIndex.Value);
                member.gameObject.SetActive(true);
                member.SetAction(t.Action);
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