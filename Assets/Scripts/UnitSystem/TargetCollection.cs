using System.Collections.Generic;
using BattleSystem;

namespace UnitSystem
{
    //Intermediate class to define a group of targets, used in the abstract methods, we use intermediate class in case we need to use another data structure or have middle logic
    public class TargetCollection
    {
        public TargetCollection(IBattleElement target) : this(new List<IBattleElement>() { target })
        {
        }

        public TargetCollection(List<IBattleElement> target)
        {
            Target = target;
        }

        public List<IBattleElement> Target { get; private set; }
    }
}