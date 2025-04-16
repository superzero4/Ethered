using UnityEngine;

namespace SquadSystem.Items
{
    [CreateAssetMenu(fileName = "ItemSO", menuName = "Items/ItemSO", order = 0)]
    public class ItemSO : ScriptableObject
    {
        public string Name
        {
            get => name;
            set => name = value;
        }

        public int CoinsPrice
        {
            get => coinsPrice;
            set => coinsPrice = value;
        }

        public int EtherPrice
        {
            get => etherPrice;
            set => etherPrice = value;
        }


        [SerializeField] private new string name;
        
        [SerializeField, Tooltip("The cost in coins to buy the item")]
        private int coinsPrice;
        [SerializeField, Tooltip("The cost in ether to buy the item")]
        private int etherPrice;
    }
}