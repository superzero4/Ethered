using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleSystem.TileSystem
{
    public class Traversing
    {
        private Tilemap _tilemap;
        public Tilemap Tilemap => _tilemap;

        public Traversing(Tilemap tilemap)
        {
            _tilemap = tilemap;
        }

        public IEnumerable<Tile> InRange(PositionData.PositionIndexer position, EPhase phase, int range)
        {
            var pos = position.position;
            for (int r = -range; r <= range; r++)
            {
                if (range == 0)
                    continue;
                foreach (var dir in new (int x, int y)[] { (1, 0), (0, 1) })
                {
                    var p = new PositionData(new PositionData.PositionIndexer(pos.x + r * dir.x, pos.y + r * dir.y),
                        phase);
                    foreach (var tile in _tilemap[p])
                    {
                        Assert.IsTrue(tile.Base.Position.DistanceTo(p) <= range);
                        yield return tile;
                    }
                }
            }
        }

        public IEnumerable<Tile> InReach(PositionData.PositionIndexer position, EPhase phase, int range)
        {
            var pos = position.position;
            HashSet<Tile> visited = new HashSet<Tile>();
            Queue<(Tile, int)> stack = new Queue<(Tile, int)>();
            foreach (var start in _tilemap[new PositionData(pos.x, pos.y, phase)])
            {
                stack.Enqueue((start, 0));
            }

            while (stack.Count > 0)
            {
                (Tile current, int depth) = stack.Dequeue();
                visited.Add(current);
                {
                    foreach (var close in InRange(current.Base.Position.Position, phase, 1))
                    {
                        if (!visited.Contains(close))
                        {
                            var cross = close.Base.allowedMovement;
                            //We consider a tile reachable if we can cross it and it's not the last or if we can stop on it
                            var couple = (close, depth + 1);
                            if (cross == EAllowedMovement.Nothing)
                                continue;
                            if ((int)cross > (int)(EAllowedMovement.Cross))
                            {
                                stack.Enqueue(couple);
                                if (depth < range || cross == EAllowedMovement.Stop)
                                {
                                    yield return close;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}