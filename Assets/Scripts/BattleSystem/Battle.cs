using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Common.Events;
using JetBrains.Annotations;
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

        public void Init(BattleInfo info)
        {
            _timeline = new Timeline();
            _battleElements = new Tilemap(new Vector3Int(2, info.Size.x, info.Size.y),
                new Tile(info.DefaultEnvironment, null));
            if (info.SpecificEnvironments != null && info.SpecificEnvironments.Count > 0)
                foreach (var env in info.SpecificEnvironments)
                    _battleElements.SetEnvironment(env);
            _units = new List<Unit>();
            for (int i = 0; i < info.Squad.Units.Count; i++)
            {
                var item = new Unit(info.Squad.Units[i], ETeam.Player, new Vector2Int(i, 0),
                    i==2 ? EPhase.Both : (i % 2 == 0 ? EPhase.Normal : EPhase.Ethered));
                _units.Add(item);
                _battleElements.SetUnit(item);
            }

            for (int i = 0; i < info.Enemies.Units.Count; i++)
            {
                var item = new Unit(info.Enemies.Units[i], ETeam.Enemy, new Vector2Int(i, info.Size.y - 1),
                    i==2 ? EPhase.Both : (i % 2 == 0 ? EPhase.Normal : EPhase.Ethered));
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
            }
        }

        private void RefreshTileMap(UnitEventData arg0)
        {
            _battleElements.RemoveUnit(arg0.oldPosition);
            _battleElements.SetUnit(arg0.unit);
        }

        public bool ConfirmAction(Action action)
        {
            if (action.CanExecute(_battleElements))
            {
                _timeline.PriorityInsert(action);
                return true;
            }

            return false;
        }

        [Serializable]
        public class Tile
        {
            public Tile(Tile other)
            {
                _base = other._base;
                _unit = other._unit;
            }
            public bool Empty => _unit == null;
            public Tile(Environment baseElement, Unit unit)
            {
                _base = baseElement;
                _unit = unit;
            }

            public EPhase Phase
            {
                get
                {
                    Assert.AreNotEqual(_base.Position.Phase, _unit.Position.Phase);
                    return _unit.Position.Phase;
                }
            }

            [SerializeField] private Environment _base;
            [SerializeField] [CanBeNull] private Unit _unit;

            public Environment Base
            {
                get => _base;
                set => _base = value;
            }

            public Unit Unit
            {
                get => _unit;
                set => _unit = value;
            }
        }

        [Serializable]
        public class Tilemap
        {
            [SerializeField] private Tile[][][] _tiles;
            public IEnumerable<Tile[][]> Tiles => _tiles;

            public IEnumerable<Tile> this[PositionData p]
            {
                get
                {
                    foreach(var phase in Utils.FlagIndexes(p.Phase))
                        yield return _tiles[phase][p.x][p.y];
                }
            }

            public void RemoveUnit(PositionData position)
            {
                //Remove unit from all phase it's in
                foreach (var phase in Utils.FlagIndexes(position.Phase))
                {
                    _tiles[phase][position.x][position.y].Unit = null;
                }
            }

            public void SetUnit(Unit element)
            {
                var coord = element.Position;
                //Set unit in all phase he's in
                foreach (var phase in Utils.FlagIndexes(coord.Phase))
                {
                    _tiles[phase][coord.x][coord.y].Unit = element;
                }
            }

            public Tilemap(Vector3Int size, Tile defaultTile)
            {
                _tiles = new Tile[size.x][][];
                for (int i = 0; i < _tiles.Length; i++)
                {
                    _tiles[i] = new Tile[size.y][];
                    for (int j = 0; j < _tiles[i].Length; j++)
                    {
                        _tiles[i][j] = new Tile[size.z];
                        for (int k = 0; k < _tiles[i][j].Length; k++)
                        {
                            var b = defaultTile.Base;
                            var u = defaultTile.Unit;
                            b.Position = new PositionData(new Vector2Int(j, k),
                                i == 0 ? EPhase.Normal : EPhase.Ethered);
                            if (u != null)
                                u.Move(b.Position);
                            _tiles[i][j][k] = new Tile(b, u);
                        }
                    }
                }
            }

            public void SetEnvironment(Environment env)
            {
                foreach (var phase in Utils.FlagIndexes(env.Position.Phase))
                {
                    _tiles[phase][env.Position.x][env.Position.y].Base = env;
                }
            }
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
                        sb.Append(Utils.BattleElementToString(b));
                        sb.Append(Utils.BattleElementToSimpleString(tile.Unit));
                        sb.Append(" ");
                    }

                    sb.Append("\n");
                }

                sb.Append("OtherPhase :\n");
            }

            return sb.ToString();
        }
    }
}