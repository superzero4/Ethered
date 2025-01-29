using System;
using Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace BattleSystem
{
    [Serializable]
    public struct PositionData
    {
        /// <summary>
        /// Simple Wrapper class in case we'd like to use another indexer type
        /// </summary>
        [System.Serializable]
        public struct PositionIndexer
        {
            [SerializeField]
            public Vector2Int position;
            public int x => position.x;
            public int y => position.y;
            public PositionIndexer(int x, int y)
            {
                position = new Vector2Int(x, y);
            }

            public static implicit operator Vector2Int(PositionIndexer positionIndexer)
            {
                return positionIndexer.position;
            }

            public static implicit operator PositionIndexer(Vector2Int position)
            {
                return new PositionIndexer { position = position };
            }
        }

        public int DistanceTo(PositionData other)
        {
            return Mathf.Abs(x - other.x) + Mathf.Abs(y - other.y);
        }

        public PositionData(PositionIndexer position, EPhase phase)
        {
            this._position = position;
            this.phase = phase;
        }

        public PositionData(int x, int y, EPhase phase) : this(new PositionIndexer(x, y), phase)
        {
        }

        public PositionData(int phase, int x, int y) : this(x, y, (EPhase)phase)
        {
        }

        [SerializeField] private PositionIndexer _position;

        public Vector2Int Position
        {
            get => _position.position;
            set => _position.position = value;
        }

        [SerializeField] public EPhase phase;
        public int x => Position.x;
        public int y => Position.y;

        public EPhase Phase
        {
            get => phase;
            set => phase = value;
        }

        public override string ToString()
        {
            return $"({Position.x},{Position.y}:{Utils.PhaseToChar(phase)})";
        }
    }
}