using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BattleSystem;
using Common;
using Common.Events;
using Common.Visuals;
using NaughtyAttributes;
using UI;
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
            //We set callbacks before initializing the _selector because we basically hook on selectione events and we want everything to be set as the selector initializes
            SetCallbacks();
            _selector.Initialize();
            _ui.ConfirmButton.AddListener(OnConfirmed);
            _ui.ConfirmButton.AddListener(_selector.Hints.Clear);
            //_selector.SelectionUpdated.AddListener(s => Debug.Log("Selected: " + s.unit));
        }

        private void OnHover(SelectionEventData selection)
        {
            if (_selectionState.CanSelectTarget)
            {
                _ui.TargetUI.SetInfo(selection.unit?.VisualInformations ?? VisualInformations.Default);
            }
            else if (_selectionState.CanSelectUnit)
            {
                _ui.UnitUI.SetUnit(selection.unit);
            }

            _ui.TileUI.SetInfo(selection.environment.Info);
        }

        [SuppressMessage("ReSharper", "ConvertClosureToMethodGroup")]
        private void SetCallbacks()
        {
            _battle.OnTimelineAction.AddListener(_ui.TimelineUI1.OnTimelineMemberInserted);

            _selector.Phase.OnSelectedPhaseChanges.AddListener(_ui.PhaseUI.DisplayPhase);

            _selector.OnHoverChanged.AddListener(OnHover);
            _selector.AddResetableElement(_selectionState);
            _selector.SelectionUpdated.AddListener(UpdateSelection);
            foreach (var actionUI in _ui.UnitUI.ActionUIs)
            {
                _selector.AddResetableElement(actionUI);
                UnityEvent<IActionInfo> onClick = actionUI.OnClick;
                onClick.AddListener(a =>
                {
                    _ui.UnitUI.ResetActionUIs(actionUI);
                    _selectionState.SelectionActionIfValid(a);
                });
                onClick.AddListener(e => _selector.UpdateHint = true);
                //onClick.AddListener(e => Debug.LogWarning(" SELECTION Action selected: " + e));
            }
        }

        private void OnConfirmed()
        {
            var action = _selectionState.Confirm();
            var confirmed = _battle.ConfirmAction(action);
            _selector.Reset();
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


        private void UpdateSelection(SelectionEventData s)
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
                    //Debug.LogWarning("SELECTION Target not valid");
                    //TODO Show negative feedback showing target wasn't selected
                }
            }
            else if (_selectionState.CanSelectUnit)
            {
                if (s.unit != null)
                {
                    //In theory, already set by the hover event so redundant but as a safe
                    _ui.UnitUI.SetUnit(s.unit);
                    _selectionState.SetUnit(s.unit, true);
                    _selector.UpdateHint = false;
                    _selector.Hints.Lock();
                }
            }
        }
    }
}