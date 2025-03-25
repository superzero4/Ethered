using System.Collections.Generic;
using SquadSystem.UI;
using UnityEngine;

namespace SquadSystem.Buttons
{
    public class LoadInventoryButton : MonoBehaviour
    {
        [Header("Editor References")]
        [SerializeField] private Inventory inventoryReference; // Reference to the inventory
        [SerializeField] private GameObject inventoryContainer; // The container that holds the inventory items (UI)
        [SerializeField] private GameObject inventoryItemPrefab; // The prefab of the inventory item (UI)
        
        /// <summary>
        /// Load the inventory of the squad when the player clicks on the button.
        /// It displays the inventory in the UI.
        /// </summary>
        public void LoadInventory()
        {
            Dictionary<string, int> inventory = inventoryReference.GetInventory();
            
            foreach (KeyValuePair<string, int> item in inventory)
            {
                GameObject inventoryItem = Instantiate(inventoryItemPrefab, inventoryContainer.transform);
                InventoryUI inventoryUI = inventoryItem.GetComponent<InventoryUI>();
                inventoryUI.SetParameters(
                    item.Key, 
                    item.Value
                    );
            }
        }
        
        /// <summary>
        /// Clear the inventory container when the player clicks on the button.
        /// </summary>
        public void ClearInventoryContainer()
        {
            foreach (Transform child in inventoryContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}