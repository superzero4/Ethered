using UnityEngine;

namespace Common.Tool
{
    public class BattleViewStarter : MonoBehaviour
    {
        [SerializeField] private BattleView _battleView;
        [SerializeField] private ConsoleSimulation _consoleSimulation;

        private void Start()
        {
            _consoleSimulation.StartCoroutine(_consoleSimulation.StartSimulation(_battleView.Battle));
        }
    }
}