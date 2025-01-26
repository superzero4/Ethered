using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using UnitSystem;
using UnitSystem.Actions.Bases;
using UnityEngine;
using Action = BattleSystem.Action;
using Random = UnityEngine.Random;

namespace Common.Events.Tool
{
    public class ConsoleSimulation : MonoBehaviour
    {
        [SerializeField] private BattleInfo _battleInfo;
        [SerializeField] private ActionInfoBaseSO[] _actionsToTest;
        IEnumerator Start()
        {
            var battle = new Battle();
            battle.Init(_battleInfo);
            while (true)
            {
                Debug.Log(battle.ToString());
                var units = battle.Units;
                var allies = units.Take(2);
                var enemies = units.TakeLast(2);
                foreach (var unit in allies.Concat(enemies))
                {
                    var ph = Random.Range(1, 3);
                    var posX = Random.Range(0, _battleInfo.Size.x);
                    var posY = Random.Range(1, _battleInfo.Size.y - 1);
                    var pos = new PositionData(ph, posX, posY);
                    var tile = battle.Tiles[pos].ToList();
                    Action action = new Action(unit,_actionsToTest[0]);
                    if(action.TrySetTarget(unit, tile[0].Base))
                    {
                        battle.ConfirmAction(action);
                    }
                    battle.ConfirmAction(action);
                }
                yield return battle.Timeline.Execute();
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void Update()
        {
            
        }

    }
}