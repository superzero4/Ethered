using System;
using JetBrains.Annotations;
using UnitSystem;
using UnityEngine;
using UnityEngine.Events;
using Environment = BattleSystem.Environment;

namespace Common.Events.UserInteraction
{
    [Serializable]
    public class SelectionEvent : UnityEvent<SelectionEventData>
    {
    }

    [Serializable]
    public struct SelectionEventData
    {
        [CanBeNull] [SerializeReference] public Unit unit;
        [SerializeReference] public Environment environment;

        public SelectionEventData(Environment environment, Unit unit)
        {
            this.unit = unit;
            this.environment = environment;
        }
    }

    [Serializable]
    public class ResetEvent : UnityEvent<EmptyEvenData>
    {
        public void Invoke()
        {
            Invoke(default);
        }
    }
    [Serializable]
    public struct EmptyEvenData
    {
    }
}