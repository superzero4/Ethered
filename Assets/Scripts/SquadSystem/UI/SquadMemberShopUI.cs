using TMPro;
using UnityEngine;

namespace SquadSystem.UI
{
    public class SquadMemberShopUI : MonoBehaviour
    {
        [Header("UI References")] 
        [SerializeField] private TMP_Text squadMemberName;
        [SerializeField] private TMP_Text squadMemberHealth;
        [SerializeField] private TMP_Text squadMemberArmor;
        [SerializeField] private TMP_Text squadMemberCoinsPrice;
        [SerializeField] private TMP_Text squadMemberEtherPrice;
        
        public void SetParameters(string objectName, int health, int armor, int coinsPrice, int etherPrice)
        {
            squadMemberName.text = objectName;
            squadMemberHealth.text = "Health: " + health;
            squadMemberArmor.text = "Armor: " + armor;
            squadMemberCoinsPrice.SetText("Cost: " + coinsPrice + " coins");
            squadMemberEtherPrice.SetText("Cost: " + etherPrice + " ether");
        }
    }
}