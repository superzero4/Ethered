using System;
using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using Common;
using Common.Events;
using NaughtyAttributes;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

namespace Views.Battle.Selection
{
    public class SimpleSelectionHintManager
    {
        private Dictionary<PositionData, Selectable> _cache;

        public SimpleSelectionHintManager(IEnumerable<Selectable> selectables = null)
        {
            _cache = new();
            if (selectables != null)
                foreach (var s in selectables)
                    _cache.Add(s.Tile.Base.Position, s);
        }

        public void Clear()
        {
            foreach (var s in _cache.Values)
                s.Hint.Deactivate();
        }


        public void Hint(PositionData s, bool b, bool altMaterial)
        {
            if (_cache.TryGetValue(s, out var selectable))
                selectable.Hint.Toggle(b, altMaterial);
            else
                Assert.IsTrue(false);
        }

        public void Lock()
        {
        }
    }

    //[Serializable]
    //public class SelectionHintManager : ISelectionHintManager
    //{
    //    [SerializeField] [ReadOnly] private Stack<SelectionHint> _actives;
    //    [SerializeField] [ReadOnly] private Queue<SelectionHint> _inactives;
    //    private bool HasCurrent => _actives.Count > 0;
//
    //    private SelectionHint current
    //    {
    //        get { return HasCurrent ? _actives.Peek() : null; }
    //    }
//
    //    public SelectionHintManager(IEnumerable<SelectionHint> hints)
    //    {
    //        _inactives = new Queue<SelectionHint>(hints);
    //        DeactivateInactives();
    //        _actives = new Stack<SelectionHint>();
    //    }
//
    //    public void Clear()
    //    {
    //        while (_actives.Count > 0)
    //            _inactives.Enqueue(_actives.Pop());
    //        //All inactives => we can update the appeareance
    //        DeactivateInactives();
    //    }
//
    //    public void Lock()
    //    {
    //        //if (!HasCurrent)
    //        //    ActivateNew();
    //        current.Lock();
    //    }
//
    //    public void ActivateNew()
    //    {
    //        //If we pull the last one, we create another after in advance
    //        if (_inactives.Count == 1)
    //            _inactives.Enqueue(_inactives.Peek().Copy());
    //        _actives.Push(_inactives.Dequeue());
    //        current.Activate();
    //    }
//
    //    public bool Unlock()
    //    {
    //        current.Deactivate();
    //        _inactives.Enqueue(_actives.Pop());
    //        if (HasCurrent)
    //            current.Activate();
    //        return true;
    //    }
//
    //    public void Hint(Selectable s, bool b = true)
    //    {
    //        if (!HasCurrent)
    //            ActivateNew();
    //        current.Place(s, b);
    //    }
//
    //    private void DeactivateInactives()
    //    {
    //        foreach (var hint in _inactives)
    //            hint.Deactivate();
    //    }
    //}

    public class SelectionHint : MonoBehaviour
    {
        private int level = 0;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material[] _materials;
        private Material _normal => _materials[0];
        private Material _alt => _materials[1];

        public void Deactivate()
        {
            level = 0;
            DisplayBasedOnLevel();
        }

        public void Toggle(bool b, bool altMaterial = false)
        {
            _renderer.enabled = b;
            level = altMaterial ? 2 : 1;
            DisplayBasedOnLevel();
        }

        public void DisplayBasedOnLevel()
        {
            _renderer.enabled = level > 0;
            _renderer.material = level > 1 ? _materials[level - 1] : _materials[0];
        }

        public void Increment()
        {
            level++;
            DisplayBasedOnLevel();
        }

        public void Decrement()
        {
            level--;
            DisplayBasedOnLevel();
        }
    }
}