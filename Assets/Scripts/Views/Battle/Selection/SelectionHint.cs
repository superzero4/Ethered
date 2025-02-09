using Common.Events;
using UnityEngine;

namespace Views.Battle.Selection
{
    public class SelectionHint : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;

        public void HideHint()
        {
            _renderer.enabled = false;
        }
        public void Hint(Selectable selectable, bool b = true)
        {
            _renderer.enabled = b;
            transform.position = selectable.HintAnchor.position;
        }
    }
}