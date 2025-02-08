using BattleSystem;
using UnityEngine;

namespace Common.Tool
{
    public class ConsoleViewer : MonoBehaviour
    {
        [SerializeField] private BattleInfo _battleInfo;
        [SerializeField] private ConsoleSimulation _consoleSimulation;

        private void Awake()
        {
            var battle = new Battle();
            battle.Init(_battleInfo);
            _consoleSimulation.StartCoroutine(_consoleSimulation.StartSimulation(battle));
        }
    }
}