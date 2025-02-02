using System;
using System.Collections.Generic;
using Common;
using NUnit.Framework;
using UnityEngine;

namespace BattleSystem.TileSystem
{
    public static class TilemapLineOfSightExtensions
    {
        public static bool HasLOS(this Tilemap map, PositionData origin, PositionData target)
        {
            foreach (var phOr in Utils.FlagIndexes(origin.Phase))
            {
                foreach (var phTar in Utils.FlagIndexes(target.Phase))
                {
                    if (phOr == phTar)
                    {
                        if (HasLOS(map, origin.Position, target.Position, phTar))
                            return true;
                    }
                    else
                    {
                        Debug.LogWarning(
                            "TODO cross phase LOS isn't defined yet in terms of game design nor implementation");
                    }
                }
            }

            return false;
        }

        public static IEnumerable<PositionIndexer> StraightLine(PositionIndexer origin, PositionIndexer target)
        {
            if (origin.x == target.x && origin.y == target.y)
                yield break;
            int endX;
            var deltaY = target.y - origin.y;
            var deltaX = target.x - origin.x;
            //We use a y = mx + b equation to find the next tile but we use the an <1 slope to avoid floating point errors, it requires we increment on the x axis insteadn, we use flip to keep track of that
            bool flip = Math.Abs(deltaY) > Math.Abs(deltaX);
            if (flip)
            {
                //Just a fancy permutation
                (origin.x, origin.y) = (origin.y, origin.x);
                endX = target.y;
                (deltaX, deltaY) = (deltaY, deltaX);
            }
            else
                endX = target.x;
            //Straight line case
            if (deltaX == 0)
            {
                var sign = Math.Sign(deltaY);
                for (int y = origin.y+sign; y != target.y-sign; y += sign)
                {
                    yield return Flip(origin.x, y);
                }
                yield break;
            }
            var coef = Math.Abs((1f * deltaY) * (1f / deltaX));
            int dir = (int)Mathf.Sign(deltaX);
            for (int step = dir; step != deltaX; step += dir)
            {
                int x = origin.x + step;
                var y = origin.y + coef * step;
                //If we intersect exactly in the middle of a tile (4 digit precision on the .5 evaluation), it means we hit both tiles
                int decimal3Digit = Mathf.Abs((int)(y * 1000) % 1000);
                if (decimal3Digit == 500)
                {
                    yield return Flip(x, Mathf.FloorToInt(y));
                    yield return Flip(x, Mathf.CeilToInt(y));
                }
                else
                {
                    yield return Flip(x, Mathf.RoundToInt(y));
                }
            }

            PositionIndexer Flip(int v1, int V2)
            {
                PositionIndexer p = flip ? new PositionIndexer(V2, v1) : new PositionIndexer(v1, V2);
                return p;
            }
        }

        public static bool HasLOS(this Tilemap map, PositionIndexer origin, PositionIndexer target, int phaseIndex)
        {
            foreach (var pos in StraightLine(origin, target))
            {
                if (map[pos, phaseIndex] == null ||
                    map[pos, phaseIndex].Base.allowedMovement == EAllowedMovement.Nothing)
                    return false;
            }

            return true;
        }
    }
}