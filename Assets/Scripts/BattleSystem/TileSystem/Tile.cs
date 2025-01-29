using System;
using JetBrains.Annotations;
using UnitSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleSystem.TileSystem
{
    [Serializable]
    public class Tile
    {
        public Tile(Tile other)
        {
            _base = other._base;
            _unit = other._unit;
        }

        public bool Empty => _unit == null;

        public Tile(Environment baseElement, Unit unit)
        {
            _base = baseElement;
            _unit = unit;
        }

        public EPhase Phase
        {
            get
            {
                Assert.AreNotEqual(_base.Position.Phase, _unit.Position.Phase);
                return _unit.Position.Phase;
            }
        }

        [SerializeField] private Environment _base;
        [SerializeField] [CanBeNull] private Unit _unit;

        public Environment Base
        {
            get => _base;
            set => _base = value;
        }

        public Unit Unit
        {
            get => _unit;
            set => _unit = value;
        }
    }
}