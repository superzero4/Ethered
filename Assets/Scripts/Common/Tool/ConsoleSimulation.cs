using System.Collections;
using System.Linq;
using BattleSystem;
using UnitSystem;
using UnitSystem.Actions.Bases;
using UnityEngine;
using Action = BattleSystem.Action;
using Random = UnityEngine.Random;
namespace Common.Tool
{
    public class ConsoleSimulation : MonoBehaviour
    {
        [SerializeField] private bool _logStatusToConsole = false;
        [SerializeField] private ActionInfoBaseSO[] _actionsToTest;
        [SerializeField] private ActionInfoBaseSO[] _actionsToTest2;

        public IEnumerator StartSimulation(Battle battle)
        {
            while (true)
            {
                //To reset timeline in beetween every round, other way action are stacked in and repeated
                //_actionsToTest2 = new[] { _actionsToTest2[0] };
                if (_logStatusToConsole)
                    Debug.LogWarning(battle.ToString());
                var units = battle.Units;
                var allies = units.Take(2);
                var enemies = units.TakeLast(2);
                foreach (var unit in allies.Concat(enemies))
                {
                    if (QueueAction(battle, unit, _actionsToTest, true))
                    {
                        if (_logStatusToConsole)
                            Debug.Log("Action1 successful, trying action 2");
                        QueueAction(battle, unit, _actionsToTest2, false);
                    }
                }

                yield return battle.TurnEnd(true);
                yield return new WaitForSeconds(0.01f);
            }
        }


        private bool QueueAction(Battle battle, Unit unit, ActionInfoBaseSO[] actionA, bool targetBase)
        {
            var Size = battle.Tiles.Size;
            IBattleElement target;
            if (targetBase)
            {
                var ph = Random.Range(1, 3);
                var posX = Random.Range(0, Size.x);
                var posY = Random.Range(0, Size.y);
                var pos = new PositionData(ph, posX, posY);
                var tile = battle.Tiles[pos].ToList();
                target = tile[0].Base;
            }
            else
            {
                target = battle.Units.ToList()[Random.Range(0, battle.Units.Count())];
            }

            var pickedAction = actionA[Random.Range(0, actionA.Length)];
            Action action = new Action(unit, pickedAction);
            var result = action.TrySetTarget(unit, target);
            if (!targetBase)
                if (_logStatusToConsole)
                    Debug.Log($"{result} => {unit.Position} is trying to use {pickedAction.name} on {target.Position}");
            if (result)
            {
                if (!battle.ConfirmAction(action))
                    if (_logStatusToConsole)
                        Debug.Log("But couldn't execute on current map");
            }

            return result;
        }

        private void Update()
        {
        }
    }
}