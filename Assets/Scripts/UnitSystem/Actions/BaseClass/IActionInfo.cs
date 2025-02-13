using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using Common.Visuals;


namespace UnitSystem.Actions.Bases
{
    //[System.Serializable]
    public interface IActionInfo : IIcon
    {
        public EPhase OriginPhase { get; }
        public IEnumerable<TargetDefinition> Target { get; }

        public bool AreTargetsValid(Unit origin, IBattleElement[] target)
        {
            if (!CouldUnitExecute(origin))
                return false;
            return Target.Any(t =>
                t.IsValidTarget(origin, target));
        }

        public bool CouldUnitExecute(Unit origin)
        {
            return (origin.Position.Phase & OriginPhase) != 0b0;
        }

        /// <summary>
        /// This methods assume that the targets have already been verified and set, you don't need to check them in there
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="targets"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public bool CanExecuteOnMap(Unit origin, TargetCollection targets, BattleSystem.TileSystem.Tilemap map);

        public void Execute(Unit origin, TargetCollection targetCollection);

        public void Execute(Unit origin, IBattleElement target)
        {
            Execute(origin, new TargetCollection(target));
        }
    }
}