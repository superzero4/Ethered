using Common;
using UnityEngine;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "World", menuName = "Battle/World")]
    public class WorldSO : ScriptableObject, ILevelCollection
    {
        [SerializeField] private Level[] _levels;
        private int _currentLevelIndex;
        public Level Current => _levels[_currentLevelIndex];

        public void Increment(int value = 1)
        {
            _currentLevelIndex += value;
            _currentLevelIndex %= _levels.Length;
        }

        public void Reset()
        {
            _currentLevelIndex = 0;
        }
    }
}