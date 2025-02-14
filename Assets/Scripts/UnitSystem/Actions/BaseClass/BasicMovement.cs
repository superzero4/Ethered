using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using BattleSystem.TileSystem;
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

        public override bool CanExecuteOnMap(Unit origin, TargetCollection targets, Tilemap map)
        {
            if (targets.Count != 1) return false;
            var target = targets[0];
            //If we are on multiple phases, we need to be able to land on all of them
            var hash = new HashSet<Tile>(map[target.Position]);
            foreach (var tile in hash)
            {
                if (!tile.Empty) return false;
            }

            int count = 0;
            foreach (var tile in map.InReach(origin.Position.Position, origin.Position.Phase, _range))
            {
                if (hash.Contains(tile))
                {
                    count++;
                    if (count == hash.Count) return true;
                }
            }

            return false;
        }

        public override void Execute(Unit origin, TargetCollection targetCollection)
        {
            origin.Move(targetCollection[0].Position);
        }
    }
}