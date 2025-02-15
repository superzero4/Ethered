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
        public IEnumerable<TargetDefinition> PossibleTargets { get; }
        public int NbTargets { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="targets">Can be one or multiple targets, will require all to be valid</param>
        /// <returns>True if all targets could execute on at least one (any) of the target definition</returns>
        public bool AreTargetsValid(Unit origin, params IBattleElement[] targets)
        {
            if (!CouldUnitExecute(origin))
                return false;
            if (targets.Length > NbTargets)
            {
                return false;
            }
            return targets.All(t => PossibleTargets.Any(targetDefinition => targetDefinition.AreValidTargets(origin, t)));
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
            Execute(origin, new TargetCollection(target,NbTargets));
        }
    }
}