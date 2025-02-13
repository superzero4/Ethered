using System;
using Common;
using Common.Events;
using UnityEngine;

namespace UI.Battle
{
    public class TimelineUI : MonoBehaviour,IReset
    {
        [SerializeField] private TimelineMemberUI[] _members;

        private void Awake()
        {
            _members = GetComponentsInChildren<TimelineMemberUI>();
        }

        public void OnTimelineMemberInserted(TimelineEventData t)
        {
            var member = _members[t.index];
            member.gameObject.SetActive(true);
            member.SetAction(t.action);
        }

        public void Reset()
        {
            foreach (var member in _members)
            {
                member.gameObject.SetActive(false);
            }
        }
    }
}