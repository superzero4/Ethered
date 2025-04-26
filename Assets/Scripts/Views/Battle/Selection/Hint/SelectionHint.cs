using System.Linq;
using Common.Events;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

namespace Views.Battle.Selection
{
    public class SelectionHint : MonoBehaviour
    {
        [SerializeField] private Renderer[] _renderers;

        //[SerializeField] private Material _materials;
        public int Level
        {
            set
            {
                foreach (var renderer in _renderers)
                {
                    if (renderer != null)
                    {
                        renderer.enabled = value > 0;
                    }
                }
            }
        }

        public void Deactivate()
        {
            Level = 0;
        }

        public void TogglePartial()
        {
            Level = 1;
            //_renderers[0].enabled = !(_renderers[0].enabled);
        }
    }
}