using System;
using Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelSystem
{
    [Serializable]
    public struct Level
    {
        [SerializeField] private EncounterInfo _battle;
        [SerializeField] private MapInfo _map;
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _rotation;
        
        public Vector3 Position
        {
            get => _position;
            set => _position = value;
        }

        public Vector3 Rotation => _rotation;

        public EncounterInfo Battle => _battle;

        public MapInfo Map => _map;
    }
}