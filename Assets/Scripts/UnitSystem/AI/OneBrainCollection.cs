using System.Collections;

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
    public class RandomBrainCollection : IBrainCollection
    {
        private IBrain[] _brains;
        public RandomBrainCollection(params  IBrain[] brains)
        {
            _brains = brains;
        }
        public IBrain GetBrain(EBrainType type)
        {
            return _brains[UnityEngine.Random.Range(0, _brains.Length)];
        }
    }
}