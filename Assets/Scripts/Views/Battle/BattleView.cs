using System.Linq;
using BattleSystem;
using Common;
using Common.Events;
using UI.Battle;
using UnityEngine;
using UnityEngine.Serialization;
using Views.Battle.Selection;
using Battle = BattleSystem.Battle;

namespace Views.Battle
{
    public class BattleView : MonoBehaviour
    {
        [SerializeField] private BattleInfo _battleInfo;
        [SerializeField] private UnitView _unitViewPrefab;
        [SerializeField] private EnvironmentView _environmentViewPrefab;
        [SerializeField] private Grid _grid;
        [SerializeField] private BattleUI _ui;
        [SerializeField] private Selector _selector;
        private BattleSystem.Battle _battle;

        public BattleSystem.Battle Battle
        {
            get => _battle;
        }

        private void Awake()
        {
            _battle = new BattleSystem.Battle();
            _battle.Init(_battleInfo);
            foreach (var unit in _battle.Units)
            {
                var unitView = Instantiate(_unitViewPrefab, transform);
                unitView.Init(unit, _grid);
            }

            foreach (var t in _battle.Tiles.TilesFlat)
            {
                EnvironmentView env = Instantiate(_environmentViewPrefab, transform);
                env.Init(t.Base, _grid);
                env.SetTile(t);
                PhaseSelector.SetLayer(env);
                env.gameObject.name = "Tile " + t.Base.Position.ToString();
            }

            _ui.Initialize();
            _selector.Initialize();
            _selector.OnHoverChanges.AddListener(OnHover);
            _selector.OnSelectionUpdates.AddListener(s => Debug.Log("Selected: " + s.unit));
        }

        private void OnHover(SelectionEventData selection)
        {
            _ui.UnitUI.SetUnit(selection.unit?.Info);
            _ui.TileUI.SetInfo(selection.environment.Info);
        }
        
    }
}