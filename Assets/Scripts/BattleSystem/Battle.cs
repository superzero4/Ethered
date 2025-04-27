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
using Common.Visuals;
using SquadSystem;
using UnitSystem;
using UnitSystem.Actions.Bases;
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
        private BattleEvent _battleEnd;
        private Turns _turns;

        public Tilemap Tiles => _battleElements;
        public IEnumerable<Unit> Units => _allies.Concat(_ennemies);
        public TimelineEvent OnTimelineActionAdded => _turns.TimeLineUpdated;

        public BattleEvent BattleEnd => _battleEnd;

        public bool AlliesCanAct => _allies.Any(a => _turns.CanStillAct(a));

        public void Init(EncounterInfo info, MapInfo map, Squad squad, EnvironmentInfo defaultEnvironment,
            EnvironmentInfo defaultObstacle,
            IBrainCollection brains = null)
        {
            TilemapPathFindingExtensions.ClearCache();
            //Assert.IsNotNull(brains, "Brains were null, ensure that the caller has a reference to a brain collection so it can work correctly");
            _battleEnd = new BattleEvent();
            if (brains == null)
            {
                Debug.LogWarning("No actual brains set, falling back to a set of one default randombrain");
                brains = new OneBrainCollection(new RandomTryoutsBrain(1000));
            }

            _turns = new Turns(this);
            _brains = brains;
            _battleElements = new Tilemap(new Vector2Int(map.Size.x, map.Size.y), 2, defaultEnvironment);
            var specific = map.GetSpecificEnvironments();
            if (specific != null && specific.Any())
                foreach (var env in specific)
                    if (env.Position.Phase != EPhase.None)
                        _battleElements.SetEnvironment(
                            env.VisualInformations.IsDefault &&
                            env.allowedMovement == defaultObstacle.AllowedMovement
                                ? new Environment(defaultObstacle, env.Position)
                                : env);
            _allies = AddUnits(map.PlayerSpawns, squad, ETeam.Player);
            _ennemies = AddUnits(map.EnemySpawns, info.Units, ETeam.Enemy);
            SubscribeToUnitsEvents();
        }

        private List<Unit> AddUnits(PositionData[] spawns, Squad squad, ETeam team)
        {
            List<Unit> list = new List<Unit>();
            int min = Math.Min(squad.Units.Count, spawns.Length);
            if (min != squad.Units.Count)
            {
                Debug.LogWarning(
                    "The number of units exceeded the number of available spawn points, ensure the map has enough spawn points for the corresponding ecounter");
            }

            for (int i = 0; i < min; i++)
            {
                var pos = spawns[i];
                var item = new Unit(squad.Units[i], team, pos.Position,
                    pos.Phase);
                list.Add(item);
                _battleElements.SetUnit(item);
                Assert.IsTrue((int)item.Position.Phase >= 0 && (int)item.Position.Phase < (int)EPhase.Both,
                    " Enum values seems corrupted, probably due to unity automatically converting ticking everything and converting all bit to 1 for a negative value, avoid using everything in serialized fields");
            }

            return list;
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
            //foreach (var position in arg0.path.Path.Take(2))
            //{
            //    foreach (var t in _battleElements[position])
            //    {
            //        if (t.Unit != null && t.Unit == arg0.unit)
            //            _battleElements.RemoveUnit(position);
            //    }
            //}
            _battleElements.RemoveUnit(arg0.oldPosition);
            _battleElements.SetUnit(arg0.unit);
            //TODO see in basic movement how we should handle the changes of the map in beetween because
            //TilemapPathFindingExtensions.ClearCache();
        }

        public IEnumerable<Action> EnemyActions()
        {
            foreach (var ennemy in _ennemies)
            {
                if (!ennemy.HealthInfo.Alive)
                    continue;
                var action = _brains.RandomBrain().GetDecision(ennemy, _battleElements);
                if (action == null)
                    continue;
                //Assert.IsTrue(action!=null && action.HasTargets, "Action provided by brain doesn't have targets, fix Brain");
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

        public void Step()
        {
            if (_turns.Step(_battleElements))
                CheckForEnd();
        }

        public IEnumerator NextTurn(float delay, System.Action _onStep = null)
        {
            yield return _turns.NextTurn(delay, _onStep);
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
            //TilemapPathFindingExtensions.ClearCache();
            yield return _turns.InitNewTurn(delay);
        }

        public bool CanStillAct(Unit unit)
        {
            return _turns.CanStillAct(unit);
        }

        public IEnumerable<PositionData> PossibleTargetPosition(Unit origin, IActionInfo action, bool absolute)
        {
            foreach (var target in _battleElements.TilesFlat)
                if (TargetPreshot(action, origin, target.Unit, absolute) ||
                    TargetPreshot(action, origin, target.Base, absolute))
                    yield return target.Base.Position;
        }

        private bool TargetPreshot(IActionInfo action, Unit origin, IBattleElement target, bool checkPositionOnly)
        {
            return ((checkPositionOnly && action.IsTargetPositionValid(origin, target)) ||
                    action.AreTargetsValid(origin, target)) &&
                   action.CanExecuteOnMap(origin, new TargetCollection(target), _battleElements);
        }
    }
}