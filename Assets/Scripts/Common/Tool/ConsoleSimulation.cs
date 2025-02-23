using System.Collections;
using System.Linq;
using BattleSystem;
using BattleSystem.Actions;
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
                    if (QueueRandomAction(battle, unit))
                    {
                        if (_logStatusToConsole)
                            Debug.Log("Action1 successful, trying action 2");
                        QueueRandomAction(battle, unit);
                    }
                }

                yield return battle.NextTurn(-1f);
                yield return new WaitForSeconds(0.01f);
            }
        }

        public static bool QueueRandomAction(Battle battle, Unit unit)
        {
            if (battle.Tiles.TryGetRandomValidAction(unit,out Action action))
            {
                if (battle.ConfirmAction(action))
                {
                    return true;
                }
            }
            return false;
        }
    }
}