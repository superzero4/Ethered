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
                    if (!_cache.TryAdd(s.Tile.Base.Position, s))
                        Assert.IsTrue(
                            s.Tile.Base.Position.Phase != EPhase.Both && s.Tile.Base.Position.Phase != EPhase.Normal &&
                            s.Tile.Base.Position.Phase != EPhase.Ethered, "A non-ignored tile has been added twice");
        }

        public void Clear()
        {
            foreach (var s in _cache.Values)
                s.Hint.Deactivate();
        }


        public void Hint(PositionData s)
        {
            if (_cache.TryGetValue(s, out var selectable))
                selectable.Hint.TogglePartial();
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