using UnitSystem;
using UnityEngine;

namespace SquadSystem.Buttons
{
    public class SquadMemberButton : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private int armor;
        [SerializeField] private int coinsCost;
        [SerializeField] private int etherCost;
        [SerializeField] private SquadClass squadClass;
        [SerializeField] private SquadMenu squadMenu;
        
        /// <summary>
        /// This method is called when the player clicks on the button to buy a unit
        /// </summary>
        public void OnClick()
        {
            // Check if the player has enough coins or ether to buy the unit
            if (squadClass.Coins < coinsCost || squadClass.Ether < etherCost)
            {
                // TODO : display a message to the player that he doesn't have enough coins or ether
                return;
            }
            
            squadMenu.UpdateCoins(-coinsCost);
            squadMenu.UpdateEther(-etherCost);
            
            UnitInfo unitInfo = new()
            {
                MaxHealth = maxHealth,
                Armor = armor
            };
            
            squadClass.AddUnit(unitInfo);
        }
        
    }
}