using UnitSystem;
using UnityEngine;

namespace SquadSystem.Buttons
{
    public class GlobalUpgradeButton : MonoBehaviour
    {
        [Header("Editor References")]
        [SerializeField] private SquadClass squadClass;
        [SerializeField] private SquadMenu squadMenu;
        [SerializeField] private UpgradeList upgradeList;
        [SerializeField] private Upgrade upgrade;
        
        private string UpgradeName => upgrade.upgradeReference.Name;
        private int CoinsCost => upgrade.upgradeReference.CoinsCost;
        private int EtherCost => upgrade.upgradeReference.EtherCost;
        
        public void ArmorUpgrade()
        {
            if (squadClass.SquadSize == 0)
            {
                // TODO : display a message to the player that he doesn't have any units in his squad
                return;
            }
            
            if (squadClass.Coins < CoinsCost || squadClass.Ether < EtherCost)
            {
                // TODO : display a message to the player that he doesn't have enough coins or ether
                return;
            }
            
            squadMenu.UpdateCoins(-CoinsCost);
            squadMenu.UpdateEther(-EtherCost);
            
            foreach (UnitInfo unit in squadClass.Units)
            {
                unit.Armor += 5;
            }

            // Add the upgrade to the list of upgrades
            upgradeList.AddUpgrade(upgrade);
        }
    }
}