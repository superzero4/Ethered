using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;

namespace UnitSystem
{
    [Serializable]
    public class TargetCollection
    {
        
        public TargetCollection(IBattleElement target) : this(new List<IBattleElement>() { target })
        {
        }

        public TargetCollection(List<IBattleElement> targetUnits)
        {
            _target = targetUnits;
        }
        public TargetCollection() : this(new List<IBattleElement>()) { }
        public void AddRange(IEnumerable<IBattleElement> targets)
        {
            _target.AddRange(targets);
        }
        private List<IBattleElement> _target;
        public IEnumerable<IBattleElement> Targets => _target;

        public IBattleElement this[int i]
        {
            get => _target[i];
        }
        public int Count => _target.Count;
    }
}