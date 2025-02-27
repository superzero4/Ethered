using SquadSystem.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeSO", menuName = "Upgrades/UpgradeSO")]
public class UpgradeSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private EUpgradeType _upgradeType;
    
    [SerializeField, Tooltip("The cost in coins to buy the upgrade")] 
    private int _coinsCost;
    [SerializeField, Tooltip("The cost in ether to buy the upgrade")] 
    private int _etherCost;
    
    [SerializeField] private int _currentTier;
    [SerializeField] private int _maxTier;
    
    public int CoinsCost => _coinsCost;
    public int EtherCost => _etherCost;
}
