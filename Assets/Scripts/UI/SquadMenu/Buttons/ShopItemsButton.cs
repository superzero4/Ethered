using SquadSystem.Items;
using UnityEngine;

namespace SquadSystem.Buttons
{
    public class ShopItemsButton : MonoBehaviour
    {
        [SerializeField] private Item itemRef;
        [SerializeField] private int quantity = 1; // Default quantity is 1
        [SerializeField] private Inventory inventoryRef;
        [SerializeField] private SquadClass squadClass;
        [SerializeField] private SquadMenu squadMenu;
        
        /// <summary>
        /// This method is called when the player clicks on the button to buy an item
        /// </summary>
        public void OnClick()
        {
            int coinsCost = itemRef.GetItemCoinsCost();
            int etherCost = itemRef.GetItemEtherCost();
            
            // Check if the player has enough coins or ether to buy the item
            if (squadClass.Coins < coinsCost || squadClass.Ether < etherCost)
            {
                // TODO : display a message to the player that he doesn't have enough coins or ether
                return;
            }
            
            squadMenu.UpdateCoins(-coinsCost);
            squadMenu.UpdateEther(-etherCost);
            
            inventoryRef.AddItem(itemRef, quantity);
        }

        /// <summary>
        /// Set the parameters of the shop item button
        /// </summary>
        /// <param name="item"></param>
        public void SetParameters(Item item)
        {
            itemRef = item;
            quantity = 1; // Default quantity is 1
            inventoryRef = FindFirstObjectByType<Inventory>();
            squadClass = FindFirstObjectByType<SquadClass>();
            squadMenu = FindFirstObjectByType<SquadMenu>();
        }
    }
}