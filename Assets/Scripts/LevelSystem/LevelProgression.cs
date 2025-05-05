//#define DEBUG_BUILD
using Common;
using SquadSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace LevelSystem
{
    public class LevelProgression : MonoBehaviour
    {
        [SerializeField, UnityEngine.Range(0, 100)]
        private int _levelSkip;

        [SerializeField] private ILevelCollection _levels;
        [SerializeField] private Object _levelsHolder;

        [Header("Progression")] [SerializeField]
        private EncounterInfo _initSquad;

        [SerializeField] private EncounterInfo _dynamicSquad;
        private static LevelProgression _instance;

        public ILevelCollection Levels => _levels;

        public EncounterInfo DynamicSquad => _dynamicSquad;

        public static LevelProgression Instance => _instance;

        public void Awake()
        {
            // Initialize the level collection
            Assert.IsTrue(_levelsHolder != null && _levelsHolder is ILevelCollection,
                " _levelsHolder is null or not of type ILevelCollection");
            _levels = _levelsHolder as ILevelCollection;
            if (_instance == null)
            {
#if UNITY_EDITOR
                if (_levelsHolder is WorldCollection worldCollection)
                    foreach (var world in worldCollection.Worlds)
                        world.SetPositionFromMarkerName();
#endif
                _levels.Reset();
                _dynamicSquad.Fill(new Squad(_initSquad.Units));
#if UNITY_EDITOR || DEBUG_BUILD
                _levels.Increment(_levelSkip);
#else
                _levels.Increment(0);
#endif

                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}