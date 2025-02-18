using System;
using BattleSystem;
using Common.Events;
using UnityEngine;
using Views.Battle.Selection;

namespace Views.Battle
{
    public class PhaseView : MonoBehaviour
    {
        [SerializeField] private Material[] _materials;
        [SerializeField] private PhaseSelector _selector;

        private void Awake()
        {
            _selector.OnSelectedPhaseChanges.AddListener(OnPhaseSelected);
        }

        private void OnPhaseSelected(PhaseEventData arg0)
        {
            SetColor(arg0.phase);
        }

        private void SetColor(EPhase arg0)
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

        private Color PickColor(EPhase arg0Phase, bool isEmmision)
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