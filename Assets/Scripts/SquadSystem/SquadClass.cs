using System.Collections.Generic;
using UnitSystem;
using UnityEngine;

namespace SquadSystem
{
    public class SquadClass : MonoBehaviour
    {
        [SerializeField] private List<UnitInfo> units;
        public List<UnitInfo> Units => units;
        
        [SerializeField] private UpgradeList upgrades;
        [SerializeField] private Inventory inventory;
        
        public int SquadSize => units.Count;

        [SerializeField] private int coins; // Basic currency
        public int Coins
        {
            get => coins;
            set => coins = value;
        }
        
        [SerializeField] private int ether; // Alternative currency
        public int Ether
        {
            get => ether;
            set => ether = value;
        }
        
        public void AddUnit(UnitInfo unitInfo)
        {
            units.Add(unitInfo);
        }
    }
}