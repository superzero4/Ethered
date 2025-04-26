using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BattleSystem;
using Common;
using Common.Events;
using Common.Events.UserInteraction;
using Common.GlobalFlow;
using Common.Visuals;
using NaughtyAttributes;
using UI;
using UI.Battle;
using UnitSystem;
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
        [Header("Settings")] [SerializeField, Range(0, 4f)]
        private float _delay = 0.5f;

        [SerializeField] private bool _unitActionsPreviewShowEmptyTiles = true;
        [SerializeField] private bool _allowActionChangeAfterUnitSelected = true;

        [Header("References")] [SerializeField]
        private TimelineView _timelineView;


        [Header("Read Only")] [SerializeReference] [ReadOnly]
        private SelectionState _selectionState;

        private UserInput _userInput;
        [SerializeField] [ReadOnly] private Selector _selector;
        [SerializeField] [ReadOnly] private PhaseSelector _phaseSelector;
        [SerializeReference] [ReadOnly] private BattleSystem.Battle _battle;
        [SerializeReference] [ReadOnly] private BattleUI _ui;


        public BattleSystem.Battle Battle
        {
            get => _battle;
        }

        private IHints _hints;

        public void Init(BattleSystem.Battle battle, Selector selector, PhaseSelector phase,
            UserInput userInput, BattleUI ui, IHints hints, IHints timelineHints)
        {
            _ui = ui;
            _hints = hints;
            _phaseSelector = phase;
            _userInput = userInput;
            _selector = selector;
            _battle = battle;
            //Creation
            _selectionState = new SelectionState(_allowActionChangeAfterUnitSelected);

            //Event linkage
            //SelectionEvents
            userInput.AddResetables(_selectionState, ui.ConfirmButton, ui.UnitUI, _hints);
            selector.HoverChanged.AddListener(OnHoverChanged);
            selector.SelectionUpdated.AddListener(OnSelectionUpdated);

            //UI Events
            _timelineView.Init(ui.TimelineUI, battle, timelineHints);
            phase.Subscribe(ui.PhaseUI);
            ui.Initialize(userInput);
            ui.PhaseUI.OnClick.AddListener(phase.TogglePhase);
            userInput.Confirm.AddListener(() =>
            {
                if (!ui.ConfirmButton.interactable)
                    _userInput.ForceMouse();
                else
                    ui.ConfirmButton.Click();
            });
            ui.ConfirmButton.AddListener(OnConfirmed);
            ui.EndTurnButton.AddListener(() =>
            {
                userInput.ForceReset();
                StartCoroutine(battle.NextTurn(_delay, () => _selector.RaiseCurrentHover()));
                ui.EndTurnButton.Reset();
            });
            battle.OnTimelineActionAdded.AddListener(d =>
            {
                _selector.RaiseCurrentHover();
                ui.EndTurnButton.interactable = true;
            });
            SetActionUIsCallback(OnActionClicked, userInput);

            userInput.ForceReset();
            StartCoroutine(_battle.InitNewTurn(_delay));
        }

        public bool CanAct(Unit unit) => unit != null && unit.Team == ETeam.Player && _battle.CanStillAct(unit);

        private void OnHoverChanged(SelectionEventData selection)
        {
            var unit = selection.unit;
            bool displayed = false;
            if (_selectionState.CanSelectUnit)
            {
                bool isTeam = unit != null && unit.Team == ETeam.Player && unit.HealthInfo.Alive;
                bool canAct = CanAct(unit);
                _ui.UnitUI.SetUnit(unit, isTeam && canAct, isTeam && !canAct);
                displayed = true;
            }
            else if (_selectionState.CanSelectTarget)
            {
                _ui.TargetUI?.SetInfo(unit?.VisualInformations, new IIcon.IconText[] { });
            }

            _ui.TileUI.SetInfo(!displayed && unit != null
                ? unit
                : selection.environment);
        }

        [SuppressMessage("ReSharper", "ConvertClosureToMethodGroup")]
        private void SetActionUIsCallback(UnityAction<IActionInfo> onClick, UserInput userInput)
        {
            userInput.Action.AddListener(i =>
            {
                if (_selectionState.CanSelectAction && i >= 0 && i < _ui.UnitUI.ActionUIRead.Length)
                    _ui.UnitUI.ActionUIRead[i].Click();
            });
            foreach (var actionUI in _ui.UnitUI.ActionUIRead)
            {
                userInput.AddResetables(actionUI);
                actionUI.OnClick.AddListener(a => _ui.UnitUI.ResetActionUIs(actionUI));
                actionUI.OnClick.AddListener(onClick);
            }
        }

        private void OnActionClicked(IActionInfo a)
        {
            var valid = _selectionState.SelectActionIfValid(a, true);
            if (valid)
            {
                var targs = _battle.PossibleTargetPosition(_selectionState.Origin, a,
                    _unitActionsPreviewShowEmptyTiles);
                _selector.Reset();
                _selector.ShowCursor = true;
                _hints.HintMultiple(targs);
                if (!a.MainTarget.Phase.ToPhase(_phaseSelector.Phase).Intersects(_phaseSelector.Phase))
                    _phaseSelector.TogglePhase();
            }
        }

        private void OnSelectionUpdated(SelectionEventData s)
        {
            if (_selectionState.CanSelectUnit)
            {
                //We ensure that the click is only handled when we are still hovering on the unit, not any click in the screen
                if (s.unit != null && CanAct(s.unit))
                {
                    _selectionState.SetUnit(s.unit, true);
                    _selector.ShowCursor = false;
                }
            }
            else if (_selectionState.CanSelectTarget && _selectionState.AcceptsMoreTargets)
            {
                bool atLeastOneTarget = _selectionState.TryAppendTarget(s, _battle.Tiles);
                if (atLeastOneTarget)
                {
                    _ui.TargetUI?.SetInfo(s.unit?.VisualInformations ?? s.environment.VisualInformations,
                        new IIcon.IconText[] { });
                    _selector.ShowCursor = _selectionState.AcceptsMoreTargets;
                    _selector.RaiseCurrentHover();
                    _ui.ConfirmButton.interactable = true;
                    //TODO Probably maintain a List of targets and not just a single LastTargetUI
                    if (!_selectionState.AcceptsMoreTargets)
                        OnConfirmed();
                }
                else
                {
                    //_userInput.ForceReset();
                }
            }
        }

        private void OnConfirmed()
        {
            var action = _selectionState.Confirm();
            var confirmed = _battle.ConfirmAction(action);
            _userInput.ForceReset();
            _ui.EndTurnButton.highlighted = !_battle.AlliesCanAct;
            if (!confirmed)
            {
                //TODO Show cancel feedback
            } // Else timeline UI should have subscribed to timeline events and be update on it's own
        }
    }
}