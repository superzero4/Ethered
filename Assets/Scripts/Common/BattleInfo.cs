using System;
using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using NaughtyAttributes;
using SquadSystem;
using UnitSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Environment = BattleSystem.Environment;

namespace Common
{
    [CreateAssetMenu(fileName = "BattleInfo", menuName = "Battle/BattleInfo", order = 0)]
    public class BattleInfo : ScriptableObject
    {
        [SerializeField] private Vector2Int _size;
        [SerializeField] private Squad _enemies;
        [SerializeField] private Squad _squad;
        [SerializeField] private UnitInfo _defaultUnit;
        [SerializeField] private UnitInfo _defaultEnemy;
        [SerializeField] private EnvironmentInfo _defaultEnvironment;
        [SerializeField] private List<EnvironmentGroup> _specificEnvironments;

        [Serializable]
        private struct EnvironmentGroup
        {
            [SerializeField] public EnvironmentInfo environment;
            [SerializeField] public PositionData[] positions;
        }

        public Squad Squad => _squad;
        public Squad Enemies => _enemies;
        public EnvironmentInfo DefaultEnvironment => _defaultEnvironment;

        public IEnumerable<Environment> GetSpecificEnvironments()
        {
            return _specificEnvironments
                    .SelectMany<EnvironmentGroup, Environment>(ep => ep.positions
                    .Select<PositionData,Environment>(p =>  new Environment(ep.environment, p)));
        }

        public Vector2Int Size => _size;

        [Button]
        private void CreateDefaultSquat()
        {
            _squad.Init(5, _defaultUnit);
        }

        [Button]
        private void CreateDefaultEnemys()
        {
            _enemies.Init(5, _defaultEnemy);
        }

        [Button]
        private void FillRandomNames()
        {
            string[] names = new string[]
            {
                "John", "Doe", "Jane", "Smith", "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Heidi",
                "Ivan", "Judy", "Kevin", "Linda", "Mallory", "Oscar", "Peggy", "Romeo", "Trent", "Ursula", "Victor",
                "Walter", "Xander", "Yvonne", "Zelda"
            };
            foreach (var unit in _squad.Units.Concat(_enemies.Units))
            {
                var info = unit.VisualInformations;
                info.Name = names[UnityEngine.Random.Range(0, names.Length)] + " " +
                            names[UnityEngine.Random.Range(0, names.Length)];
                unit.VisualInformations = info;
            }
        }

        [Button]
        private void SetAllWhite()
        {
            SetColors(Color.white);
        }

        private void SetColors(Color color)
        {
            foreach (var unit in _squad.Units.Concat(_enemies.Units))
            {
                var info = unit.VisualInformations;
                info.Color = color;
                unit.VisualInformations = info;
            }
        }
    }
}