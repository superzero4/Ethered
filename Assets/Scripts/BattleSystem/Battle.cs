using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleSystem.TileSystem;
using Common;
using Common.Events;
using Common.Events.Combat;
using Common.Events.UserInterface;
using UnitSystem;
using UnitSystem.AI;
using UnitSystem.AI.Dev;
using UnityEngine;
using UnityEngine.Assertions;

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

        public IEnumerator TurnEnd(bool b, float delay = .1f)
        {
            yield return _timeline.Execute(true, delay);
        }

        public IEnumerator NextTurn(float delay = .1f)
        {
            yield return TurnEnd(true, delay);
            if (delay > 0f)
                yield return new WaitForSeconds(delay);
            yield return InitNewTurn(delay);
            _currentTurn++;
        }

        public IEnumerator InitNewTurn(float delay = .1f)
        {
            foreach (var action in _battle.EnemyActions())
            {
                _timeline.Append(action);
                yield return new WaitForSeconds(delay);
            }
        }

        public void AddAction(Action action)
        {
            _timeline.Append(action);
        }

        public void Reset()
        {
            Init(_battle);
        }

        public bool CanStillAct(Unit unit)
        {
            return unit != null && (unit.ActionsPerTurn == 1
                ? _timeline.Actors.All(a => a != unit)
                : _timeline.Actors.Count(a => a == unit) < unit.ActionsPerTurn);
        }
    }

    [Serializable]
    public class Battle
    {
        [SerializeField] private List<Unit> _allies;
        [SerializeField] private List<Unit> _ennemies;
        [SerializeField] private Tilemap _battleElements;
        [SerializeReference] private IBrainCollection _brains;
        private BattleEvent _battleEnd;
        private Turns _turns;

        public Tilemap Tiles => _battleElements;
        public IEnumerable<Unit> Units => _allies.Concat(_ennemies);
        public TimelineEvent OnTimelineAction => _turns.TimeLineUpdated;

        public BattleEvent BattleEnd => _battleEnd;


        public void Init(BattleInfo info, IBrainCollection brains)
        {
            //Assert.IsNotNull(brains, "Brains were null, ensure that the caller has a reference to a brain collection so it can work correctly");
            _battleEnd = new BattleEvent();
            if (brains == null)
            {
                Debug.LogWarning("No actual brains set, falling back to a set of one random brain");
                brains = new OneBrainCollection(new RandomTryoutsBrain(1000));
            }

            _turns = new Turns(this);
            _brains = brains;
            _battleElements = new Tilemap(new Vector2Int(info.Size.x, info.Size.y), 2, info.DefaultEnvironment);
            var specific = info.GetSpecificEnvironments();
            if (specific != null && specific.Any())
                foreach (var env in specific)
                    _battleElements.SetEnvironment(env);
            _allies = new List<Unit>();
            var mid = info.Size.x / 2;
            for (int i = 0; i < info.Squad.Units.Count; i++)
            {
                var item = new Unit(info.Squad.Units[i], ETeam.Player, new Vector2Int(i, 0),
                    i == 2 ? EPhase.Both : (i % 2 == 0 ? EPhase.Normal : EPhase.Ethered));
                Assert.IsTrue(item.Position.Phase != EPhase.None);
                _allies.Add(item);
                _battleElements.SetUnit(item);
            }

            _ennemies = new List<Unit>();
            for (int i = 0; i < info.Enemies.Units.Count; i++)
            {
                var enemy = info.Enemies.Units[i];
                var pos = new Vector2Int(mid + (i % 2 == 0 ? 1 : -1) * ((i + 1) / 2), info.Size.y - 1);
                var item = new Unit(enemy, ETeam.Enemy, pos,
                    pos.x == mid ? EPhase.Both : (pos.x % 2 == 0 ? EPhase.Normal : EPhase.Ethered));
                Assert.IsTrue(item.Position.Phase != EPhase.None);
                _ennemies.Add(item);
                _battleElements.SetUnit(item);
            }

            SubscribeToUnitsEvents();
        }

        private void SubscribeToUnitsEvents()
        {
            foreach (var unit in Units)
            {
                unit.OnUnitMoves?.AddListener(UnitMoved);
            }
        }

        private void UnitMoved(UnitMovementData arg0)
        {
            _battleElements.RemoveUnit(arg0.path.Path[0]);
            _battleElements.SetUnit(arg0.unit);
        }

        public IEnumerable<Action> EnemyActions()
        {
            foreach (var ennemy in _ennemies)
            {
                var action = _brains.RandomBrain().GetDecision(ennemy, _battleElements);
                Assert.IsTrue(action.HasTargets, "Action provided by brain doesn't have targets, fix Brain");
                yield return action;
            }
        }

        public bool ConfirmAction(Action action)
        {
            if (action != null && action.CanExecute(_battleElements))
            {
                _turns.AddAction(action);
                //_timeline.PriorityInsert(action);
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Battle:\n");
            foreach (var phase in _battleElements.Tiles)
            {
                foreach (var row in phase)
                {
                    foreach (var tile in row)
                    {
                        var b = tile.Base;
                        sb.Append(b.Position);
                        sb.Append(Utils.WalkTypeToChar(b.allowedMovement));
                        //sb.Append(Utils.BattleElementToString(b));
                        sb.Append(Utils.BattleElementToSimpleString(tile.Unit, true));
                        sb.Append(" ");
                    }

                    sb.Append("\n");
                }

                sb.Append("OtherPhase :\n");
            }

            return sb.ToString();
        }


        public IEnumerator NextTurn(float delay)
        {
            yield return _turns.NextTurn(delay);
            CheckForEnd();
        }

        private void CheckForEnd()
        {
            var allies = _allies.Any(u => u.HealthInfo.Alive);
            var enemies = _ennemies.Any(u => u.HealthInfo.Alive);
            ETeam winner = ETeam.None;
            switch ((allies, enemies))
            {
                case (true, true):
                    return;
                case (false, true):
                    winner = ETeam.Enemy;
                    break;
                case (true, false):
                    winner = ETeam.Player;
                    break;
                case (false, false):
                    winner = ETeam.None;
                    break;
            }

            _battleEnd?.Invoke(new BattleEventData() { winner = winner });
        }

        public IEnumerator InitNewTurn(float delay)
        {
            yield return _turns.InitNewTurn(delay);
        }

        public bool CanStillAct(Unit unit)
        {
            return _turns.CanStillAct(unit);
        }
    }
}