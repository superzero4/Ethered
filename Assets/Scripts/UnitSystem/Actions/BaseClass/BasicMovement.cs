using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using BattleSystem.TileSystem;
using Common.Visuals;
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
            return TryFindPath(origin, targets, map);
        }

        private bool TryFindPath(Unit origin, TargetCollection targets, Tilemap map)
        {
            Assert.IsTrue(targets.Count == 1);
            var target = targets.MainTarget;
            //Cached pathfinding
            var inReach = map.InReach(origin.Position.Position,
                TravelPhases(origin, target), _range);
            //If we are on multiple phases, we need to be able to land on all of them
            foreach (var tile in map[target.Position])
                if (!tile.Empty || !inReach.ContainsKey(tile.Base.Position))
                    return false;

            return true;
        }

        private static EPhase TravelPhases(Unit origin, IBattleElement target)
        {
            return origin.Position.Phase != target.Position.Phase ? EPhase.Both : origin.Position.Phase;
        }

        public override void Execute(Unit origin, TargetCollection targetCollection)
        {
            var cached = TilemapPathFindingExtensions.cache[(origin.Position.Position, TravelPhases(origin,targetCollection.MainTarget), _range)];
            origin.Move(cached[targetCollection.MainTarget.Position]);
            //targetCollection.MainTarget.Position);
        }

        public override IIcon.IconText AdditionalInfo => default;
    }
}