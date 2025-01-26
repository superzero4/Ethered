using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleSystem;

namespace UnitSystem
{
    //Intermediate class to define a group of targets, used in the abstract methods, we use intermediate class in case we need to use another data structure or have middle logic
    public class TargetCollection
    {
        public TargetCollection(IBattleElement target) : this(new List<IBattleElement>() { target })
        {
        }

        public TargetCollection(List<IBattleElement> targetUnits)
        {
            Target = targetUnits;
        }

        //public IEnumerable<IBattleElement> TargetEnvironment => TargetTiles.Select(t=>(IBattleElement)t.Base);
        //public IEnumerable<Unit> TargetUnits => TargetTiles.Select(t=>t.Unit);
        public List<IBattleElement> Target { get; private set; }
        //public List<Battle.Tile> TargetTiles { get; private set; }
    }
}