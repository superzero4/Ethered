using BattleSystem;
using UnityEngine;

namespace Common.Tool
{
    public class ConsoleViewer : MonoBehaviour
    {
        [SerializeField] private BattleInfo _battleInfo;
        [SerializeField] private ConsoleSimulation _consoleSimulation;

        private void Start()
        {
            var battle = new Battle();
            battle.Init(_battleInfo,null);
            _consoleSimulation.StartCoroutine(_consoleSimulation.StartSimulation(battle));
        }
    }
}