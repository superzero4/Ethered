using System;
using System.Diagnostics.CodeAnalysis;
using BattleSystem;
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

        [Header("References")] [SerializeField, InfoBox("Just a big reference holder")]
        private BattleUI _ui;

        [SerializeField] private TimelineView _timelineView;


        [Header("Read Only")] [SerializeReference] [ReadOnly]
        private SelectionState _selectionState;

        [SerializeField] [ReadOnly] private Selector _selector;

        [SerializeReference] [ReadOnly] private BattleSystem.Battle _battle;

        public BattleSystem.Battle Battle
        {
            get => _battle;
        }


        public void Init(BattleSystem.Battle battle, Selector selector)
        {
            //Creation
            _selector = selector;
            _battle = battle;
            _selectionState = new SelectionState();

            //Event linkage

            //SelectionEvents
            _selector.Phase.Subscribe(_ui.PhaseUI);
            _selector.AddResetables(_selectionState, _ui.ConfirmButton);
            _selector.HoverChanged.AddListener(OnHoverChanged);
            _selector.SelectionUpdated.AddListener(OnSelectionUpdated);

            //UI Events
            _timelineView.Init(_ui.TimelineUI, _battle);
            _ui.Initialize();
            _ui.ConfirmButton.AddListener(OnConfirmed);
            _ui.EndTurnButton.AddListener(() => StartCoroutine(_battle.NextTurn(_delay)));
            SetActionUIsCallback(OnActionClicked);

            StartCoroutine(_battle.InitNewTurn(_delay));
        }

        public bool CanAct(Unit unit) => unit != null && unit.Team == ETeam.Player && _battle.CanStillAct(unit);

        private void OnHoverChanged(SelectionEventData selection)
        {
            var unit = selection.unit;
            if (_selectionState.CanSelectUnit)
            {
                bool isTeam = unit != null && unit.Team == ETeam.Player;
                bool canAct = CanAct(unit);
                _ui.UnitUI.SetUnit(unit, isTeam && canAct, isTeam && !canAct);
            }
            else if (_selectionState.CanSelectTarget)
            {
                _ui.TargetUI.SetInfo(unit?.VisualInformations ?? VisualInformations.Default);
            }

            _ui.TileUI.SetInfo(selection.environment.Info);
        }

        [SuppressMessage("ReSharper", "ConvertClosureToMethodGroup")]
        private void SetActionUIsCallback(UnityAction<IActionInfo> onClick)
        {
            foreach (var actionUI in _ui.UnitUI.ActionUIs)
            {
                _selector.AddResetables(actionUI);
                actionUI.OnClick.AddListener(a => _ui.UnitUI.ResetActionUIs(actionUI));
                actionUI.OnClick.AddListener(onClick);
            }
        }

        private void OnActionClicked(IActionInfo a)
        {
            var valid = _selectionState.SelectActionIfValid(a);
            if (valid)
            {
                var targs = _battle.PossibleTargetPosition(_selectionState.Origin, a, _unitActionsPreviewShowEmptyTiles);
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
            else if (_selectionState.CanSelectTarget)
            {
                bool atLeastOneTarget = _selectionState.AppendTarget(s);
                if (atLeastOneTarget)
                {
                    _ui.TargetUI.SetInfo(s.unit?.VisualInformations ?? s.environment.VisualInformations);
                    _selector.ShowHints = _selectionState.AcceptsMoreTargets;
                    _selector.RaiseCurrentHover();
                    _ui.ConfirmButton.interactable = true;
                    //TODO Probably maintain a List of targets and not just a single LastTargetUI
                }
            }
        }

        private void OnConfirmed()
        {
            var action = _selectionState.Confirm();
            var confirmed = _battle.ConfirmAction(action);
            _selector.Reset();
            if (!confirmed)
            {
                //TODO Show cancel feedback
            } // Else timeline UI should have subscribed to timeline events and be update on it's own
        }
    }
}