using UI.Battle;

namespace BattleSystem
{
    using UnityEngine;

    
    public interface IBattleElement : IIcon
    {
        public EPhase Phase { get; set; }
        public Vector2Int Position { get; set; }
        public ETeam Team { get; }
        
    }
}