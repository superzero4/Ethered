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
        [SerializeReference] private Pool<TimelineMemberUI> _memberPool;
        private UnityAction<ActionEventData> _onHover;

        public void Initialize(UnityAction<ActionEventData> onHover)
        {
            _memberPool = new Pool<TimelineMemberUI>(_memberPrefab, 10, transform);
            _onHover = onHover;
        }

        public void OnTimelineMemberInserted(TimelineEventData t)
        {
            if (t.InsertIndex.HasValue)
            {
                int index = t.InsertIndex.Value;
                var member = _memberPool.At(index);
                member.gameObject.SetActive(true);
                member.SetAction(t.Action, t.IsLast);
                member.ActionEvent.AddListener(_onHover);
                if (t.IsLast && index > 0)
                    _memberPool.Elements[index - 1].IsLast = false;
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