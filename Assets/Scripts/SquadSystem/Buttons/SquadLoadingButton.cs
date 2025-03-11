using SquadSystem.UI;
using UnityEngine;

namespace SquadSystem.Buttons
{
    public class SquadLoadingButton : MonoBehaviour
    {
        [SerializeField] private SquadClass squadReference; // Reference to the squad
        [SerializeField] private GameObject squadContainer; // The container that holds the squad members (UI)
        [SerializeField] private GameObject squadMemberPrefab; // The prefab of the squad member (UI)
        
        /// <summary>
        /// Load the squad members of the squad when the player clicks on the button.
        /// It displays the squad members in the UI.
        /// </summary>
        public void LoadSquad()
        {
            foreach (var unit in squadReference.Units)
            {
                int index = squadReference.Units.IndexOf(unit);
                GameObject squadMember = Instantiate(squadMemberPrefab, squadContainer.transform);
                SquadMemberUI squadMemberUI = squadMember.GetComponent<SquadMemberUI>();
                squadMemberUI.SetParameters(index, unit.MaxHealth, unit.Armor);
            }
        }
        
        /// <summary>
        /// Clear the squad container when the player clicks on the button.
        /// </summary>
        public void ClearSquadContainer()
        {
            foreach (Transform child in squadContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}