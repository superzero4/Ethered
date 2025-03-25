using Common.GlobalFlow;
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
        
        private Squad Squad { get; set; }

        private void Start()
        {
            Squad = squadClass.GetSquad();
            coinsText.SetText(Squad.Coins.ToString());
            etherText.SetText(Squad.Ether.ToString());
            InitInventory();
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
            inventory.AddItem("Health Kit", 5);
            inventory.AddItem("Grenade", 3);
        }

        public void GoToNextScene()
        {
            squadClass.ForwardToBattle();
            SceneFlow.LoadScene(SceneFlow.EScene.Battle);
        }
    }
}