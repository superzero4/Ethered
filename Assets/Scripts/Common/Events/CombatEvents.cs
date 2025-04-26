using System;
using System.Collections.Generic;
using BattleSystem;
using BattleSystem.TileSystem;
using UnitSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Action = BattleSystem.Action;

namespace Common.Events.Combat
{
    [Serializable]
    public class ActionEvent : UnityEvent<ActionEventData>
    {
    }

    [Serializable]
    public class UnitMovementEvent : UnityEvent<UnitMovementData>
    {
    }

    [Serializable]
    public class UnitHealthEvent : UnityEvent<UnitHitData>
    {
    }

    [Serializable]
    public class UnitAttackEvent : UnityEvent<UnitAttackData>
    {
    }

    public class UnitEvent : UnityEvent<UnitEventData>
    {
    }

    [Serializable]
    public struct ActionEventData
    {
        public Action action;
    }

    [Serializable]
    public class UnitCancelEvent : UnityEvent<UnitCancelEventData>
    {
    }

    [Serializable]
    public struct UnitCancelEventData
    {
        public UnitCancelEventData(bool isCancelTarget)
        {
            this.isCancelTarget = isCancelTarget;
        }

        public bool isCancelTarget;
    }

    [Serializable]
    public class UnitMovementData : UnitEventData
    {
        public PathWrapper path;
        public PositionData oldPosition;
    }

    [Serializable]
    public class UnitHitData : UnitEventData
    {
        public PositionIndexer direction;
        public int oldHealth;
    }

    [Serializable]
    public class UnitAttackData : UnitEventData
    {
        public PositionIndexer direction;
        public bool needLos;
        public int manhattandistance;
        public bool IsCloseQuarter => manhattandistance <= 1;
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

    /// <summary>
    /// ETeam.None => battle still running
    /// </summary>
    public class BattleEvent : UnityEvent<BattleEventData>
    {
    }

    public struct BattleEventData
    {
        public ETeam winner;
    }
}