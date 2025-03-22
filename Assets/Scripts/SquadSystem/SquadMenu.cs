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
        
        //public Squad Squad { get; set; }

        private void Awake()
        {
            //Squad = CreateSquad(2, new UnitInfo());
            coinsText.SetText(squadClass.Coins.ToString());
            etherText.SetText(squadClass.Ether.ToString());
            InitInventory();
        }
        
        // private Squad CreateSquad(int nbUnits, UnitInfo info)
        // {
        //     Squad squad = new()
        //     {
        //         Coins = 100,
        //         Ether = 100
        //     };
        //     squad.Init(nbUnits, info);
        //     return squad;
        // }
        
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
            SceneFlow.LoadScene(SceneFlow.EScene.Battle);
        }
    }
}