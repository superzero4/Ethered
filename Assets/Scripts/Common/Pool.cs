using System;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common
{
    [Serializable]
    public class Pool<Pooled> : IReset where Pooled : MonoBehaviour
    {
        [SerializeField] private Pooled _prefab;
        [SerializeField, Range(0, 200)] private int _initialCount = 4;
        [FormerlySerializedAs("_panels")] [SerializeField] private List<Pooled> _elements;

        public Pool(List<Pooled> elements)
        {
            _elements = elements;
            _prefab = elements[0];
        }

        public Pool(Pooled prefab, int initialCount, Transform root)
        {
            _prefab = prefab;
            _initialCount = initialCount;
            _elements = new List<Pooled>(_initialCount);
            for (int i = 0; i < _initialCount; i++)
            {
                InstantiateNew(root);
            }
        }

        public List<Pooled> Elements => _elements;

        private void InstantiateNew(Transform transform)
        {
            var actionUI = GameObject.Instantiate(_prefab, transform);
            actionUI.gameObject.SetActive(false);
            _elements.Add(actionUI);
        }

        public void DisableAllFrom(int i)
        {
            for (; i < _elements.Count; i++)
                _elements[i].gameObject.SetActive(false);
        }

        public void SetElements<T>(IEnumerable<T> toSet, Action<T, Pooled> setter)
        {
            int i = 0;
            foreach (var t in toSet)
            {
                while (i >= _elements.Count)
                    InstantiateNew(_elements[^1].transform.parent);
                var actionUI = _elements[i];
                actionUI.gameObject.SetActive(true);
                setter(t, actionUI);
                i++;
            }

            DisableAllFrom(i);
        }

        public void Reset()
        {
            foreach (var panel in _elements)
            {
                //panel.Reset();
            }

            DisableAllFrom(0);
        }

        public Pooled At(int index)
        {
            //If we lack panels
            while (index >= _elements.Count)
                InstantiateNew(_elements[^1].transform.parent);
            return _elements[index];
        }
    }
}