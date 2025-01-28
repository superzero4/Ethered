using System;
using UI.Battle;
using UnityEngine;

namespace BattleSystem
{
    public interface IBattleElement : IIcon, IHealth
    {
        public PositionData Position { get; }
        public ETeam Team { get; }
        public EPhase Phase => Position.Phase;
        public Vector2Int PositionVector => Position.Position;

        [Obsolete("Not very OOP, find another way")]
        public bool IsGround => Team == ETeam.None;
    }
}