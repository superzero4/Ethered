using Common.GlobalFlow;
using SquadSystem.Buttons;
using SquadSystem.Items;
using TMPro;
using UnitSystem;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private LoadSquadButton loadSquadButton;
        [SerializeField] private Inventory inventory;
        [SerializeField] private Shop shop;
        
        private Squad Squad { get; set; }

        private void Awake()
        {
            squadClass.LoadSquad();
            Squad = squadClass.Squad;
            coinsText.SetText(Squad.Coins.ToString());
            etherText.SetText(Squad.Ether.ToString());
            shop.GenerateSquadMemberShopWithTheList(6);
            squadPanel.SetActive(true);
            loadSquadButton.GetComponent<Button>().onClick.Invoke();
            //shop.GenerateGlobalUpgradesShopWithTheList(2);
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

        public void GoToNextScene() 
        {
            squadClass.ForwardToBattle();
            SceneFlow.LoadScene(SceneFlow.EScene.Battle);
        }
    }
}