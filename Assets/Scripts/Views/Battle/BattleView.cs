using System;
using System.Diagnostics.CodeAnalysis;
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
        [Header("Settings")] [SerializeField] private float _delay = 0.5f;
        [SerializeField] private bool _unitActionsPreviewShowEmptyTiles = true;
        [SerializeField] private bool _allowActionChangeAfterUnitSelected = true;

        [Header("References")] [SerializeField, InfoBox("Just a big reference holder")]
        private BattleUI _ui;

        [SerializeField] private TimelineView _timelineView;


        [Header("Read Only")] [SerializeReference] [ReadOnly]
        private SelectionState _selectionState;

        private UserInput _userInput;
        [SerializeField] [ReadOnly] private Selector _selector;

        [SerializeReference] [ReadOnly] private BattleSystem.Battle _battle;

        public BattleSystem.Battle Battle
        {
            get => _battle;
        }


        public void Init(BattleSystem.Battle battle, Selector selector, PhaseSelector phase, UserInput userInput)
        {
            //Creation
            _userInput = userInput;
            _selector = selector;
            _battle = battle;
            _selectionState = new SelectionState(_allowActionChangeAfterUnitSelected);

            //Event linkage
            //SelectionEvents
            userInput.AddResetables(_selectionState, _ui.ConfirmButton);
            selector.HoverChanged.AddListener(OnHoverChanged);
            selector.SelectionUpdated.AddListener(OnSelectionUpdated);

            //UI Events
            _timelineView.Init(_ui.TimelineUI, battle);
            phase.Subscribe(_ui.PhaseUI);
            _ui.Initialize(userInput);
            _ui.PhaseUI.OnClick.AddListener(phase.TogglePhase);
            userInput.Confirm.AddListener(() =>
            {
                if (!_ui.ConfirmButton.interactable)
                    _userInput.ForceMouse();
                else
                    _ui.ConfirmButton.Click();
            });
            _ui.ConfirmButton.AddListener(OnConfirmed);
            _ui.EndTurnButton.AddListener(() =>
            {
                userInput.ForceReset();
                StartCoroutine(battle.NextTurn(_delay));
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
                bool isTeam = unit != null && unit.Team == ETeam.Player;
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
            userInput.Action0.AddListener(i =>
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
                _selector.Hints.HintMultiple(targs);
                _selector.ShowHints = true;
            }
        }

        private void OnSelectionUpdated(SelectionEventData s)
        {
            if (_selectionState.CanSelectUnit)
            {
                if (s.unit != null && CanAct(s.unit))
                {
                    _selectionState.SetUnit(s.unit, true);
                    _selector.ShowHints = false;
                }
            }
            else if (_selectionState.CanSelectTarget && _selectionState.AcceptsMoreTargets)
            {
                bool atLeastOneTarget = _selectionState.TryAppendTarget(s, _battle.Tiles);
                if (atLeastOneTarget)
                {
                    _ui.TargetUI?.SetInfo(s.unit?.VisualInformations ?? s.environment.VisualInformations,
                        new IIcon.IconText[] { });
                    _selector.ShowHints = _selectionState.AcceptsMoreTargets;
                    _selector.RaiseCurrentHover();
                    _ui.ConfirmButton.interactable = true;
                    //TODO Probably maintain a List of targets and not just a single LastTargetUI
                    if (!_selectionState.AcceptsMoreTargets)
                        OnConfirmed();
                }
                else
                {
                    Debug.LogWarning(
                        "Reseting on target couldn't append isn't really a good thing, we should try append AND validate the execution on map and then append instead of TryAppend then confirm execution after appending has been made and then reset to compensate that as we do currently");
                    _userInput.ForceReset();
                }
            }
        }

        private void OnConfirmed()
        {
            var action = _selectionState.Confirm();
            var confirmed = _battle.ConfirmAction(action);
            _userInput.ForceReset();
            if (!confirmed)
            {
                //TODO Show cancel feedback
            } // Else timeline UI should have subscribed to timeline events and be update on it's own
        }
    }
}