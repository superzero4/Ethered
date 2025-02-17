using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using BattleSystem.TileSystem;
using NUnit.Framework;
using UnityEngine;

namespace UnitSystem.Actions.Bases
{
    [CreateAssetMenu(fileName = "BasicMovement", menuName = "Actions/Movement/BasicMovement", order = 0)]
    public class BasicMovement : ActionInfoBaseSO
    {
        [SerializeField] private EPhase _originPhase;

        [SerializeField, UnityEngine.Range(0, 20)]
        private int _range = 1;

        [SerializeField] private ERelativePhase _targetPhase;
        public override EPhase OriginPhase => _originPhase;

        public override IEnumerable<TargetDefinition> PossibleTargets
        {
            get { yield return new TargetDefinition(_targetPhase, _range, TargetDefinition.TargetType.Ground); }
        }

        public override int NbTargets => 1;

        public override bool CanExecuteOnMap(Unit origin, TargetCollection targets, Tilemap map)
        {
            Assert.IsTrue(targets.Count == 1);
            var target = targets.MainTarget;
            //If we are on multiple phases, we need to be able to land on all of them
            var hash = new HashSet<Tile>(map[target.Position]);
            foreach (var tile in hash)
            {
                if (!tile.Empty) return false;
            }

            int count = 0;
            foreach (var tile in map.InReach(origin.Position.Position,
                         origin.Position.Phase != target.Position.Phase ? EPhase.Both : origin.Position.Phase, _range))
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
            origin.Move(targetCollection.MainTarget.Position);
        }
    }
}