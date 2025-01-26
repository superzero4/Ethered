using UI.Battle;
using UnityEngine;

namespace BattleSystem
{
    public interface IBattleElement : IIcon
    {
        public PositionData Position { get; }
        public ETeam Team { get; }
        public EPhase Phase => Position.phase;
        public Vector2Int PositionVector => Position.position;
    }
}