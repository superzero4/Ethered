using System.Collections.Generic;
using UnityEngine;

namespace SquadSystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Dictionary<string, int> inventory; // TODO : make the class serializable
        
        private void Awake()
        {
            inventory = new Dictionary<string, int>();
        }
        
        /// <summary>
        /// Add an item to the inventory
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(string item)
        {
            if (!inventory.TryAdd(item, 1))
            {
                inventory[item]++;
            }
        }
        
        /// <summary>
        /// Add an item to the inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void AddItem(string item, int quantity)
        {
            if (!inventory.TryAdd(item, quantity))
            {
                inventory[item] += quantity;
            }
        }
        
        /// <summary>
        /// Remove an item from the inventory
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(string item)
        {
            if (!inventory.ContainsKey(item)) return;
            inventory[item]--;
            if (inventory[item] <= 0)
            {
                inventory.Remove(item);
            }
        }
        
        /// <summary>
        /// Remove an item from the inventory in a certain quantity if possible
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void RemoveItem(string item, int quantity)
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
        public bool ContainsItem(string item)
        {
            return inventory.ContainsKey(item);
        }
        
        /// <summary>
        /// Check if the inventory contains an item in a certain quantity
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool ContainsItem(string item, int quantity)
        {
            return inventory.TryGetValue(item, out int value) && value >= quantity;
        }
        
        /// <summary>
        /// Get the number of items in the inventory
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetItemCount(string item)
        {
            return inventory.TryGetValue(item, out int value) ? value : 0;
        }
        
        /// <summary>
        /// Get the list of items in the inventory
        /// </summary>
        /// <returns></returns>
        public List<string> GetItems()
        {
            return new List<string>(inventory.Keys);
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
        public Dictionary<string, int> GetInventory()
        {
            return inventory;
        }
        
        /// <summary>
        /// Set the inventory
        /// </summary>
        /// <param name="newInventory"></param>
        public void SetInventory(Dictionary<string, int> newInventory)
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