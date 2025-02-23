using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace UnitSystem.AI
{
    public interface IBrainCollection
    {
        public IBrain RandomBrain()
        {
            var arr = Enum.GetValues(typeof(EBrainType)).Cast<EBrainType>().ToArray();
            return GetBrain(arr[UnityEngine.Random.Range(0, arr.Length)]);
        }

        public IBrain GetBrain(EBrainType type);

        public IBrain this[EBrainType type]
        {
            get => GetBrain(type);
        }
    }
}