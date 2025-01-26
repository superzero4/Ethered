using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using UI.Battle;
using UnityEngine;

namespace UnitSystem
{
    //[System.Serializable]
    public interface ActionInfo : IIcon
    {
        protected EPhase OriginPhase { get; };
        protected IEnumerable<TargetDefinition> Target { get; };
        
        public bool IsValidTarget(IBattleElement origin, IBattleElement[] target)
        {
            return Target.Any(t => t.IsValidTarget(origin, target));
        }
        public bool CanExecute(IBattleElement origin, TargetCollection targets, Battle.Tilemap map);
        public void Execute(IBattleElement origin, TargetCollection targetCollection);
        public void Execute(IBattleElement origin, IBattleElement target)
        {
            Execute(origin, new TargetCollection(target));
        }
    }
}