using System;
using NaughtyAttributes;
using System.Linq;
using BattleSystem;
using Common;
using Common.Events.Combat;
using Common.GlobalFlow;
using SquadSystem;
using UI.Battle;
using UnitSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Views.Battle;
using Views.Battle.Selection;
using Views.Phase;

namespace LevelSystem
{
    public class LevelCollectionBattleInitializer : MonoBehaviour
    {
        [Header("Dev")] [SerializeField] private bool _autoEnd;
        [SerializeField] private bool _cinematicMode = false;

        [Header("References")] [SerializeField]
        private LevelProgression _levelProgression;

        [Header("Battle")] [SerializeField] private BattleViewInitializer _battleViewInitializer;

        [SerializeField] private BattleView _battleView;
        [Header("Selection")] [SerializeField] private Selector _selector;
        [SerializeField] private PhaseSelector _phaseSelector;
        [SerializeField] private Grid _grid;
        [SerializeField] private PooledHints _timelineHints;

        [Header("UIX")] [FormerlySerializedAs("_bindings")] [SerializeField]
        private UserInput _userInput;

        [SerializeField] private BattleUI _ui;

        [Header("Camera")] [SerializeField] private Camera _camera;
        [SerializeField] private PostProcessPhaseView _postProcess;


        [Header("Settings")] [SerializeField] private bool _goToNextSceneOnEnd = true;
        [SerializeField] private bool _skipShop = true;


        [Header("Intro")] [SerializeField, UnityEngine.Range(0, 10f)]
        private float _duration;

        [SerializeField] private LeanTweenType _ease = LeanTweenType.easeInOutCubic;

        [FormerlySerializedAs("_nextScenDelay")] [SerializeField, Range(0.001f, 10f)]
        private float _nextSceneDelay = .5f;


        [Header("ReadOnly")] [SerializeReference, ReadOnly]
        private TileHints _hints;

        private void Start()
        {
            _nextScene = SceneFlow.EScene.Unset;
            SetBattle();
        }

        private bool _battleEnded => _nextScene != SceneFlow.EScene.Unset;
        private SceneFlow.EScene _nextScene = SceneFlow.EScene.Unset;


        private ILevelCollection _levels => LevelProgression.Instance.Levels;
        private EncounterInfo _dynamicSquad => LevelProgression.Instance.DynamicSquad;

        private void SetBattle()
        {
            var current = _levels.Current;
            PlaceGrid(current);
            var precedent = _levels.Precedent;

            _postProcess.Init(_camera);

            _phaseSelector.Subscribe(_postProcess);
            _phaseSelector.Subscribe(_selector);
            _phaseSelector.Subscribe();

            _userInput.AddResetables(_selector);
            _userInput.MouseButton.AddListener(_selector.Select);
            var tempSquad = new Squad(_dynamicSquad.Units);
            if (current.PlayerActionsOverride != null && current.PlayerActionsOverride.Length > 0)
            {
                tempSquad.Trim(current.PlayerActionsOverride.Length);
                for (int i = 0; i < tempSquad.Units.Count; i++)
                    tempSquad.Units[i] = new UnitInfo(tempSquad.Units[i], current.PlayerActionsOverride);
            }

            var battle = _battleViewInitializer.Init(current, tempSquad, _phaseSelector, _grid, out var selectables);
            _hints = new TileHints(selectables);
            _timelineHints.Init(2, _grid);
            _selector.Initialize(selectables, Selector.GetLayerMask(), _camera, _grid);
            _battleView.Init(battle, _selector, _phaseSelector, _userInput, _ui, _hints, _timelineHints);
            battle.OnTimelineActionAdded.AddListener(d =>
            {
                if (d.isReset && _battleEnded)
                {
                    _ui.EndTurnButton.Emphasize = true;
                    _ui.EndTurnButton.RemoveAllListeners();
                    _ui.EndTurnButton.AddListener(() => SceneFlow.LoadScene(_nextScene));
                }
            });
            _userInput.Reset.Invoke();
            _phaseSelector.Initialize(EPhase.Normal);
            battle.BattleEnd.AddListener(OnBattleEndCache);
            if (_autoEnd)
                _userInput.Dev.AddListener(e => ForceEnd(battle));
            if (_cinematicMode)
            {
                _ui.gameObject.SetActive(false);
                _camera.cullingMask = 0b1 << LayerMask.NameToLayer("Default");
            }

            LeanTween.sequence()
                .append(AnimateBattleView(precedent, current))
                .append(() =>
                {
                    _ui.PhaseUI.ToggleVisibility(!current.OnlyOnePhase);
                    _selector.ShowCursor = true;
                });
        }

        private void PlaceGrid(Level current)
        {
            _grid.transform.parent = null;
            _grid.transform.position = current.Position;
            _grid.transform.eulerAngles = current.Rotation;
        }

        public void ForceEnd(Battle battle)
        {
            OnBattleEndCache(new BattleEventData()
            {
                winner = ETeam.Player,
                alliesStatus = battle.Units.Where(u => u.Team == ETeam.Player).ToList(),
            });
            SceneFlow.LoadScene(_nextScene);
        }

        private void OnBattleEndCache(BattleEventData t)
        {
            Debug.Log($"Battle Ended, won by {t.winner}");
            SceneFlow.EScene dest = SceneFlow.EScene.Unset;
            if (_goToNextSceneOnEnd)
            {
                if (t.winner == ETeam.Enemy)
                {
                    _levels.Reset();
                    dest = SceneFlow.EScene.GameOver;
                }
                else
                {
                    var current = _levels.Current;
                    _dynamicSquad.Coins += current.Rewards.Coins;
                    _dynamicSquad.Ether += current.Rewards.Ether;
                    var filtered = _dynamicSquad.Units.Units.Where((inf, i) =>
                        i >= t.alliesStatus.Count || t.alliesStatus[i].HealthInfo.Alive);
                    _dynamicSquad.Units.ReplaceUnitInfo(filtered);
                    //We keep the base squad without the potential locla overrride but we filter out the dead units
                    _levels.Increment(1, out bool finished);
                    if (finished)
                        dest = SceneFlow.EScene.GameOver;
                    else if (!current.ShowShop)
                    {
                        //TODO hot reload scene intelligently instead if needed
                        dest = SceneFlow.EScene.Battle;
                    }
                    else
                        dest = SceneFlow.EScene.SquadMenu;
                }

                _nextScene = dest;
            }
        }

        private LTDescr AnimateBattleView(Level precedent, Level current)
        {
            _battleView.transform.position = precedent.Position;
            _battleView.transform.eulerAngles = precedent.Rotation;
            _battleView.transform.LeanMove(current.Position, _duration).setEase(_ease);
            _battleView.transform.LeanRotate(current.Rotation, _duration).setEase(_ease);
            return _camera.transform.LeanMoveLocalX(_grid.cellSize.x * current.Map.Size.x / 2f,
                _duration);
        }
    }
}