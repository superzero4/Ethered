using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleSystem.TileSystem
{
    public static class TilemapPathFindingExtensions
    {
        public static IEnumerable<Tile> InRange(this Tilemap map,PositionIndexer position, EPhase phase, int range)
        {
            var pos = position.position;
            for (int r = -range; r <= range; r++)
            {
                if (range == 0)
                    continue;
                foreach (var dir in new (int x, int y)[] { (1, 0), (0, 1) })
                {
                    var p = new PositionData(new PositionIndexer(pos.x + r * dir.x, pos.y + r * dir.y),
                        phase);
                    foreach (var tile in map[p])
                    {
                        Assert.IsTrue(tile.Base.Position.DistanceTo(p) <= range);
                        yield return tile;
                    }
                }
            }
        }

        public static IEnumerable<Tile> InReach(this Tilemap map,PositionIndexer position, EPhase phase, int range)
        {
            var pos = position.position;
            HashSet<Tile> visited = new HashSet<Tile>();
            Queue<(Tile, int)> stack = new Queue<(Tile, int)>();
            foreach (var start in map[new PositionData(pos.x, pos.y, phase)])
            {
                stack.Enqueue((start, 0));
            }

            while (stack.Count > 0)
            {
                (Tile current, int depth) = stack.Dequeue();
                visited.Add(current);
                {
                    foreach (var close in map.InRange(current.Base.Position.Position, phase, 1))
                    {
                        if (!visited.Contains(close))
                        {
                            var allowed = close.Base.allowedMovement;
                            //We consider a tile reachable if we can cross it and it's not the last or if we can stop on it
                            if (allowed == EAllowedMovement.Nothing)
                                continue;
                            var couple = (close, depth + 1);
                            if ((int)allowed >= (int)(EAllowedMovement.Cross))
                            {
                                stack.Enqueue(couple);
                                if (allowed == EAllowedMovement.Stop)
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