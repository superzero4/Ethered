using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleSystem.TileSystem;
using Common;
using Common.Events.UserInterface;
using UnitSystem;
using UnityEngine;

namespace BattleSystem
{
    [Serializable]
    public class Turns : IReset
    {
        private Timeline _timeline;
        private Battle _battle;
        private bool _hastarted = false;
        private int _currentTurn = 0;

        //TODO refactor double coupling by making the BrainCollection have a reference to the tilemap on creation and forwarding it to the brains, (because it's class) and therefore Turns could work withe the brains list directly without necistating the whole Battle/Tilemap
        public Turns(Battle battle)
        {
            Init(battle);
        }

        public TimelineEvent TimeLineUpdated => _timeline.TimeLineUpdated;


        private void Init(Battle battle)
        {
            _currentTurn = 0;
            _timeline = new Timeline();
            _timeline.Initialize(new List<Action>());
            _battle = battle;
            _hastarted = false;
            _timeline.TimeLineUpdated.AddListener(d =>
            {
                if (d.isReset)
                    _hastarted = false;
            });
        }

        public void AddAction(Action action)
        {
            _timeline.Prepend(action);
        }

        public void Reset()
        {
            Init(_battle);
        }

        public bool CanStillAct(Unit unit)
        {
            return _hastarted && unit != null && unit.HealthInfo.Alive && (unit.ActionsPerTurn == 1
                ? _timeline.Actors.All(a => a != unit)
                : _timeline.Actors.Count(a => a == unit) < unit.ActionsPerTurn);
        }

        public void NewTurn()
        {
            foreach (var action in _battle.EnemyActions())
                AddAction(action);

            _currentTurn++;
            _hastarted = true;
        }
        public void Step()
        {
            _timeline.Step(_battle.Tiles);
        }
    }
}