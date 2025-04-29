using System.ComponentModel;
using BattleSystem.TileSystem;
using Common.Events;
using Common.Events.UserInteraction;
using Common.Events.UserInterface;
using UnityEngine.Assertions;
using UnityEngine;
using NaughtyAttributes;
namespace Views.Battle.Selection
{
    public class Selectable : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private EnvironmentView _env;
        [SerializeField] private Transform _hintAnchor;

        [SerializeField] private SelectionHint _hint;
        [SerializeField, NaughtyAttributes.ReadOnly] private Selectable _other;

        public Tile Tile => _env.Tile;
        public SelectionEventData Selection => new(Tile.Base, Tile.Unit);

        public Transform HintAnchor => _hintAnchor;

        public SelectionHint Hint => _hint;
        public int Level
        {
            set => _hint.Level = value;
        }
        public Selectable Other
        {
            get { return _other; }
            set { _other = value; }
        }

        private void Awake()
        {
            Assert.IsTrue(_collider != null && _collider.isTrigger);
        }

        private void Start()
        {
            Assert.IsTrue(_env != null);
            Assert.IsTrue(gameObject.layer == Selector.SelectableLayer);
        }

        public void OnPhaseChanged(PhaseEventData data)
        {
        }
    }
}