using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;

namespace UnitSystem
{
    [Serializable]
    public class TargetCollection
    {
        [Obsolete]
        int maxSize;
        public IBattleElement MainTarget => _target.Peek();
        public TargetCollection(IBattleElement target, int maxSize) : this(new List<IBattleElement>() { target },
            maxSize)
        {
        }

        public TargetCollection(IEnumerable<IBattleElement> targetUnits, int maxSize)
        {
            this.maxSize = maxSize;
            _target = new Queue<IBattleElement>();
            AddRange(targetUnits);
        }

        public TargetCollection(int maxSize) : this(new List<IBattleElement>(), maxSize)
        {
        }

        public void AddRange(IEnumerable<IBattleElement> targets)
        {
            foreach(var target in targets)
                Add(target);
        }

        private Queue<IBattleElement> _target;
        public IEnumerable<IBattleElement> Targets => _target;

        //public IBattleElement this[int i]
        //{
        //    get => _target[i];
        //}
        public int Count => _target.Count;
        //We use a maximud sized queue to store the targets in case we input some more but we currently only do safe calls to this because the input system should prevent that or update based on the result of this Add (which is not the case currently)
        public void Add(IBattleElement battleElement)
        {
            _target.Enqueue(battleElement);
            while (_target.Count > maxSize)
                _target.Dequeue();
        }
    }
}