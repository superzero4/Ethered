using System;
using System.Collections.Generic;
using BattleSystem;
using Common;
using UnitSystem;
using UnityEngine;

namespace BattleSystem.TileSystem
{
    [Serializable]
    public class Tilemap
    {
        [SerializeField] private Tile[][][] _tiles;
        public IEnumerable<Tile[][]> Tiles => _tiles;

        public IEnumerable<Tile> this[PositionData p]
        {
            get
            {
                foreach (var phase in Utils.FlagIndexes(p.Phase))
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
}