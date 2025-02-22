using UnityEngine;

namespace SquadSystem
{
    public class SquadMenu : MonoBehaviour
    {
        public Squad Squad { get; set; }
        public UpgradeList UpgradeList { get; set; }

        private void Awake()
        {
            Squad = new Squad();
            UpgradeList = gameObject.AddComponent<UpgradeList>();
        }
    }
}