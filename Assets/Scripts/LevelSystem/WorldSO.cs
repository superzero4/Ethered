using System.Linq;
using Common;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "World", menuName = "Battle/World")]
    public class WorldSO : ScriptableObject, ILevelCollection
    {
#if UNITY_EDITOR
        [Header("Editor only")] [SerializeField]
        private string[] _markerNames;

        [Button]
        public void SetPositionFromMarkerName()
        {
            var gos = GameObject.FindGameObjectsWithTag("EditorOnly");
            for (var i = 0; i < _markerNames.Length; i++)
            {
                string s = _markerNames[i];
                var t = gos.First(x => x.name == s).transform;
                _levels[i].Position = t.position;
                _levels[i].Rotation = t.rotation.eulerAngles;
            }
        }
#endif
        [SerializeField] private EncounterInfo _dynamicSquad;
        [SerializeField] private Level[] _levels;

        [SerializeField, Tooltip("Shouldn't be used for more than position and rotation")]
        private Level PreludePosition;

        private int _currentLevelIndex;
        public Level Current => _levels[_currentLevelIndex];
        public Level Precedent => _currentLevelIndex > 0 ? _levels[_currentLevelIndex - 1] : PreludePosition;

        public EncounterInfo StartingSquad => _dynamicSquad;

        public void Increment(int value, out bool reset)
        {
            _currentLevelIndex += value;
            _currentLevelIndex %= _levels.Length;
            reset = _currentLevelIndex == 0;
        }

        public void Reset()
        {
            _currentLevelIndex = 0;
        }

    }
}