using TMPro;
using UnityEngine;

namespace SquadSystem.UI
{
    public class SquadMemberUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text squadMemberIndexText;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text armorText;
        
        public void SetParameters(int index, int health, int armor)
        {
            squadMemberIndexText.text = "Squad Member " + index;
            healthText.text = "Health: " + health;
            armorText.text = "Armor: " + armor;
        }
    }
}