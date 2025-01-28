using BattleSystem;
using UnitSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Events
{
    public class ActionEvent : UnityEvent<ActionEventData>
    {
        
    }
    public class SelectionEvent : UnityEvent<SelectionEventData>
    {
        
    }
    public class UnitMovementEvent : UnityEvent<UnitMovementData>
    {
        
    }
    public class UnitHealthEvent : UnityEvent<UnitHealthData>
    {
        
    }
    public class UnitEvent : UnityEvent<UnitEventData>
    {
        
    }
    public struct ActionEventData
    {
        
    }

    public struct SelectionEventData
    {
        
    }
    public class UnitMovementData : UnitEventData
    {
        public PositionData oldPosition;
    }
    public class UnitHealthData : UnitEventData
    {
        public int oldHealth;
    }
    public class UnitEventData 
    {
        public Unit unit;
        public static implicit operator UnitEventData(Unit unit)
        {
            return new UnitEventData { unit = unit };
        }
    }
}