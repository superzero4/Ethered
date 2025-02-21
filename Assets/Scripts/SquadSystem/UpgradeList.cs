using System.Collections.Generic;
using UnityEngine;

namespace SquadSystem
{
    public class UpgradeList : Upgrade
    {
        /*
         * List of all upgrades currently available in the shop
         * TODO : initialize the list with all tier 1 upgrades
         */
        public List<Upgrade> AllUpgrades { get; set; } = new();

        /*
         * Add an upgrade to the list of all upgrades
         */
        public void AddUpgrade(Upgrade upgrade)
        {
            AllUpgrades.Add(upgrade);
        }
            
        /*
         * Remove an upgrade from the list of all upgrades
         */
        public void RemoveUpgrade(Upgrade upgrade)
        {
            AllUpgrades.Remove(upgrade);
        }
        
        /*
         * Return a list of all available upgrades for a squad
         * TODO : modify the method to return only the upgrades that the squad can afford, maybe different methods based on the price type
         */
        public List<Upgrade> AvailableUpgrades(Squad squad)
        {
            List<Upgrade> availableUpgrades = new List<Upgrade>();
            foreach (var upgrade in AllUpgrades)
            {
                availableUpgrades.Add(upgrade);
            }
            return availableUpgrades;
        }
        
        /*
         * Return a list of all upgrades that the squad has already bought
         */
        public List<Upgrade> BoughtUpgrades(Squad squad)
        {
            List<Upgrade> boughtUpgrades = new();
            foreach (var upgrade in AllUpgrades)
            {
                if (squad.Upgrades.Contains(upgrade))
                {
                    boughtUpgrades.Add(upgrade);
                }
            }
            return boughtUpgrades;
        }
        
        /*
         * Buy an upgrade for a squad if it can afford it and add it to the list of bought upgrades
         */
        public void BuyUpgrade(Squad squad, Upgrade upgrade)
        {
            int coinsCost = upgrade.GetUpgradeCoinsCost();
            int etherCost = upgrade.GetUpgradeEtherCost();
            
            if (squad.Coins >= coinsCost && squad.Ether >= etherCost)
            {
                squad.Coins -= coinsCost;
                squad.Ether -= etherCost;
                squad.Upgrades.Add(upgrade);
                // TODO : update the AllUpgrades list with the next tier of the upgrade if it exists
            }
            else
            {
                Debug.Log("Not enough coins or ether to buy the upgrade"); // TODO : replace with a popup
            }
        }
    }
}