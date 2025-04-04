using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using BattleSystem.TileSystem;
using NaughtyAttributes;
using UnitSystem.Actions.Bases;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnitSystem.Actions.BaseClass
{
    [CreateAssetMenu(fileName = "BasicAttack", menuName = "Actions/Attack/BasicAttack", order = 0)]
    public class BasicAbility : ActionInfoBaseSO
    {
        [SerializeField] EPhase _originPhase;
        [SerializeField] TargetDefinition[] _target;
        [SerializeField, Range(0, 10)] private int _nbTargets = 1;

        [SerializeField, Range(-20, 20), InfoBox("Negative means heal")]
        private int _damage = 1;

        [SerializeField, Tooltip("If it requires a direct line of sight on target")]
        private bool _requireLOS = true;


        public override EPhase OriginPhase => _originPhase;

        public override IEnumerable<TargetDefinition> PossibleTargets => _target;

        public override int NbTargets => _nbTargets;

        public override bool CanExecuteOnMap(Unit origin, TargetCollection targets, Tilemap map)
        {
            if (_requireLOS)
                return targets.Targets.All(t => map.HasLOS(origin.Position, t.Position));

            return true;
        }

        public override void Execute(Unit origin, TargetCollection targetCollection)
        {
            foreach (var target in targetCollection.Targets)
            {
                target.TakeDamage(_damage,origin);
            }
        }
    }
}