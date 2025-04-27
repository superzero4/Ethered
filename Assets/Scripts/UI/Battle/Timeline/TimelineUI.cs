using System;
using Common;
using Common.Events;
using Common.Events.Combat;
using Common.Events.UserInterface;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace UI.Battle
{
    public class TimelineUI : MonoBehaviour, IReset
    {
        [SerializeField] private TimelineMemberUI _memberPrefab;
        [SerializeField] private ScrollRect _scroll;
        [Header("Animation")] [SerializeField] private LeanTweenType _ease = LeanTweenType.easeInOutCubic;
        [SerializeField, Range(0, 1f)] private float _tweenDuration;
        [SerializeReference] private Pool<TimelineMemberUI> _memberPool;
        private UnityAction<ActionEventData> _onHover;

        public void Initialize(UnityAction<ActionEventData> onHover)
        {
            _memberPool = new Pool<TimelineMemberUI>(_memberPrefab, 10, _scroll.content);
            _onHover = onHover;
        }

        public void OnTimelineMemberInserted(TimelineEventData t)
        {
            if (t.InsertIndex.HasValue)
            {
                int index = t.InsertIndex.Value;
                if (t.IsRemove)
                {
                    _memberPool.Disable(index);
                }
                else
                {
                    var member = _memberPool.InsertNew(index);
                    member.gameObject.SetActive(true);
                    member.SetAction(t.Action, t.IsLast);
                    member.ActionEvent.AddListener(_onHover);
                    if (t.IsLast && index > 0)
                        _memberPool.Elements[index - 1].IsLast = false;
                    LeanTween.cancel(_scroll.gameObject);
                    LeanTween.value(_scroll.gameObject, _scroll.horizontalNormalizedPosition - 1f / t.Count,
                            (index + 1f) / t.Count,
                            _tweenDuration)
                        .setEase(_ease)
                        .setOnUpdate(value => { _scroll.horizontalNormalizedPosition = value; });
                }
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