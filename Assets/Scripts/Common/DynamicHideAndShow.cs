using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Common
{
    public class DynamicHideAndShow<Panel> : IReset where Panel : MonoBehaviour
    {
        [SerializeField] private Panel _prefab;
        [SerializeField, Range(0, 200)] private int _initialCount = 4;
        [SerializeField] private List<Panel> _panels;

        public DynamicHideAndShow(List<Panel> panels)
        {
            _panels = panels;
            _prefab = panels[0];
        }

        public DynamicHideAndShow(Panel prefab, int initialCount, Transform root)
        {
            _prefab = prefab;
            _initialCount = initialCount;
            _panels = new List<Panel>(_initialCount);
            for (int i = 0; i < _initialCount; i++)
            {
                CreateNewPanel(root);
            }
        }

        private void CreateNewPanel(Transform transform)
        {
            var actionUI = GameObject.Instantiate(_prefab, transform);
            actionUI.gameObject.SetActive(false);
            _panels.Add(actionUI);
        }

        public void HideActionPanelsFrom(int i)
        {
            for (; i < _panels.Count; i++)
                _panels[i].gameObject.SetActive(false);
        }

        public void SetPanels<T>(IEnumerable<T> toSet, Action<T, Panel> setter)
        {
            int i = 0;
            foreach (var t in toSet)
            {
                while (i >= _panels.Count)
                    CreateNewPanel(_panels[^1].transform.parent);
                var actionUI = _panels[i];
                actionUI.gameObject.SetActive(true);
                setter(t, actionUI);
                i++;
            }

            HideActionPanelsFrom(i);
        }

        public void Reset()
        {
            foreach (var panel in _panels)
            {
                //panel.Reset();
            }

            HideActionPanelsFrom(0);
        }

        public Panel At(int index)
        {
            //If we lack panels
            while (index >= _panels.Count)
                CreateNewPanel(_panels[^1].transform.parent);
            return _panels[index];
        }
    }
}