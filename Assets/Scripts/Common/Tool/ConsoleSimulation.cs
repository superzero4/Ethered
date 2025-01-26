using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

namespace Common.Events.Tool
{
    public class ConsoleSimulation : MonoBehaviour
    {
        [SerializeField] private BattleInfo _battleInfo;
        IEnumerator Start()
        {
            var battle = new Battle();
            battle.Init(_battleInfo);
            while (true)
            {
                Debug.Log(battle.ToString());
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void Update()
        {
            
        }

    }
}