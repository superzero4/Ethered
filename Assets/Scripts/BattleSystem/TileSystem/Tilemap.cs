using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Common;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine.Assertions;
using UnitSystem;
using UnityEngine;
using ReadOnly = NaughtyAttributes.ReadOnlyAttribute;
namespace BattleSystem.TileSystem
{
    [Serializable]
    public class Tilemap
    {
        [SerializeField] private Tile[][][] _tiles;
        [SerializeField] [ReadOnly] private Vector3Int _size;
        public Vector3Int Size => _size;
        public IEnumerable<Tile[][]> Tiles => _tiles;
        public IEnumerable<Tile> TilesFlat => _tiles.SelectMany(x => x.SelectMany(y => y));

        public Tilemap(Vector2Int sizeXY, int numberOfPhase, Tile defaultTile)
        {
            _size = new Vector3Int(sizeXY.x, sizeXY.y, numberOfPhase);
            _tiles = new Tile[_size.z][][];
            for (int i = 0; i < _tiles.Length; i++)
            {
                _tiles[i] = new Tile[_size.x][];
                for (int j = 0; j < _tiles[i].Length; j++)
                {
                    _tiles[i][j] = new Tile[_size.y];
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


        [CanBeNull]
        public Tile this[PositionIndexer p, int phaseIndex]
        {
            get
            {
                if (phaseIndex < 0 || phaseIndex >= _tiles.Length || p.x < 0 || p.x >= _tiles[phaseIndex].Length ||
                    p.y < 0 || p.y >= _tiles[phaseIndex][p.x].Length)
                    return null;
                return _tiles[phaseIndex][p.x][p.y];
            }
        }

        public IEnumerable<Tile> this[PositionData p]
        {
            get
            {
                foreach (var ph in Utils.FlagIndexes(p.Phase))
                {
                    var val = this[p.Position, ph];
                    if (val != null)
                        yield return val;
                }
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
                Assert.IsTrue(element.Position.Position == _tiles[phase][coord.x][coord.y].Base.Position.Position);
            }
        }


        public void SetEnvironment(Environment env)
        {
            foreach (var phase in Utils.FlagIndexes(env.Position.Phase))
            {
                var tile = this[env.Position.Position, phase];
                if (tile != null)
                {
                    tile.Base = env;
                }
            }
        }
    }
}