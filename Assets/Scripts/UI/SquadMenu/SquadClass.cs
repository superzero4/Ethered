using System.Collections.Generic;
using Common;
using UnitSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace SquadSystem
{
    public class Wallet : ScriptableObject
    {
        [SerializeField] private int _coins;
        [SerializeField] private int _ether;

        public int Coins
        {
            get => _coins;
            set => _coins = value;
        }

        public int Ether
        {
            get => _ether;
            set => _ether = value;
        }
    }

    public class SquadClass : MonoBehaviour
    {
        [SerializeField] private Squad _squad;
        [SerializeField] private EncounterInfo _battleInfo;

        [FormerlySerializedAs("_coins")] [SerializeField]
        private int _startCoins;

        [FormerlySerializedAs("_ether")] [SerializeField]
        private int _startEther;

        [SerializeField] private Inventory _inventory;

        public List<UnitInfo> Units => _squad.Units;
        public int SquadSize => Units.Count;

        [SerializeField] private UpgradeList upgrades;

        public int Coins
        {
            get => _squad.Coins;
            set => _squad.Coins = value;
        }

        public int Ether
        {
            get => _squad.Ether;
            set => _squad.Ether = value;
        }

        private void Awake()
        {
            _squad = _battleInfo.Units;
        }

        public void ForwardToBattle()
        {
            _battleInfo.Fill(_squad);
        }

        public void AddUnit(UnitInfo unitInfo)
        {
            Units.Add(unitInfo);
        }


        public Squad Squad
        {
            get => _squad;
            set => _squad = value;
        }
    }
}