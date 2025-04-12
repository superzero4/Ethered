using BattleSystem;
using LevelSystem;
using UnityEngine;

namespace Common.Tool
{
    public class ConsoleViewer : MonoBehaviour
    {
        [SerializeField] private WorldSO _battleInfo;
        [SerializeField] private ConsoleSimulation _consoleSimulation;

        private void Start()
        {
            var battle = new Battle();
            var level = _battleInfo.Current;
            battle.Init(level.Battle, level.Map, _battleInfo.DynamicSquad, new EnvironmentInfo());
            _consoleSimulation.StartCoroutine(_consoleSimulation.StartSimulation(battle));
        }
    }
}