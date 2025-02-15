using Common;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HighlightUI : MonoBehaviour,IReset
    {
        [SerializeField] private Graphic _highlightGO;
        [SerializeField] private bool _canHighlight = true;

        public bool CanHighlight
        {
            get => _canHighlight;
            set => _canHighlight = value;
        }

        protected void Awake()
        {
            Reset();
        }

        public void Highlight()
        {
            if (_canHighlight)
                _highlightGO.enabled = true;
        }

        public virtual void Reset()
        {
            _highlightGO.enabled = false;
        }
    }
}