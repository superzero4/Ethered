using System.Collections.Generic;
using SquadSystem.Buttons;
using SquadSystem.Items;
using SquadSystem.UI;
using UnitSystem;
using UnityEngine;

namespace SquadSystem
{
    /// <summary>
    /// This class is used to manage the shop
    /// It contains the list of items that can be bought and generates the shop randomly based on a given list of possible items
    /// </summary>
    public class Shop : MonoBehaviour
    {
        [Header("Squad Members Shop Settings")]
        [SerializeField] private int minSquadMembersInShop = 1;
        [SerializeField] private int maxSquadMembersInShop = 3;
        [SerializeField] private List<UnitInfo> squadMembersList;
        [SerializeField] private GameObject squadMemberShopContainer; // The container where the squad members will be displayed (UI)
        [SerializeField] private GameObject squadMemberShopPrefab; // The prefab of the squad member button in the shop (UI)
        
        [Header("Global Upgrades Shop Settings")]
        [SerializeField] private int minGlobalUpgradesInShop = 1;
        [SerializeField] private int maxGlobalUpgradesInShop = 3;
        [SerializeField] private List<Upgrade> globalUpgradesList;
        [SerializeField] private GameObject globalUpgradesShopContainer; // The container where the global upgrades will be displayed (UI)
        [SerializeField] private GameObject globalUpgradesShopPrefab; // The prefab of the global upgrade button in the shop (UI)
        
        [Header("Items Shop Settings")]
        [SerializeField] private int minItemsInShop = 1;
        [SerializeField] private int maxItemsInShop = 3;
        [SerializeField] private List<Item> itemsList;
        [SerializeField] private GameObject itemsShopContainer; // The container where the items will be displayed (UI)
        [SerializeField] private GameObject itemsShopPrefab; // The prefab of the item button in the shop (UI)

        #region Squad Members Shop
        
        /// <summary>
        /// Generates the squad member tab from the shop.
        /// This method uses the squadMembersList to add the squad members in the shop.
        /// </summary>
        /// <param name="squadMembersInShop"></param>
        public void GenerateSquadMemberShopWithTheList(int squadMembersInShop)
        {
            for (int i = 0; i < squadMembersInShop; i++)
            {
                int squadMemberPicked = Random.Range(0, squadMembersList.Count);
                
                GameObject squadMember = Instantiate(squadMemberShopPrefab, squadMemberShopContainer.transform);
                SquadMemberShopUI squadMemberShopUI = squadMember.GetComponent<SquadMemberShopUI>();
                squadMemberShopUI.SetParameters(
                    i.ToString(), 
                    squadMembersList[squadMemberPicked].MaxHealth, 
                    squadMembersList[squadMemberPicked].Armor,
                    10,
                    0
                    );
                
                SquadMemberButton squadMemberButton = squadMember.GetComponent<SquadMemberButton>();
                squadMemberButton.SetParameters(
                    squadMembersList[squadMemberPicked].MaxHealth, 
                    squadMembersList[squadMemberPicked].Armor,
                    10,
                    0
                    );
                
                squadMembersList.RemoveAt(squadMemberPicked); // Remove the squad member from the list to avoid duplicates
            }
        }

        /// <summary>
        /// Generates the squad member tab from the shop.
        /// This method generates the squad members in the shop randomly.
        /// </summary>
        public void GenerateSquadMemberShopRandomly()
        {
            int squadMembersInShop = Random.Range(minSquadMembersInShop, maxSquadMembersInShop + 1);
            for (int i = 0; i < squadMembersInShop; i++)
            {
                int randomHealth = Random.Range(50, 100);
                int randomArmor = Random.Range(0, 50);
                
                GameObject squadMember = Instantiate(squadMemberShopPrefab, squadMemberShopContainer.transform);
                SquadMemberShopUI squadMemberShopUI = squadMember.GetComponent<SquadMemberShopUI>();
                squadMemberShopUI.SetParameters(
                    i.ToString(), 
                    randomHealth, 
                    randomArmor,
                    10,
                    0
                    );
                
                SquadMemberButton squadMemberButton = squadMember.GetComponent<SquadMemberButton>();
                squadMemberButton.SetParameters(
                    randomHealth, 
                    randomArmor,
                    10,
                    0
                    );
            }
        }
        
        #endregion
        
        #region Global Upgrades Shop
        
        /// <summary>
        /// Generates the global upgrades tab from the shop.
        /// This method uses the globalUpgradesList to add the global upgrades in the shop.
        /// </summary>
        /// <param name="globalUpgradesInShop"></param>
        public void GenerateGlobalUpgradesShopWithTheList(int globalUpgradesInShop)
        {
            for (int i = 0; i < globalUpgradesInShop; i++)
            {
                int globalUpgradePicked = Random.Range(0, globalUpgradesList.Count);
                
                GameObject globalUpgrade = Instantiate(globalUpgradesShopPrefab, globalUpgradesShopContainer.transform);
                UpgradeShopUI globalUpgradeUI = globalUpgrade.GetComponent<UpgradeShopUI>();
                globalUpgradeUI.SetParameters(
                    globalUpgradesList[globalUpgradePicked].GetUpgradeName(), 
                    globalUpgradesList[globalUpgradePicked].GetUpgradeCoinsCost(),
                    globalUpgradesList[globalUpgradePicked].GetUpgradeEtherCost()
                    );
                
                GlobalUpgradeButton globalUpgradeButton = globalUpgrade.GetComponent<GlobalUpgradeButton>();
                globalUpgradeButton.SetParameters(globalUpgradesList[globalUpgradePicked]);
                
                globalUpgradesList.RemoveAt(globalUpgradePicked); // Remove the global upgrade from the list to avoid duplicates
            }
        }
        
        #endregion
        
        #region Items Shop
        
        /// <summary>
        /// Generates the items tab from the shop.
        /// This method uses the itemsList to add the items in the shop.
        /// </summary>
        /// <param name="itemsInShop"></param>
        public void GenerateItemsShopWithTheList(int itemsInShop)
        {
            for (int i = 0; i < itemsInShop; i++)
            {
                int itemPicked = Random.Range(0, itemsList.Count);
                
                string itemName = itemsList[itemPicked].GetItemName();
                int itemCoinsCost = itemsList[itemPicked].GetItemCoinsCost();
                int itemEtherCost = itemsList[itemPicked].GetItemEtherCost();
                
                GameObject item = Instantiate(itemsShopPrefab, itemsShopContainer.transform);
                ItemShopUI itemShopUI = item.GetComponent<ItemShopUI>();
                itemShopUI.SetParameters(
                    itemName, 
                    itemCoinsCost,
                    itemEtherCost
                    );
                
                ShopItemsButton itemButton = item.GetComponent<ShopItemsButton>();
                itemButton.SetParameters(itemsList[itemPicked]);
                
                itemsList.RemoveAt(itemPicked); // Remove the item from the list to avoid duplicates
            }
        }
        
        #endregion
    }
}