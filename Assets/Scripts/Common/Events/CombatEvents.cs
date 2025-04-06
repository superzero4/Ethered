using System;
using System.Collections.Generic;
using BattleSystem;
using BattleSystem.TileSystem;
using UnitSystem;
using UnityEngine;
using UnityEngine.Events;

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