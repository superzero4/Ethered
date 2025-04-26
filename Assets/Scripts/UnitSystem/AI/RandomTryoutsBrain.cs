using BattleSystem;
using BattleSystem.Actions;
using BattleSystem.TileSystem;

namespace UnitSystem.AI.Dev
{
    public class RandomTryoutsBrain : IBrain
    {
        private int _maxTryouts;
        public RandomTryoutsBrain(int maxTryouts)
        {
            _maxTryouts = maxTryouts;
        }
        public Action GetDecision(Unit source, Tilemap map)
        {
            return map.GetRandomValidAction(source, 0f,_maxTryouts);
        }
    }
}