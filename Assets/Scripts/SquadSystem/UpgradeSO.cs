using SquadSystem.Enums;
using UnityEngine;

namespace SquadSystem
{
    [CreateAssetMenu(fileName = "UpgradeSO", menuName = "Upgrades/UpgradeSO")]
    public class UpgradeSO : ScriptableObject
    {
        [SerializeField] private new string name;
        [SerializeField] private EUpgradeType upgradeType;
    
        [SerializeField, Tooltip("The cost in coins to buy the upgrade")] 
        private int coinsCost;
        [SerializeField, Tooltip("The cost in ether to buy the upgrade")] 
        private int etherCost;
    
        [SerializeField] private int currentTier;
        [SerializeField] private int maxTier;
    
        public int CoinsCost => coinsCost;
        public int EtherCost => etherCost;
        
        public EUpgradeType UpgradeType => upgradeType;
        
        public int CurrentTier => currentTier;
        public int MaxTier => maxTier;
        
        public string Name => name;
    }
}
