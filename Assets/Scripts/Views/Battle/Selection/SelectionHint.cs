using System;
using System.Collections.Generic;
using Common;
using Common.Events;
using NaughtyAttributes;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

namespace Views.Battle.Selection
{
    [Serializable]
    public class SelectionHintManager
    {
        [SerializeField] [ReadOnly] private Stack<SelectionHint> _actives;
        [SerializeField] [ReadOnly] private Queue<SelectionHint> _inactives;
        private bool HasCurrent => _actives.Count > 0;

        private SelectionHint current
        {
            get { return HasCurrent ? _actives.Peek() : null; }
        }

        public SelectionHintManager(IEnumerable<SelectionHint> hints)
        {
            _inactives = new Queue<SelectionHint>(hints);
            DeactivateInactives();
            _actives = new Stack<SelectionHint>();
        }

        public void Clear()
        {
            while (_actives.Count > 0)
                _inactives.Enqueue(_actives.Pop());
            //All inactives => we can update the appeareance
            DeactivateInactives();
        }

        public void Lock()
        {
            //if (!HasCurrent)
            //    ActivateNew();
            current.Lock();
        }

        public void ActivateNew()
        {
            //If we pull the last one, we create another after in advance
            if (_inactives.Count == 1)
                _inactives.Enqueue(_inactives.Peek().Copy());
            _actives.Push(_inactives.Dequeue());
            current.Activate();
        }

        public bool Unlock()
        {
            current.Deactivate();
            _inactives.Enqueue(_actives.Pop());
            if (HasCurrent)
                current.Activate();
            return true;
        }

        public void Hint(Selectable s, bool b = true)
        {
            if (!HasCurrent)
                ActivateNew();
            current.Place(s, b);
        }

        private void DeactivateInactives()
        {
            foreach (var hint in _inactives)
                hint.Deactivate();
        }
    }

    public class SelectionHint : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _normal;
        [SerializeField] private Material _alt;
        public bool IsActive => _renderer.enabled;

        private void Awake()
        {
            //initialColor = _renderer.material.color;
            //_reference ??= this;
        }


        public void Deactivate()
        {
            _renderer.enabled = false;
            _renderer.material = _normal;
        }

        public void Activate()
        {
            _renderer.enabled = true;
            _renderer.material = _normal;
        }

        private void SetColor(Color color, bool emmision)
        {
            _renderer.material.color = color;
            _renderer.material.SetColor("_Emmision", color);
            _renderer.material.globalIlluminationFlags = emmision
                ? MaterialGlobalIlluminationFlags.None
                : MaterialGlobalIlluminationFlags.EmissiveIsBlack;
        }

        public void Lock()
        {
            _renderer.material = _alt;
        }

        public void Place(Selectable selectable, bool b = true)
        {
            _renderer.enabled = b;
            transform.position = selectable.HintAnchor.position;
        }

        public SelectionHint Copy()
        {
            var go = GameObject.Instantiate(this, transform.parent);
            //go.SetColor(initialColor, true);
            go.gameObject.SetActive(gameObject.activeSelf);
            if (go.IsActive) go.Activate();
            else go.Deactivate();
            return go;
        }
    }
}