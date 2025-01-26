using UnityEngine;

namespace BattleSystem
{
    public struct PositionData
    {
        public PositionData(Vector2Int position, EPhase phase)
        {
            this.position = position;
            this.phase = phase;
        }

        public Vector2Int position;
        public EPhase phase;
        public int x => position.x;
        public int y => position.y;
    }
}