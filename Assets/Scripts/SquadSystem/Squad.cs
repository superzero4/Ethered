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
        [SerializeField] private List<UnitInfo> _units;
        public List<UnitInfo> Units => _units;
        public void Init(int nbUnits, UnitInfo info)
        {
            _units = new List<UnitInfo>(Enumerable.Repeat(info, nbUnits));
        }
    }
}