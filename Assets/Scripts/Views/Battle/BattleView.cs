using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        [SerializeField] private bool _goToNextSceneOnEnd = true;

        [Header("References")]
        [SerializeField,
         InfoBox("Component responsible for initialization only to spawn corresponding prefabs and call required init")]
        private BattleViewInitializer _initializer;

        [SerializeField, InfoBox("Just a big reference holder")]
        private BattleUI _ui;

        [SerializeField] private Selector _selector;
        [SerializeField] private GlobalMaterialPhaseView _materialPhaseView;
        [SerializeField] private PostProcessPhaseView _postProcess;

        [Header("Read Only")] [SerializeReference] [ReadOnly]
        private SelectionState _selectionState;

        [SerializeReference] [ReadOnly] private BattleSystem.Battle _battle;

        public BattleSystem.Battle Battle
        {
            get => _battle;
        }

        public BattleViewInitializer Initializer
        {
            get { return _initializer; }
        }

        private void Awake()
        {
            _battle = _initializer.Init(_selector.Phase);
            _selectionState = new SelectionState();
            _ui.Initialize();
            //We set callbacks before initializing the _selector because we basically hook on selectione events and we want everything to be set as the selector initializes
            _selector.AddResetables(_selectionState, _ui.ConfirmButton);
            SetCallbacks();
            _selector.Initialize();
            _ui.ConfirmButton.AddListener(OnConfirmed);
            _ui.EndTurnButton.AddListener(() => { StartCoroutine(_battle.NextTurn(_delay)); });
            _battle.BattleEnd.AddListener(t =>
            {
                Debug.Log($"Battle Ended, won by {t.winner}");
                if (_goToNextSceneOnEnd)
                    SceneFlow.LoadScene(t.winner == ETeam.Player
                        ? SceneFlow.EScene.SquadMenu
                        : SceneFlow
                            .EScene.GameOver);
            });
            //_selector.SelectionUpdated.AddListener(s => Debug.Log("Selected: " + s.unit));

            StartCoroutine(_battle.InitNewTurn(_delay));
        }

        public bool CanAct(Unit unit) => unit != null && unit.Team == ETeam.Player && _battle.CanStillAct(unit);

        private void OnHover(SelectionEventData selection)
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
        private void SetCallbacks()
        {
            _battle.OnTimelineAction.AddListener(_ui.TimelineUI1.OnTimelineMemberInserted);

            _selector.Phase.Subscribe(_ui.PhaseUI, _materialPhaseView, _postProcess);

            _selector.OnHoverChanged.AddListener(OnHover);
            _selector.SelectionUpdated.AddListener(UpdateSelection);

            foreach (var actionUI in _ui.UnitUI.ActionUIs)
            {
                _selector.AddResetables(actionUI);
                UnityEvent<IActionInfo> onClick = actionUI.OnClick;
                onClick.AddListener(a =>
                {
                    _ui.UnitUI.ResetActionUIs(actionUI);
                    _selectionState.SelectActionIfValid(a);
                });
                onClick.AddListener(e =>
                {
                    _selector.UpdateHint = true;
                    _selector.Hints.ActivateNew();
                });
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
                Debug.Log("SELECTION Action confirmed");
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
            if (_selectionState.CanSelectUnit)
            {
                if (s.unit != null && CanAct(s.unit))
                {
                    //In theory, already set by the hover event so redundant but as a safe
                    //_ui.UnitUI.SetUnit(s.unit,false);
                    _selectionState.SetUnit(s.unit, true);
                    _selector.UpdateHint = false;
                    _selector.Hints.Lock();
                }
            }
            else if (_selectionState.CanSelectTarget)
            {
                bool atLeastOneTarget = _selectionState.AppendTarget(s);
                if (atLeastOneTarget)
                {
                    _ui.TargetUI.SetInfo(s.unit?.VisualInformations ?? s.environment.VisualInformations);
                    _selector.Hints.Lock();
                    _selector.UpdateHint = _selectionState.AcceptsMoreTargets;
                    _selector.RaiseCurrentHover();
                    _ui.ConfirmButton.interactable = true;
                    //TODO Probably maintain a List of targets and not just a single LastTargetUI
                }
                else
                {
                    //Debug.LogWarning("SELECTION Target not valid");
                    //TODO Show negative feedback showing target wasn't selected
                }
            }
        }
    }
}