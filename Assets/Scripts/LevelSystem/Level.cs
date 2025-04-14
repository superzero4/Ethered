using System;
using System.Linq;
using Common;
using NaughtyAttributes;
using UnitSystem.Actions.Bases;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelSystem
{
    [Serializable]
    public struct Level
    {
        [SerializeField,Tooltip("Leave empty for no override and use the current squad status")] private ActionInfoBaseSO[] _playerActionsOverride;
        [SerializeField] private EncounterInfo _battle;
        [SerializeField,Header("Map layout")] private MapInfo _map;
        [FormerlySerializedAs("_spawnPrefabs")] [SerializeField,Tooltip("If not, tiles will have the status defined but no additional prefab will be spawned/displayed on it, it will rely on the already existing world for the player to know")] private bool _showTileModels;
        [Header("Placement")]
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _rotation;
        public Vector3 Position
        {
            get => _position;
            set => _position = value;
        }

        public Vector3 Rotation
        {
            set => _rotation = value;
            get { return _rotation; }
        }

        public EncounterInfo Battle => _battle;

        public MapInfo Map => _map;

        public bool ShowTileModels => _showTileModels;

        public ActionInfoBaseSO[] PlayerActionsOverride => _playerActionsOverride;
    }
}