using System;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Action = BattleSystem.Action;

namespace Common.Events.UserInterface
{
    [Serializable]
    public class PhaseEvent : UnityEvent<PhaseEventData>
    {
    }

    [Serializable]
    public struct PhaseEventData
    {
        public EPhase targetPhase;
        public float progress;

        public PhaseEventData(EPhase targetPhase)
        {
            this.targetPhase = targetPhase;
            progress = targetPhase == EPhase.Ethered ? 1 : 0;
        }
    }
    [Serializable]
    public class TimelineEvent : UnityEvent<TimelineEventData>
    {
    }


    [Serializable]
    public struct TimelineEventData
    {
        private int? _insertIndex;
        private bool _isRemove;
        private List<Action> _actions;

        public TimelineEventData(IEnumerable<Action> actions, int? insertIndex = null, bool isRemove = false)
        {
            this._insertIndex = insertIndex;
            //We do a copy of the references but we do not reference the list in case it's modified, we just want a copy
            this._actions = new List<Action>(actions);
            this._isRemove = isRemove;
        }

        public int? InsertIndex => _insertIndex;
        public Action Action => _insertIndex.HasValue ? _actions[_insertIndex.Value] : null;
        public bool IsLast => _insertIndex.HasValue && _insertIndex.Value == _actions.Count - 1;
        public int Count => _actions.Count;
        public bool IsRemove => _isRemove;
    }
}