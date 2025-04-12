using TMPro;
using UnityEngine;

namespace SquadSystem.UI
{
    public class ItemShopUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text itemName;
        [SerializeField] private TMP_Text itemCoinsPrice;
        [SerializeField] private TMP_Text itemEtherPrice;
        
        public void SetParameters(string objectName, int coinsPrice, int etherPrice)
        {
            itemName.text = objectName;
            itemCoinsPrice.SetText("Cost: " + coinsPrice + " coins");
            itemEtherPrice.SetText("Cost: " + etherPrice + " ether");
        }
    }
}