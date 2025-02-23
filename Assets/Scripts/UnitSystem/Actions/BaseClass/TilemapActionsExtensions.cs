using System.Collections.Generic;
using System.Linq;
using BattleSystem.TileSystem;
using UnitSystem;
using UnitSystem.Actions.Bases;
using UnityEngine;

namespace BattleSystem.Actions
{
    public static class TilemapActionsExtensions
    {
        public static Action GetRandomValidAction(this Tilemap tiles,Unit unit, int maxTries)
        {
            int i = 0;
            Action action = null;
            for (i = 0; i < maxTries; i++)
            {
                if (tiles.TryGetRandomValidAction(unit, out action))
                {
                    if (action.CanExecute(tiles))
                    {
                        return action;
                    }
                }
            }

            return null;
        }

        public static bool TryGetRandomValidAction(this Tilemap Tiles, Unit unit, out Action action)
        {
            var Size = Tiles.Size;
            IBattleElement target;
            bool targetTile = UnityEngine.Random.Range(0, 1f) >= .5f;
            if (targetTile)
            {
                var ph = Random.Range(1, 3);
                var posX = Random.Range(0, Size.x);
                var posY = Random.Range(0, Size.y);
                var pos = new PositionData(ph, posX, posY);
                var tile = Tiles[pos].ToList();
                target = tile[0].Base;
            }
            else
            {
                var units = Tiles.TilesFlat.Where(t => t.Unit != null).ToArray();
                target = units[UnityEngine.Random.Range(0, units.Length)].Unit;
            }

            var actions = unit.Info.Actions;
            var pickedAction = actions[Random.Range(0, actions.Count)];
            action = new Action(unit, pickedAction);
            var result = action.TrySetTarget(unit, target);
            return result;
        }
    }
}