using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleSystem.TileSystem
{
    public static class TilemapPathFindingExtensions
    {
        public static IEnumerable<Tile> InRange(this Tilemap map, PositionIndexer position, EPhase phase, int range)
        {
            var pos = position.position;
            for (int r = -range; r <= range; r++)
            {
                foreach (var dir in new (int x, int y)[] { (1, 0), (0, 1) })
                {
                    var p = new PositionData(new PositionIndexer(pos.x + r * dir.x, pos.y + r * dir.y),
                        phase);
                    foreach (var tile in map[p])
                    {
                        Assert.IsTrue(tile.Base.Position.DistanceTo(p) <= range);
                        if (r != 0 || tile.Phase != phase) //i.e. if we're not on the same tile
                            yield return tile;
                    }
                }
            }
        }

        public static IEnumerable<(Tile, PathWrapper)> InReach(this Tilemap map, PositionIndexer position, EPhase phase,
            int range)
        {
            var pos = position.position;
            HashSet<Tile> visited = new HashSet<Tile>();
            Queue<(Tile, PathWrapper)> stack = new Queue<(Tile, PathWrapper)>();

            void Enqueue(Tile tile,PathWrapper predecessor)
            {
                var path = new PathWrapper(new List<PositionData>(predecessor.Path));
                path.Path.Add(tile.Base.Position);
                stack.Enqueue((tile, path));
            }

            foreach (var start in map[new PositionData(pos.x, pos.y, phase)])
            {
                Enqueue(start, new PathWrapper(new List<PositionData>()));
            }

            while (stack.Count > 0)
            {
                (Tile current, PathWrapper path) = stack.Dequeue();
                var depth = path.Path.Count;
                visited.Add(current);
                var allowed = current.Base.allowedMovement;
                //We consider a tile reachable if we can cross it and it's not the last or if we can stop on it
                if (allowed == EAllowedMovement.Nothing || depth > range)
                    continue;
                //We consider a tile reachable if we can cross it and it's not the last or if we can stop on it
                if (allowed == EAllowedMovement.Stop)
                {
                    yield return (current, path);
                }

                Assert.IsTrue((int)allowed >= (int)(EAllowedMovement.Cross));
                foreach (var close in map.InRange(current.Base.Position.Position, phase, 1))
                {
                    if (!visited.Contains(close))
                    {
                        Enqueue(close,path);
                    }
                }
            }
        }
    }
}