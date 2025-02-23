using BattleSystem;
using BattleSystem.Actions;
using BattleSystem.TileSystem;

namespace UnitSystem.AI.Dev
{
    public class OneBrainCollection : IBrainCollection
    {
        public OneBrainCollection(IBrain brain)
        {
            _brain = brain;
        }
        private IBrain _brain;
        public IBrain GetBrain(EBrainType type)
        {
            return _brain;
        }
    }

    public class RandomTryoutsBrain : IBrain
    {
        private int _maxTryouts;
        public RandomTryoutsBrain(int maxTryouts)
        {
            _maxTryouts = maxTryouts;
        }
        public Action GetDecision(Unit source, Tilemap map)
        {
            return map.GetRandomValidAction(source, _maxTryouts);
        }
    }
}