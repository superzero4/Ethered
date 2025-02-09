using NaughtyAttributes;
using UnitSystem.Actions.Bases;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Battle
{
    public class ActionUI : ClickableUI
    {
        [SerializeReference] [ReadOnly] private IActionInfo _action;
        
        
    }
}