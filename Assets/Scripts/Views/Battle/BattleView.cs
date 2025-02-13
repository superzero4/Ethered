using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BattleSystem;
using Common;
using Common.Events;
using NaughtyAttributes;
using UI.Battle;
using UnitSystem.Actions.Bases;
using UnityEngine;
using UnityEngine.Events;
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

            _selectionState = new SelectionState();
            _ui.Initialize();
            _selector.Initialize();
            ReactOnHover(true);
            //_selector.SelectionUpdated.AddListener(s => Debug.Log("Selected: " + s.unit));
            SetCallbacks();
        }

        private void ReactOnHover(bool val)
        {
            if (val)
            {
                _selector.OnHoverChanges.AddListener(OnHover);
                //We force the raise to get back the currecnt selection when we start to listining to it basically, relistening to the last hover message we possibly missed
                _selector.RaiseCurrentHover();
            }
            else
                _selector.OnHoverChanges.RemoveListener(OnHover);
        }

        private void OnHover(SelectionEventData selection)
        {
            _ui.UnitUI.SetUnit(selection.unit?.Info);
            _ui.TileUI.SetInfo(selection.environment.Info);
        }

        [SuppressMessage("ReSharper", "ConvertClosureToMethodGroup")]
        private void SetCallbacks()
        {
            _selector.Reseted.AddListener(e => _selectionState.Reset());
            _selector.Reseted.AddListener(e => ReactOnHover(true));
            _selector.Reseted.AddListener(e =>
            {
                _selector.Hints.Clear();
                _selector.RaiseCurrentHover();
            });
            _selector.SelectionUpdated.AddListener(OnSelected);
            foreach (var actionUI in _ui.UnitUI.Actions)
            {
                _selector.Reseted.AddListener(e => actionUI.Reset());
                actionUI.OnClick.AddListener(_selectionState.SelectActionIfValid);
                actionUI.OnClick.AddListener(e => Debug.LogWarning(" SELECTION Action selected: " + e));
            }

            _ui.ConfirmButton.AddListener(OnConfirmed);
            _ui.ConfirmButton.AddListener(_selector.Hints.Clear);
        }

        private void OnConfirmed()
        {
            var action = _selectionState.Confirm();
            var confirmed = _battle.ConfirmAction(action);
            _selectionState.Reset();
            if (confirmed)
            {
                //TODO Show positive feedback
                //Timeline UI should have subscribed to timeline events and be update on it's own
            }
            else
            {
                //TODO Show cancel feedback
            }
        }


        private void OnSelected(SelectionEventData s)
        {
            if (_selectionState.CanSelectTarget)
            {
                bool targetValid = _selectionState.AppendTarget(s.unit);
                if (targetValid)
                {
                    _ui.TargetUI.SetInfo(s.unit);
                    _selector.Hints.Lock();
                    _selector.RaiseCurrentHover();
                    //TODO Probably maintain a List of targets and not just a single LastTargetUI
                }
                else
                {
                    //TODO Show negative feedback showing target wasn't selected
                }
            }
            else if (_selectionState.CanSelectUnit)
            {
                if (s.unit != null)
                {
                    //In theory, already set by the hover event so redundant but as a safe
                    _ui.UnitUI.SetUnit(s.unit.Info);
                    _selectionState.SetUnit(s.unit, true);
                    ReactOnHover(false);
                    _selector.Hints.Lock();
                }
            }
        }
    }
}