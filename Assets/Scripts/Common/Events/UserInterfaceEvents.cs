using System;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine.Events;
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
        public EPhase phase;
    }

    [Serializable]
    public class TimelineEvent : UnityEvent<TimelineEventData>
    {
    }


    [Serializable]
    public struct TimelineEventData
    {
        private int? _insertIndex;
        private List<Action> _actions;

        public TimelineEventData(IEnumerable<Action> newActions, int? insertIndex = null)
        {
            this._insertIndex = insertIndex;
            //We do a copy of the references but we do not reference the list in case it's modified, we just want a copy
            this._actions = new List<Action>(newActions);
        }

        public int? InsertIndex => _insertIndex;
        public Action Action => _insertIndex.HasValue ? _actions[_insertIndex.Value] : null;
    }
}