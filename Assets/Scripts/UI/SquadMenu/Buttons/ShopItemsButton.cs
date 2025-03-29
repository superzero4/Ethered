using UnityEngine;

namespace SquadSystem.Buttons
{
    public class ShopItemsButton : MonoBehaviour
    {
        [SerializeField] private string itemName;
        [SerializeField] private int coinsCost;
        [SerializeField] private int etherCost;
        [SerializeField] private int quantity = 1; // Default quantity is 1
        [SerializeField] private Inventory inventoryRef;
        [SerializeField] private SquadClass squadClass;
        [SerializeField] private SquadMenu squadMenu;
        
        /// <summary>
        /// This method is called when the player clicks on the button to buy an item
        /// </summary>
        public void OnClick()
        {
            // Check if the player has enough coins or ether to buy the item
            if (squadClass.Coins < coinsCost || squadClass.Ether < etherCost)
            {
                // TODO : display a message to the player that he doesn't have enough coins or ether
                return;
            }
            
            squadMenu.UpdateCoins(-coinsCost);
            squadMenu.UpdateEther(-etherCost);
            
            inventoryRef.AddItem(itemName, quantity);
        }
    }
}