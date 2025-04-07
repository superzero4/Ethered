using System.Linq;
using Common.Events;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

namespace Views.Battle.Selection
{
    public class SelectionHint : MonoBehaviour
    {
        private int level = 0;

        [SerializeField] private Renderer[] _renderers;

        //[SerializeField] private Material _materials;
        public int Level
        {
            get => level;
            set
            {
                var temp = Mathf.Clamp(value, 0, _renderers.Length);
                if (temp == level)
                    return;
                var diff = Mathf.Abs(temp - level);
                if (diff == 1)
                {
                    if (temp > level)
                        _renderers[temp - 1].enabled = true;
                    else
                        _renderers[level - 1].enabled = false;
                }
                else
                {
                    for (int i = 0; i < _renderers.Length; i++)
                        _renderers[i].enabled = i == temp - 1;
                }

                level = temp;
            }
        }

        public void Deactivate()
        {
            level = 0;
            foreach (var renderer in _renderers)
                renderer.enabled = false;
        }

        public void TogglePartial()
        {
            level = 1;
            _renderers[0].enabled = !(_renderers[0].enabled);
        }
    }
}