using TMPro;
using UnityEngine;

namespace SquadSystem.UI
{
    /// <summary>
    /// This class contains all the data to display in the Upgrade part of the Squad menu.
    /// </summary>
    public class UpgradeUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text upgradeNameText;
        [SerializeField] private TMP_Text upgradeTypeText;
        [SerializeField] private TMP_Text upgradeTierText;
        
        public void SetParameters(string upgradeName, string upgradeType, int currentTier, int maxTier)
        {
            upgradeNameText.text = upgradeName;
            upgradeTypeText.text = "Type: " + upgradeType;
            upgradeTierText.text = "Tier: " + currentTier + "/" + maxTier;
        }
    }
}