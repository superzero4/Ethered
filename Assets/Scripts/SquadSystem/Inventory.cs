using System.Collections.Generic;
using SquadSystem.Items;
using UnityEngine;

namespace SquadSystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Dictionary<Item, int> inventory; // TODO : make the class serializable
        
        private void Awake()
        {
            inventory = new Dictionary<Item, int>();
        }
        
        /// <summary>
        /// Add an item to the inventory
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(Item item)
        {
            AddItem(item, 1);
        }
        
        /// <summary>
        /// Add an item to the inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void AddItem(Item item, int quantity)
        {
            inventory ??= new Dictionary<Item, int>();
            if (!inventory.TryAdd(item, quantity))
            {
                inventory[item] += quantity;
            }
        }
        
        /// <summary>
        /// Remove an item from the inventory
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(Item item)
        {
            if (!inventory.ContainsKey(item)) return;
            inventory.Remove(item);
        }
        
        /// <summary>
        /// Remove an item from the inventory in a certain quantity if possible
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void RemoveItem(Item item, int quantity)
        {
            if (!inventory.ContainsKey(item) || inventory[item] < quantity) return;
            inventory[item] -= quantity;
            if (inventory[item] <= 0)
            {
                inventory.Remove(item);
            }
        }
        
        /// <summary>
        /// Check if the inventory contains an item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool ContainsItem(Item item)
        {
            return inventory.ContainsKey(item);
        }
        
        /// <summary>
        /// Check if the inventory contains an item in a certain quantity
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool ContainsItem(Item item, int quantity)
        {
            return inventory.TryGetValue(item, out int value) && value >= quantity;
        }
        
        /// <summary>
        /// Get the number of items in the inventory
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetItemCount(Item item)
        {
            return inventory.TryGetValue(item, out int value) ? value : 0;
        }
        
        /// <summary>
        /// Get the list of items in the inventory
        /// </summary>
        /// <returns></returns>
        public List<Item> GetItems()
        {
            return new List<Item>(inventory.Keys);
        }
        
        /// <summary>
        /// Clear the inventory
        /// </summary>
        public void ClearInventory()
        {
            inventory.Clear();
        }
        
        /// <summary>
        /// Get the number of items in the inventory
        /// </summary>
        /// <returns></returns>
        public int GetInventorySize()
        {
            return inventory.Count;
        }
        
        /// <summary>
        /// Get the inventory
        /// </summary>
        /// <returns></returns>
        public Dictionary<Item, int> GetInventory()
        {
            return inventory;
        }
        
        /// <summary>
        /// Set the inventory
        /// </summary>
        /// <param name="newInventory"></param>
        public void SetInventory(Dictionary<Item, int> newInventory)
        {
            inventory = newInventory;
        }
        
        /// <summary>
        /// Check if the inventory is empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return inventory.Count == 0;
        }
    }
}