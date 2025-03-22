using System.Collections.Generic;

namespace BattleSystem.TileSystem
{
    public struct PathWrapper
    {
        private List<PositionData> _path;
        public PathWrapper(List<PositionData> path)
        {
            _path = path;
        }

        public List<PositionData> Path => _path;
    }
}