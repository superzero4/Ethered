using BattleSystem;
using BattleSystem.TileSystem;

namespace UnitSystem.AI
{
    public interface IBrain
    {
        public Action GetDecision(Unit source, Tilemap map);
    }
}