using System;
using Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelSystem
{
    [Serializable]
    public struct Level
    {
        [SerializeField] private BattleInfo _battle;
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _rotation;
        
        public BattleInfo Battle => _battle;

        public Vector3 Position
        {
            get => _position;
            set => _position = value;
        }

        public Vector3 Rotation => _rotation;
    }
}