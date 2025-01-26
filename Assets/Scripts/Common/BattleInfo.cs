using System;
using System.Collections.Generic;
using BattleSystem;
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
        [SerializeField] private Environment _defaultEnvironment;
        [SerializeField] private List<Environment> _specificEnvironments;
        public Squad Squad => _squad;
        public Squad Enemies => _enemies;
        public Environment DefaultEnvironment => _defaultEnvironment;
        public List<Environment> SpecificEnvironments => _specificEnvironments;
        
        public Vector2Int Size => _size;

        private void OnValidate()
        {
            if (_squad.Units == null || _squad.Units.Count == 0)
            {
                _squad.Init(5, null);
            }
            if (_enemies.Units == null || _enemies.Units.Count == 0)
            {
                _enemies.Init(5, null);
            }
        }
    }
}