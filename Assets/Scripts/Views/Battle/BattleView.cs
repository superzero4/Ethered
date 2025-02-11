using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BattleSystem;
using Common;
using Common.Events;
using NaughtyAttributes;
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
        [SerializeReference] [ReadOnly] private SelectionState _selectionState;

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
            SetCallbacks();
        }

        private void OnHover(SelectionEventData selection)
        {
            _ui.UnitUI.SetUnit(selection.unit?.Info);
            _ui.TileUI.SetInfo(selection.environment.Info);
        }

        [SuppressMessage("ReSharper", "ConvertClosureToMethodGroup")]
        private void SetCallbacks()
        {
            _selector.OnSelectionUpdates.AddListener(s =>
            {
                if (_selectionState.CanSelectTarget)
                {
                    bool targetValid = _selectionState.AppendTarget(s.unit);
                    if (targetValid)
                    {
                        _ui.TargetUI.SetInfo(s.unit);
                        //TODO Probably maintain a List of targets and not just a single LastTargetUI
                    }
                    else
                    {
                        //TODO Show negative feedback showing target wasn't selected
                    }
                }
                else
                    _selectionState.SetUnit(s.unit, true);
            });
            foreach (var actionUI in _ui.UnitUI.Actions)
            {
                actionUI.OnClick.AddListener(action =>
                {
                    if (_selectionState.CanSelectAction)
                        _selectionState.SetAction(action);
                });
            }
            _ui.ActionButton.AddListener( () =>
            {
                var action = _selectionState.Confirm();
                var confirmed = _battle.ConfirmAction(action);
                _selectionState.Reset();
                if (confirmed)
                {
                    //TODO Show positive feedback
                }
                else
                {
                    //TODO Show cancel feedback
                }
            });
        }
    }
}