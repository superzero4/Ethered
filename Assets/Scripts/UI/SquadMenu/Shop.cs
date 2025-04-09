using System.Collections.Generic;
using SquadSystem.Buttons;
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
        [SerializeField] private GameObject squadMemberShopPrefab; // The prefab of the squad member in the shop (UI)
        
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
    }
}