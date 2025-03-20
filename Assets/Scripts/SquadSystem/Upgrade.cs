using SquadSystem.Enums;
using UnitSystem.Actions.Bases;
using UnityEngine;
using UnityEngine.Serialization;

namespace SquadSystem
{
    public class Upgrade : MonoBehaviour
    {
        public IActionInfo Current { get; set; }
        public IActionInfo Next { get; set; }

        [SerializeField] public UpgradeSO upgradeReference;
        
        /// <summary>
        /// Get the name of the upgrade
        /// </summary>
        /// <returns></returns>
        public string GetUpgradeName()
        {
            return upgradeReference.Name;
        }
        
        /// <summary>
        /// Get the type of the upgrade
        /// </summary>
        /// <returns></returns>
        public EUpgradeType GetUpgradeType()
        {
            return upgradeReference.UpgradeType;
        }

        /// <summary>
        /// Get the cost of the upgrade in coins
        /// </summary>
        /// <returns></returns>
        public int GetUpgradeCoinsCost()
        {
            return upgradeReference.CoinsCost;
        }
        
        /// <summary>
        /// Get the cost of the upgrade in ether
        /// </summary>
        /// <returns></returns>
        public int GetUpgradeEtherCost()
        {
            return upgradeReference.EtherCost;
        }
        
        /// <summary>
        /// Get the current tier of the upgrade
        /// </summary>
        /// <returns></returns>
        public int GetCurrentTier()
        {
            return upgradeReference.CurrentTier;
        }
        
        /// <summary>
        /// Get the max tier of the upgrade
        /// </summary>
        /// <returns></returns>
        public int GetMaxTier()
        {
            return upgradeReference.MaxTier;
        }
    }
}