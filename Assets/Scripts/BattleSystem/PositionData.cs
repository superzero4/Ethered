using UnityEngine;

namespace BattleSystem
{
    public struct PositionData
    {
        public int DistanceTo(PositionData other)
        {
            return Mathf.Abs(x - other.x) + Mathf.Abs(y - other.y);
        }
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