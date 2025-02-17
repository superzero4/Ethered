using System;
using System.Collections.Generic;
using BattleSystem;
using BattleSystem.TileSystem;
using JetBrains.Annotations;
using UnitSystem;
using UnityEngine;
using UnityEngine.Events;
using Action = BattleSystem.Action;
using Environment = BattleSystem.Environment;

namespace Common.Events
{
    [Serializable]
    public class ActionEvent : UnityEvent<ActionEventData>
    {
    }

    [Serializable]
    public class SelectionEvent : UnityEvent<SelectionEventData>
    {
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
    public class UnitMovementEvent : UnityEvent<UnitMovementData>
    {
    }

    [Serializable]
    public class UnitHealthEvent : UnityEvent<UnitHealthData>
    {
    }

    public class UnitEvent : UnityEvent<UnitEventData>
    {
    }

    [Serializable]
    public class PhaseEvent : UnityEvent<PhaseEventData>
    {
    }
    [Serializable] public class TimelineEvent : UnityEvent<TimelineEventData> { }

    [Serializable]
    public struct PhaseEventData
    {
        public EPhase phase;
    }

    [Serializable]
    public struct ActionEventData
    {
    }

    [Serializable]
    public struct SelectionEventData
    {
        [CanBeNull] [SerializeReference] public Unit unit;
        [SerializeReference] public  Environment environment;

        public SelectionEventData(Environment environment, Unit unit)
        {
            this.unit = unit;
            this.environment = environment;
        }
    }

    [Serializable]
    public class UnitMovementData : UnitEventData
    {
        public PositionData oldPosition;
    }

    [Serializable]
    public class UnitHealthData : UnitEventData
    {
        public int oldHealth;
    }

    [Serializable]
    public class UnitEventData
    {
        public Unit unit;

        public static implicit operator UnitEventData(Unit unit)
        {
            return new UnitEventData { unit = unit };
        }
    }

    [Serializable]
    public struct TimelineEventData
    {
        private int? _insertIndex;
        private List<Action> _actions;

        public TimelineEventData(IEnumerable<Action> newActions, int? insertIndex=null)
        {
            this._insertIndex = insertIndex;
            //We do a copy of the references but we do not reference the list in case it's modified, we just want a copy
            this._actions = new List<Action>(newActions);
        }

        public int? InsertIndex => _insertIndex;
        public Action Action => _insertIndex.HasValue ? _actions[_insertIndex.Value] : null;
    }
    [Serializable]
    public struct EmptyEvenData
    {
    }
}