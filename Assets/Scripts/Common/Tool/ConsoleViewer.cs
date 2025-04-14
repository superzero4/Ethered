using BattleSystem;
using LevelSystem;
using SquadSystem;
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
            battle.Init(level.Battle, level.Map, new Squad(_battleInfo.StartingSquad.Units), new EnvironmentInfo(), new EnvironmentInfo());
            _consoleSimulation.StartCoroutine(_consoleSimulation.StartSimulation(battle));
        }
    }
}