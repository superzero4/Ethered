using UnityEngine;

namespace SquadSystem.Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private ItemSO itemSO;
        
        /// <summary>
        /// Set the item parameters
        /// </summary>
        /// <param name="itemSO"></param>
        public void SetParameters(ItemSO itemSO)
        {
            this.itemSO = itemSO;
        }
        
        /// <summary>
        /// Set the item parameters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="coinsPrice"></param>
        /// <param name="etherPrice"></param>
        public void SetParameters(string name, int coinsPrice, int etherPrice)
        {
            itemSO = ScriptableObject.CreateInstance<ItemSO>();
            itemSO.Name = name;
            itemSO.CoinsPrice = coinsPrice;
            itemSO.EtherPrice = etherPrice;
        }
        
        /// <summary>
        /// Get the item name
        /// </summary>
        /// <returns></returns>
        public string GetItemName()
        {
            return itemSO.Name;
        }
        
        /// <summary>
        /// Get the item coins cost
        /// </summary>
        /// <returns></returns>
        public int GetItemCoinsCost()
        {
            return itemSO.CoinsPrice;
        }
        
        /// <summary>
        /// Get the item ether cost
        /// </summary>
        /// <returns></returns>
        public int GetItemEtherCost()
        {
            return itemSO.EtherPrice;
        }
    }
}