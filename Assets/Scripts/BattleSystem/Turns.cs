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
        }

        public IEnumerator NextTurn(float delay = 0f, System.Action _onStep = null)
        {
            yield return _timeline.Execute(true, _battle.Tiles, delay, _onStep);
            yield return new WaitForSeconds(delay);
            yield return InitNewTurn(delay);
            _currentTurn++;
        }

        public IEnumerator InitNewTurn(float delay = .1f)
        {
            foreach (var action in _battle.EnemyActions())
            {
                yield return new WaitForSeconds(delay);
                _timeline.Prepend(action);
            }
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
            return unit != null && unit.HealthInfo.Alive && (unit.ActionsPerTurn == 1
                ? _timeline.Actors.All(a => a != unit)
                : _timeline.Actors.Count(a => a == unit) < unit.ActionsPerTurn);
        }

        public bool Step(Tilemap map, float newTurnDelay = .1f)
        {
            bool finished = _timeline.Step(map);
            if (finished)
            {
                foreach (var action in _battle.EnemyActions())
                {
                    _timeline.Prepend(action);
                }

                _currentTurn++;
                return true;
            }

            return false;
        }
    }
}