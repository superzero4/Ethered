using BattleSystem;
using UnityEngine;

namespace Views.Battle
{
    public static class GridExtensions
    {
        public static Vector3 PhasedCellToWorld(this Grid grid, PositionIndexer dataPos)
        {
            var pos = grid.GetCellCenterWorld((Vector3Int)dataPos.position);
            pos.y -= grid.cellSize.y / 2;
            return pos;
        }

        public static Vector3 PhasedCellToWorld(this Grid grid, PositionData dataPos)
        {
            return grid.PhasedCellToWorld(dataPos.Position);
        }
    }
}