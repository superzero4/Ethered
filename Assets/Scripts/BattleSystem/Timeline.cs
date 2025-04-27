using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleSystem.TileSystem;
using Common.Events;
using Common.Events.Combat;
using Common.Events.UserInteraction;
using Common.Events.UserInterface;
using UnityEngine;
using UnityEngine.Serialization;

namespace BattleSystem
{
    [Serializable]
    public class Timeline
    {
        [SerializeField] private List<Action> _actions;
        [SerializeField] private TimelineEvent _timeLineUpdated = new();
        [SerializeField] private ResetEvent _timelineCleared = new();

        public TimelineEvent TimeLineUpdated => _timeLineUpdated;

        public IEnumerable<IBattleElement> Actors => _actions.Select(action => action.Origin);

        int index = 0;
        
        private void Reset()
        {
            TilemapPathFindingExtensions.ClearCache();
            index = 0;
            _actions.Clear();
            _timeLineUpdated.Invoke(new TimelineEventData(_actions, null));
        }

        public void Initialize(List<Action> actions)
        {
            _actions = actions;
        }

        public void Prepend(Action action)
        {
            Insert(0, action);
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

            Prepend(action);
        }

        public void Append(Action action)
        {
            Insert(_actions.Count, action);
        }

        private void Insert(int index, Action action)
        {
            _actions.Insert(index, action);
            _timeLineUpdated.Invoke(new TimelineEventData(_actions, index));
        }

        public void Step(Tilemap map)
        {
            if (index >= _actions.Count)
            {
                Reset();
                return;
            }

            var action = _actions[index];
            if (action.CanExecute(map))
                action.Execute();
            else
            {
                action.Origin.CancelAction(true);
                foreach (var target in action.TargetsEnumerable)
                    target.CancelAction(false);
            }

            _timeLineUpdated.Invoke(new TimelineEventData(_actions, index, true));
            index++;
        }
    }
}