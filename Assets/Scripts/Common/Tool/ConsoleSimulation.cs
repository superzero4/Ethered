using System.Collections;
using System.Linq;
using BattleSystem;
using BattleSystem.Actions;
using UnitSystem;
using UnitSystem.Actions.Bases;
using UnityEngine;
using Action = BattleSystem.Action;
using Random = UnityEngine.Random;
using NaughtyAttributes;
using UnitSystem.AI;

namespace Common.Tool
{
    public class ConsoleSimulation : MonoBehaviour
    {
        [SerializeField, Range(-1, 10)] private float _delay = 0.01f;
        [SerializeField] private bool _logStatusToConsole = false;

        [SerializeField] private PriorityComparer _comp;
        private IBrain _brain;

        //[SerializeField] private ActionInfoBaseSO[] _actionsToTest;
        //[SerializeField] private ActionInfoBaseSO[] _actionsToTest2;
        private Battle _battle;

        public IEnumerator StartSimulation(Battle battle)
        {
         _brain = new UtilityBasedBrain(_comp);
            _battle = battle;
            while (true)
            {
                //To reset timeline in beetween every round, other way action are stacked in and repeated
                //_actionsToTest2 = new[] { _actionsToTest2[0] };
                if (_logStatusToConsole)
                    LogBattle();
                var units = battle.Units;
                var allies = units.Take(2);
                var enemies = units.TakeLast(2);
                foreach (var unit in allies.Concat(enemies))
                {
                    if (QueuBrainAction(battle, unit))
                    {
                        if (_logStatusToConsole)
                            Debug.Log("Action1 successful, trying action 2");
                        QueuBrainAction(battle, unit);
                    }
                }

                battle.Step();
                yield return new WaitForSeconds(_delay);
            }
        }

        [Button]
        public void LogBattle()
        {
            Debug.LogWarning(_battle.ToString());
        }

        public bool QueuBrainAction(Battle battle, Unit unit)
        {
            var action = _brain.GetDecision(unit, battle.Tiles);
            if (action != null && battle.ConfirmAction(action))
            {
                return true;
            }

            return false;
        }
    }
}