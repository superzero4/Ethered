using System;
using System.Linq;
using Common.Events;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Views.Battle.Selection
{
    public class SelectionHint : MonoBehaviour
    {
        [SerializeField] private Renderer[] _renderers;

        [FormerlySerializedAs("_color")] [SerializeField]
        private Color[] _colorLevels;

        public int Level
        {
            set
            {
                ToggleAll(value >= 1,
                    _colorLevels != null && _colorLevels.Length > 0
                        ? _colorLevels[Mathf.Clamp(value - 1, 0, _colorLevels.Length - 1)]
                        : null);
            }
        }

        private void ToggleAll(bool value, Color? color)
        {
            foreach (var renderer in _renderers)
                if (renderer != null)
                {
                    renderer.enabled = value;
                    if (color.HasValue)
                        renderer.material.color = color.Value;
                }
        }

        public void Deactivate()
        {
            Level = 0;
        }

        public void TogglePartial()
        {
            Level = 1;
        }
    }
}