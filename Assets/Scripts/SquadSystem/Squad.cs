using System;
using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using UnitSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace SquadSystem
{
    [Serializable]
    public struct Squad
    {
        public Squad(Squad other)
        {
            _units = new List<UnitInfo>(other._units.Select(u => new UnitInfo(u)));
            _upgrades = new List<Upgrade>(other._upgrades);
            Coins = other.Coins;
            Ether = other.Ether;
        }

        [SerializeField] private List<UnitInfo> _units;

        public void Trim(int size)
        {
            if (size < _units.Count)
                _units = _units.Take(size).ToList();
        }

        public List<UnitInfo> Units => _units;

        [SerializeField] private List<Upgrade> _upgrades;
        public List<Upgrade> Upgrades => _upgrades;

        public int SquadSize => Units.Count;

        public int Coins { get; set; } // Basic currency
        public int Ether { get; set; } // Alternative currency

        public void Init(int nbUnits, UnitInfo info)
        {
            _units = new List<UnitInfo>(Enumerable.Range(0, nbUnits).Select(i => new UnitInfo(info)));
        }
    }
}