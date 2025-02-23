using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleSystem.TileSystem;
using Common;
using Common.Events;
using UnitSystem;
using UnitSystem.AI;
using UnitSystem.AI.Dev;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleSystem
{
    [Serializable]
    public class Battle
    {
        [SerializeField] private List<Unit> _allies;
        [SerializeField] private List<Unit> _ennemies;
        [SerializeField] private Tilemap _battleElements;
        [SerializeReference] private IBrainCollection _brains;
        [SerializeReference] private Timeline _timeline;
        public Tilemap Tiles => _battleElements;
        public IEnumerable<Unit> Units => _allies.Concat(_ennemies);
        public TimelineEvent OnTimelineAction => _timeline.TimeLineUpdated;

        public void Init(BattleInfo info, IBrainCollection brains)
        {
            //Assert.IsNotNull(brains, "Brains were null, ensure that the caller has a reference to a brain collection so it can work correctly");
            if (brains == null)
            {
                Debug.LogWarning("No actual brains set, falling back to a set of one random brain");
                brains = new OneBrainCollection(new RandomTryoutsBrain(1000));
            }

            _brains = brains;
            _timeline = new Timeline();
            _timeline.Initialize(new List<Action>());
            _battleElements = new Tilemap(new Vector2Int(info.Size.x, info.Size.y), 2, info.DefaultEnvironment);
            var specific = info.GetSpecificEnvironments();
            if (specific != null && specific.Any())
                foreach (var env in specific)
                    _battleElements.SetEnvironment(env);

            _allies = new List<Unit>();
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
                var item = new Unit(info.Enemies.Units[i], ETeam.Enemy, new Vector2Int(i, info.Size.y - 1),
                    i == 2 ? EPhase.Both : (i % 2 == 0 ? EPhase.Normal : EPhase.Ethered));
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
                unit.OnUnitMoves?.AddListener(RefreshTileMap);
            }
        }

        private void RefreshTileMap(UnitMovementData arg0)
        {
            _battleElements.RemoveUnit(arg0.oldPosition);
            _battleElements.SetUnit(arg0.unit);
        }


        public bool ConfirmAction(Action action)
        {
            if (action != null && action.CanExecute(_battleElements))
            {
                _timeline.Append(action);
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

        public IEnumerator TurnEnd(bool b,float delay = .1f)
        {
            yield return _timeline.Execute(true, delay);
        }

        public IEnumerator NextTurn( float delay = .1f)
        {
            yield return TurnEnd(true, delay);
            if (delay > 0f)
                yield return new WaitForSeconds(delay);
            yield return InitNewTurn(delay);
        }

        public IEnumerator InitNewTurn(float delay = .1f)
        {
            foreach (var ennemy in _ennemies)
            {
                var action = _brains.RandomBrain().GetDecision(ennemy, _battleElements);
                Assert.IsTrue(action.CanExecute(_battleElements),
                    "Action provided by brain cannot execute on current map, fix Brain");
                Assert.IsTrue(action.HasTargets, "Action provided by brain doesn't have targets, fix Brain");
                _timeline.Append(action);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}