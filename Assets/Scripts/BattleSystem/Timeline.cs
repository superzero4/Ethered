using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Events;
using Common.Events.UserInterface;
using UnityEngine;

namespace BattleSystem
{
    [Serializable]
    public class Timeline
    {
        [SerializeField] private List<Action> _actions;
        [SerializeField] private TimelineEvent _timeLineUpdated = new();

        public TimelineEvent TimeLineUpdated => _timeLineUpdated;

        public IEnumerable<IBattleElement> Actors => _actions.Select(action => action.Origin);

        public IEnumerator Execute(bool resetAfter, float delay = -1f)
        {
            foreach (var action in _actions)
            {
                action.Execute();
                yield return delay > 0 ? new WaitForSeconds(delay) : null;
            }

            if (resetAfter)
                Reset();
        }

        private void Reset()
        {
            _actions.Clear();
            _timeLineUpdated.Invoke(new TimelineEventData(_actions, null));
        }

        public void Initialize(List<Action> actions)
        {
            _actions = actions;
        }

        public void Append(Action action)
        {
            Insert(_actions.Count, action);
        }

        [Obsolete(
            "This method relies on Actions being concretely implemented as IComparable, comparision will throw an Exception, to implement in case we decide to work with action order and priority",
            error: false)]
        public void PriorityInsert(Action action)
        {
            for (int i = 0; i < _actions.Count; i++)
            {
                if (action.CompareTo(_actions[i]) < 0)
                {
                    Insert(i, action);
                    return;
                }
            }

            Append(action);
        }

        public void Prepend(Action action)
        {
            Insert(0, action);
        }

        private void Insert(int index, Action action)
        {
            _actions.Insert(index, action);
            _timeLineUpdated.Invoke(new TimelineEventData(_actions, index));
        }
    }
}