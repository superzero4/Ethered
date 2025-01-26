using UnitSystem;
using UnityEngine.Events;

namespace Common.Events
{
    public class ActionEvent : UnityEvent<ActionEventData>
    {
        
    }
    public class SelectionEvent : UnityEvent<SelectionEventData>
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
    
    public struct UnitEventData 
    {
        public Unit unit;
        public static implicit operator UnitEventData(Unit unit)
        {
            return new UnitEventData { unit = unit };
        }
    }
}