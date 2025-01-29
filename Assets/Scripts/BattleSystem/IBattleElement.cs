using System;
using UI.Battle;
using UnityEngine;

namespace BattleSystem
{
    public interface IBattleElement : IIcon, IHealth
    {
        public PositionData Position { get; }
        public ETeam Team { get; }

        //public EAllowedMovement allowedMovement { get; }
        //public void CopyFrom(IBattleElement element)
        //{
        //    Position = element.Position;
        //    Team = element.Team;
        //    VisualInformations = element.VisualInformations;
        //}
        public EPhase Phase => Position.Phase;
        public Vector2Int PositionVector => Position.Position;

        [Obsolete("Not very OOP, find another way")]
        public bool IsGround => Team == ETeam.None;
    }
}