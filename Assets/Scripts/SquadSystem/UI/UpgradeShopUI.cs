using TMPro;
using UnityEngine;

namespace SquadSystem.UI
{
    public class UpgradeShopUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text upgradeName;
        [SerializeField] private TMP_Text upgradeCoinsPrice;
        [SerializeField] private TMP_Text upgradeEtherPrice;

        public void SetParameters(string objectName, int coinsPrice, int etherPrice)
        {
            upgradeName.text = objectName;
            upgradeCoinsPrice.SetText("Cost: " + coinsPrice + " coins");
            upgradeEtherPrice.SetText("Cost: " + etherPrice + " ether");
        }
    }
}