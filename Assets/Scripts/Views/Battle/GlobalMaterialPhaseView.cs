using System;
using BattleSystem;
using Common.Events;
using UnityEngine;
using Views.Battle.Selection;

namespace Views.Battle
{
    public class GlobalMaterialPhaseView : MonoBehaviour, IPhaseView
    {
        [SerializeField] private Material[] _materials;
        
        public void OnPhaseSelected(PhaseEventData arg0)
        {
            if(gameObject.activeInHierarchy && this.isActiveAndEnabled)
                SetColor(arg0.phase);
            else
                Debug.LogWarning("GlobalMaterialPhaseView is not active or enabled, even if it's listening to the event");
        }

        public void SetColor(EPhase arg0)
        {
            foreach (var material in _materials)
            {
                if (false && material.HasColor("_Color"))
                {
                    var color = PickColor(arg0, false);
                    material.SetColor("_Color", color);
                }
                else
                {
                    var color = PickColor(arg0, true);
                    material.SetColor("_EmissionColor", color);
                }
            }
        }

        public Color PickColor(EPhase arg0Phase, bool isEmmision)
        {
            var color = Color.grey;
            switch (arg0Phase)
            {
                case EPhase.Normal: color = isEmmision ? Color.black : Color.white; break;
                case EPhase.Ethered:
                    //HDR color, will be clamped to 1 either
                    color = Color.blue * 20;
                    if (!isEmmision)
                        color.a = 0.5f;
                    break;
                case EPhase.Both: color = (Color.blue) / 2f; break;
            }

            return color;
        }

        private void OnDestroy()
        {
            SetColor(EPhase.Normal);
        }
    }
}