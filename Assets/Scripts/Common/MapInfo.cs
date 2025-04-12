using System;
using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using UnityEngine;
using Environment = BattleSystem.Environment;

namespace Common
{
    [CreateAssetMenu(fileName = "Map", menuName = "Battle", order = 1)]
    public class MapInfo : ScriptableObject
    {
        [SerializeField] private Vector2Int _size;
        [SerializeField] private EnvironmentInfo _defaultEnvironment;
        [SerializeField] private List<EnvironmentGroup> _specificEnvironments;
        
        [Serializable]
        private struct EnvironmentGroup
        {
            [SerializeField] public EnvironmentInfo environment;
            [SerializeField] public PositionData[] positions;
        }
        public EnvironmentInfo DefaultEnvironment => _defaultEnvironment;

        public IEnumerable<Environment> GetSpecificEnvironments()
        {
            return _specificEnvironments
                .SelectMany<EnvironmentGroup, Environment>(ep => ep.positions
                    .Select<PositionData,Environment>(p =>  new Environment(ep.environment, p)));
        }
        public Vector2Int Size => _size;
    }
}