using UnitSystem;
using UnityEngine;

namespace SquadSystem.Buttons
{
    public class GlobalUpgradeButton : MonoBehaviour
    {
        [SerializeField] private int coinsCost;
        [SerializeField] private int etherCost;
        [SerializeField] private SquadClass squadClass;
        [SerializeField] private SquadMenu squadMenu;
        
        public void ArmorUpgrade()
        {
            if (squadClass.SquadSize == 0)
            {
                // TODO : display a message to the player that he doesn't have any units in his squad
                return;
            }
            
            if (squadClass.Coins < coinsCost || squadClass.Ether < etherCost)
            {
                // TODO : display a message to the player that he doesn't have enough coins or ether
                return;
            }
            
            squadMenu.UpdateCoins(-coinsCost);
            squadMenu.UpdateEther(-etherCost);
            
            foreach (UnitInfo unit in squadClass.Units)
            {
                unit.Armor += 5;
            }
        }
    }
}