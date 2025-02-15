using System;
using Common;
using Common.Events;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace UI.Battle
{
    public class TimelineUI : MonoBehaviour,IReset
    {
        [SerializeField] private TimelineMemberUI _memberPrefab;
        private DynamicHideAndShow<TimelineMemberUI> _memberPool;
        private void Awake()
        {
            _memberPool = new DynamicHideAndShow<TimelineMemberUI>(_memberPrefab,10,transform);
        }

        public void OnTimelineMemberInserted(TimelineEventData t)
        {
            var member = _memberPool.At(t.index);
            member.gameObject.SetActive(true);
            member.SetAction(t.action);
        }

        public void Reset()
        {
            _memberPool.Reset();
        }
    }
}