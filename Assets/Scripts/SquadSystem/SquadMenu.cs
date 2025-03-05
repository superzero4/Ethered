using TMPro;
using UnitSystem;
using UnityEngine;

namespace SquadSystem
{
    public class SquadMenu : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private TMP_Text _etherText;
        [SerializeField] private GameObject _shopPanel;
        [SerializeField] private GameObject _upgradePanel;
        
        public Squad Squad { get; set; }
        public UpgradeList UpgradeList { get; set; }

        private void Awake()
        {
            Squad = CreateSquad(2, new UnitInfo());
            UpgradeList = gameObject.AddComponent<UpgradeList>();
            _coinsText.SetText(Squad.Coins.ToString());
            _etherText.SetText(Squad.Ether.ToString());
        }
        
        private Squad CreateSquad(int nbUnits, UnitInfo info)
        {
            Squad squad = new()
            {
                Coins = 100,
                Ether = 100
            };
            squad.Init(nbUnits, info);
            return squad;
        }
        
        /// <summary>
        /// Update the coins of the squad and the UI
        /// </summary>
        /// <param name="value"></param>
        public void UpdateCoins(int value)
        {
            if (value < 0 && Squad.Coins + value < 0)
            {
                // TODO : display a message to the player that he doesn't have enough coins
                return;
            }
            
            var squad = Squad;
            squad.Coins += value;
            Squad = squad;
            
            _coinsText.SetText(Squad.Coins.ToString());
        }
        
        /// <summary>
        /// Update the ether of the squad and the UI
        /// </summary>
        /// <param name="value"></param>
        public void UpdateEther(int value)
        {
            if (value < 0 && Squad.Ether + value < 0)
            {
                // TODO : display a message to the player that he doesn't have enough ether
                return;
            }
            
            var squad = Squad;
            squad.Ether += value;
            Squad = squad;
            
            _etherText.SetText(Squad.Ether.ToString());
        }
    }
}