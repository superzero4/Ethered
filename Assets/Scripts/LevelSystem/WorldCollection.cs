using System.Collections.Generic;
using Common;
using UnityEngine;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "worldCollection", menuName = "Battle/WorldCollection")]
    public class WorldCollection : ScriptableObject, ILevelCollection
    {
        [SerializeField] private WorldSO[] _worlds;
        private int _currentIndex = 0;
        private ILevelCollection currentLevelCollection => _worlds[_currentIndex];

        public Level Current => currentLevelCollection.Current;

        public Level Precedent => currentLevelCollection.Precedent;

        public EncounterInfo StartingSquad => currentLevelCollection.StartingSquad;

        public IEnumerable<WorldSO> Worlds => _worlds;

        public void Increment(int value, out bool reset)
        {
            reset = false;
            for (int i = 0; i < value; i++)
            {
                currentLevelCollection.Increment(1, out bool b);
                if (b)
                {
                    _currentIndex++;
                    _currentIndex %= _worlds.Length;
                    reset = _currentIndex == 0;
                }
            }
        }

        public void Reset()
        {
            _currentIndex = 0;
            foreach (var lv in _worlds)
            {
                lv.Reset();
            }
        }
    }
}