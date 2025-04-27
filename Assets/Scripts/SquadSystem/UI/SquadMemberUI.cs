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
        
        public void SetParameters(string name, int health, int armor)
        {
            squadMemberIndexText.text = name;
            healthText.text = "Health: " + health;
            armorText.text = "Armor: " + armor;
        }
    }
}