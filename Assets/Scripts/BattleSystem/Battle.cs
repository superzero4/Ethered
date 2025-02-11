using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleSystem.TileSystem;
using Common;
using Common.Events;
using UnitSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleSystem
{
    public class Battle
    {
        [SerializeField] private List<Unit> _units;
        [SerializeField] private Tilemap _battleElements;
        [SerializeField] private Timeline _timeline;
        public Tilemap Tiles => _battleElements;
        public IEnumerable<Unit> Units => _units;

        public void Init(BattleInfo info)
        {
            _timeline = new Timeline();
            _timeline.Initialize(new List<Action>());
            _battleElements = new Tilemap(new Vector2Int(info.Size.x, info.Size.y), 2, info.DefaultEnvironment);
            var specific = info.GetSpecificEnvironments();
            if (specific != null && specific.Any())
            {
                foreach (var env in specific)
                {
                    _battleElements.SetEnvironment(env);
                }
            }

            _units = new List<Unit>();
            for (int i = 0; i < info.Squad.Units.Count; i++)
            {
                var item = new Unit(info.Squad.Units[i], ETeam.Player, new Vector2Int(i, 0),
                    i == 2 ? EPhase.Both : (i % 2 == 0 ? EPhase.Normal : EPhase.Ethered));
                _units.Add(item);
                Assert.IsTrue(item.Position.Phase != EPhase.None);
                _battleElements.SetUnit(item);
            }

            for (int i = 0; i < info.Enemies.Units.Count; i++)
            {
                var item = new Unit(info.Enemies.Units[i], ETeam.Enemy, new Vector2Int(i, info.Size.y - 1),
                    i == 2 ? EPhase.Both : (i % 2 == 0 ? EPhase.Normal : EPhase.Ethered));
                _units.Add(item);
                _battleElements.SetUnit(item);
            }

            SubscribeToUnitsEvents();
        }

        private void SubscribeToUnitsEvents()
        {
            foreach (var unit in _units)
            {
                unit.OnUnitMoves?.AddListener(RefreshTileMap);
                unit.OnUnitHealthChange?.AddListener(RefreshHealth);
            }
        }

        private void RefreshTileMap(UnitMovementData arg0)
        {
            _battleElements.RemoveUnit(arg0.oldPosition);
            _battleElements.SetUnit(arg0.unit);
            //TODO link with the tilemap display
        }

        private void RefreshHealth(UnitHealthData arg0)
        {
            //TODO link with the health display
        }

        public bool ConfirmAction(Action action)
        {
            if (action.CanExecute(_battleElements))
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

        public IEnumerator TurnEndConfirmed(bool b)
        {
            yield return _timeline.Execute(true);
        }
    }
}