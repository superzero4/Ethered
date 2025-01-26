using System;
using Common;
using UnityEngine;

namespace BattleSystem
{
    [Serializable]
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
        public PositionData(int phase,int x,int y) : this(new Vector2Int(x,y),(EPhase)phase)
        {
        }
        [SerializeField]
        public Vector2Int position;
        [SerializeField]
        public EPhase phase;
        public int x => position.x;
        public int y => position.y;

        public Vector2Int Position
        {
            get => position;
            set => position = value;
        }

        public EPhase Phase
        {
            get => phase;
            set => phase = value;
        }

        public override string ToString()
        {
            return $"({position.x},{position.y}:{Utils.PhaseToChar(phase)})";
        }
    }
}