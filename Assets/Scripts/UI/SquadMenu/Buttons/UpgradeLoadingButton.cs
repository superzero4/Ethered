using System.Collections.Generic;
using SquadSystem.UI;
using UnityEngine;

namespace SquadSystem.Buttons
{
    public class LoadUpgradesButton : MonoBehaviour
    {
        [Header("Editor References")]
        [SerializeField] private UpgradeList upgradeListReference; // Reference to the squad
        [SerializeField] private GameObject upgradeContainer; // The container that holds the bought upgrades (UI)
        [SerializeField] private GameObject upgradePrefab; // The prefab of the upgrade (UI)
        
        /// <summary>
        /// Load the upgrades of the squad when the player clicks on the button.
        /// It displays the upgrades in the UI.
        /// </summary>
        public void LoadUpgrades()
        {
            List<Upgrade> upgrades = upgradeListReference.GetUpgradeList();
            foreach (Upgrade upgrade in upgrades)
            {
                GameObject upgradeUI = Instantiate(upgradePrefab, upgradeContainer.transform);
                UpgradeUI upgradeUIComponent = upgradeUI.GetComponent<UpgradeUI>();
                upgradeUIComponent.SetParameters(
                    upgrade.upgradeReference.Name, 
                    upgrade.upgradeReference.UpgradeType.ToString(), 
                    upgrade.upgradeReference.CurrentTier, 
                    upgrade.upgradeReference.MaxTier
                    );
            }
        }
        
        /// <summary>
        /// Clear the upgrade container when the player clicks on the button.
        /// </summary>
        public void ClearUpgradeContainer()
        {
            foreach (Transform child in upgradeContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}