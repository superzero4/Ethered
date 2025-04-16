using Common.GlobalFlow;
using SquadSystem.Items;
using TMPro;
using UnitSystem;
using UnityEngine;

namespace SquadSystem
{
    public class SquadMenu : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text coinsText;
        [SerializeField] private TMP_Text etherText;
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private GameObject squadPanel;
        [SerializeField] private SquadClass squadClass;
        [SerializeField] private UpgradeList upgradeList;
        [SerializeField] private Inventory inventory;
        [SerializeField] private Shop shop;
        
        private Squad Squad { get; set; }

        private void Start()
        {
            Squad = squadClass.GetSquad();
            coinsText.SetText(Squad.Coins.ToString());
            etherText.SetText(Squad.Ether.ToString());
            InitInventory();
            //shop.GenerateSquadMemberShopRandomly();
            shop.GenerateSquadMemberShopWithTheList(2);
            //shop.GenerateGlobalUpgradesShopWithTheList(2);
            shop.GenerateItemsShopWithTheList(2);
        }
        
        /// <summary>
        /// Update the coins of the squad and the UI
        /// </summary>
        /// <param name="value"></param>
        public void UpdateCoins(int value)
        {
            if (value < 0 && squadClass.Coins + value < 0)
            {
                // TODO : display a message to the player that he doesn't have enough coins
                return;
            }
            
            squadClass.Coins += value;
            
            coinsText.SetText(squadClass.Coins.ToString());
        }
        
        /// <summary>
        /// Update the ether of the squad and the UI
        /// </summary>
        /// <param name="value"></param>
        public void UpdateEther(int value)
        {
            if (value < 0 && squadClass.Ether + value < 0)
            {
                // TODO : display a message to the player that he doesn't have enough ether
                return;
            }
            
            squadClass.Ether += value;
            
            etherText.SetText(squadClass.Ether.ToString());
        }
        
        /// <summary>
        /// Temporary method to initialize the inventory
        /// </summary>
        private void InitInventory()
        {
            Item healthKit = squadPanel.AddComponent<Item>();
            healthKit.SetParameters("Health Kit", 10, 0);
            inventory.AddItem(healthKit, 5);
            
            Debug.Log(inventory.GetInventorySize());
            
            Item grenade = squadPanel.AddComponent<Item>();
            grenade.SetParameters("Grenade", 0, 10);
            inventory.AddItem(grenade, 3);
            
            Debug.Log(inventory.GetInventorySize());
        }

        public void GoToNextScene()
        {
            squadClass.ForwardToBattle();
            SceneFlow.LoadScene(SceneFlow.EScene.Battle);
        }
    }
}