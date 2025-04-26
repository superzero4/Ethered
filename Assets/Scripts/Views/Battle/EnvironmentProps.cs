using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Views.Phase;

namespace Views.Battle
{
    public class EnvironmentProps : MonoBehaviour
    {
        [Serializable]
        public struct Prop
        {
            [SerializeField] private ScalingPhaseView _prefab;
            [SerializeField] private Vector2Int _size;

            public ScalingPhaseView Prefab => _prefab;

            public Vector2Int Size => _size;
        }

        [SerializeField] private Prop[] _props;
        private Dictionary<Vector2Int, ScalingPhaseView> _prefabs;

        public ScalingPhaseView this[Vector2Int index]
        {
            get
            {
                _prefabs ??= _props.ToDictionary(x => x.Size, x => x.Prefab);
                return _prefabs[new Vector2Int(Math.Max(index.x, index.y), Math.Min(index.x, index.y))];
            }
        }
    }
}