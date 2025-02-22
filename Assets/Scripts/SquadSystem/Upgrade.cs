using UnitSystem.Actions.Bases;
using UnityEngine;

namespace SquadSystem
{
    public class Upgrade : MonoBehaviour
    {
        public IActionInfo Current { get; set; }
        public IActionInfo Next { get; set; }
        
        public UpgradeSO UpgradeReference { get; set; }

        public int GetUpgradeCoinsCost()
        {
            return UpgradeReference.CoinsCost;
        }
        
        public int GetUpgradeEtherCost()
        {
            return UpgradeReference.EtherCost;
        }
    }
}