using BattleSystem.TileSystem;
using Common.Events;
using Common.Events.UserInteraction;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

namespace Views.Battle.Selection
{
    public class Selectable : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private EnvironmentView _env;
        [SerializeField]
        private Transform _hintAnchor;

        public Tile Tile => _env.Tile;
        public SelectionEventData Selection => new(Tile.Base, Tile.Unit);

        public Transform HintAnchor => _hintAnchor;

        private void Awake()
        {
            Assert.IsTrue(_collider != null && _collider.isTrigger);
        }

        private void Start()
        {
            Assert.IsTrue(_env != null);
            Assert.IsTrue(gameObject.layer == PhaseSelector.SelectableLayer);
        }
        
    }
}