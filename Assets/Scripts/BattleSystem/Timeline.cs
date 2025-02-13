using System;
using System.Collections;
using System.Collections.Generic;
using Common.Events;
using UnityEngine;

namespace BattleSystem
{
    public class Timeline
    {
        [SerializeField] private List<Action> _actions;
        [SerializeField] private TimelineEvent _actionInserted = new();

        public TimelineEvent ActionInserted => _actionInserted;

        public IEnumerator Execute(bool resetAfter, float delay = -1f)
        {
            foreach (var action in _actions)
            {
                action.Execute();
                yield return delay > 0 ? new WaitForSeconds(delay) : null;
            }

            if (resetAfter)
                _actions.Clear();
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
            _actionInserted.Invoke(new TimelineEventData()
            {
                action = action,
                index = index
            });
        }
    }
}