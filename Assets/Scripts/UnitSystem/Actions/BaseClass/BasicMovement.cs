using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

namespace UnitSystem.Actions.Bases
{
    [CreateAssetMenu(fileName = "BasicMovement", menuName = "Actions/Movement/BasicMovement", order = 0)]
    public class BasicMovement : ActionInfoBaseSO
    {
        [SerializeField] private EPhase _originPhase;
        [SerializeField, Range(0, 20)] private int _range = 1;
        [SerializeField] private ERelativePhase _targetPhase;
        public override EPhase OriginPhase => _originPhase;

        public override IEnumerable<TargetDefinition> Target
        {
            get { yield return new TargetDefinition(_targetPhase, 1, _range, TargetDefinition.TargetType.Ground); }
        }

        public override bool CanExecuteOnMap(Unit origin, TargetCollection targets, Battle.Tilemap map)
        {
            var target = targets.Target[0];
            //If we are on multiple phases, we need to be able to land on all of them
            foreach (var tile in map[target.Position])
            {
                if (!tile.Empty) return false;
            }

            Debug.LogWarning("Simple movement is implemented via teleportation for now, no pathfinding.");
            return true;
        }

        public override void Execute(Unit origin, TargetCollection targetCollection)
        {
            origin.Move(targetCollection.Target[0].Position);
        }
    }
}