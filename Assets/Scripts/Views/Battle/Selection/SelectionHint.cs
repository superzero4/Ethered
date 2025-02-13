using System;
using System.Collections.Generic;
using Common.Events;
using NUnit.Framework;
using UnityEngine;

namespace Views.Battle.Selection
{
    public class SelectionHintManager
    {
        public Stack<SelectionHint> _actives;
        private Queue<SelectionHint> _inactives;

        private SelectionHint current
        {
            get
            {
                Assert.IsTrue(_actives.Count > 0);
                return _actives.Peek();
            }
        }

        public SelectionHintManager(IEnumerable<SelectionHint> hints)
        {
            _inactives = new Queue<SelectionHint>(hints);
            foreach (var hint in _inactives)
            {
                hint.Deactivate();
            }
            _actives = new Stack<SelectionHint>();
            ActivateNew();
        }

        public void Clear()
        {
            while (_actives.Count > 1)
                Unlock();
        }

        public void Lock()
        {
            current.Lock();
            ActivateNew();
        }

        private void ActivateNew()
        {
            //If we pull the last one, we create another after in advance
            if (_inactives.Count == 1)
                _inactives.Enqueue(_inactives.Peek().Copy());
            _actives.Push(_inactives.Dequeue());
            current.Activate();
        }

        public bool Unlock()
        {
            if (_actives.Count == 1)
            {
                return false;
            }

            current.Deactivate();
            _inactives.Enqueue(_actives.Pop());
            current.Activate();
            return true;
        }

        public void Hint(Selectable s, bool b = true)
        {
            current.Place(s, b);
        }
    }

    public class SelectionHint : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        private Color initialColor;
        public bool IsActive => _renderer.enabled;
        private void Awake()
        {
            initialColor = _renderer.material.color;
        }


        public void Deactivate()
        {
            _renderer.enabled = false;
        }

        public void Activate()
        {
            _renderer.enabled = true;
            SetColor(initialColor,true);
        }

        private void SetColor(Color color, bool emmision)
        {
            _renderer.material.color = color;
            _renderer.material.SetColor("_Emmision", color);
            _renderer.material.globalIlluminationFlags = emmision ? MaterialGlobalIlluminationFlags.None : MaterialGlobalIlluminationFlags.EmissiveIsBlack;
            
        }

        public void Lock()
        {
            var col = initialColor;
            col.a = .5f;
            SetColor(col,false);
        }

        public void Place(Selectable selectable, bool b = true)
        {
            _renderer.enabled = b;
            transform.position = selectable.HintAnchor.position;
        }

        public SelectionHint Copy()
        {
            var go = GameObject.Instantiate(this, transform.parent);
            go.gameObject.SetActive(gameObject.activeSelf);
            if(go.IsActive) go.Activate(); else go.Deactivate();
            return go;
        }
    }
}