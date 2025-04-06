using Common;
using Unity.VisualScripting;
using UnityEngine;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "World", menuName = "Battle/World")]
    public class WorldSO : ScriptableObject, ILevelCollection
    {
        [SerializeField] private Level[] _levels;
        [SerializeField,Tooltip("Shouldn't be used for more than position and rotation")] private Level PreludePosition;
        private int _currentLevelIndex;
        public Level Current => _levels[_currentLevelIndex];
        public Level Precedent => _currentLevelIndex > 0 ? _levels[_currentLevelIndex - 1] : PreludePosition;

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