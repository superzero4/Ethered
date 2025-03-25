using TMPro;
using UnityEngine;

namespace SquadSystem.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text itemNameText;
        [SerializeField] private TMP_Text quantityText;
        
        public void SetParameters(string itemName, int quantity)
        {
            itemNameText.text = itemName;
            quantityText.text = "Quantity: " + quantity;
        }
    }
}