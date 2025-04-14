using System;
using BattleSystem;
using Common;
using Common.Events.Combat;
using Common.GlobalFlow;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Views.Battle;
using Views.Battle.Selection;
using Views.Phase;
using Object = UnityEngine.Object;

namespace LevelSystem
{
    public class LevelCollectionBattleInitializer : MonoBehaviour
    {
        private static bool _flag;
        private ILevelCollection _levels;

        [Header("References")] [SerializeField]
        private Object _levelsHolder;

        [FormerlySerializedAs("_bindings")] [SerializeField]
        private UserInput _userInput;
        
        [SerializeField] private BattleViewInitializer _battleViewInitializer;
        [SerializeField] private BattleView _battleView;
        [SerializeField] private Selector _selector;
        [SerializeField] private PhaseSelector _phaseSelector;
        [SerializeField] private PostProcessPhaseView _postProcess;
        [SerializeField] private Camera _camera;
        [Header("Settings")] [SerializeField] private bool _goToNextSceneOnEnd = true;
        [SerializeField] private bool _skipShop = true;

        [Header("Dev")] [SerializeField] private bool _autoEnd;

        [SerializeField, UnityEngine.Range(0, 100)]
        private int _levelSkip;

        [Header("Intro")] [SerializeField, UnityEngine.Range(0, 10f)]
        private float _duration;

        [SerializeField] private LeanTweenType _ease = LeanTweenType.easeInOutCubic;

        private void Awake()
        {
            // Initialize the level collection
            Assert.IsTrue(_levelsHolder != null && _levelsHolder is ILevelCollection,
                " _levelsHolder is null or not of type ILevelCollection");
            _levels = _levelsHolder as ILevelCollection;
            if (!_flag)
            {
#if UNITY_EDITOR
                if (_levelsHolder is WorldSO world)
                {
                    world.SetPositionFromMarkerName();
                }
#endif
                _levels.Reset();
                _levels.Increment(_levelSkip);
                _flag = true;
            }

            SetBattle();
        }

        private void SetBattle()
        {
            var current = _levels.Current;
            var precedent = _levels.Precedent;
            
            _postProcess.Init(_camera);

            _phaseSelector.Subscribe(_postProcess);
            _phaseSelector.Subscribe(_selector);
            _phaseSelector.Subscribe();

            _userInput.AddResetables(_selector);
            _userInput.MouseButton.AddListener(_selector.Select);

            _battleViewInitializer.Init(current, _levels.DynamicSquad, _phaseSelector, out var selectables,
                out var battle);
            _selector.Initialize(selectables, _phaseSelector.GetLayerMask(),_camera);
            _battleView.Init(battle, _selector, _phaseSelector, _userInput);
            _userInput.Reset.Invoke();
            _phaseSelector.Initialize(EPhase.Normal);
            battle.BattleEnd.AddListener(OnBattleEnd);
            if (_autoEnd)
                _userInput.Dev.AddListener(e => ForceEnd());
            AnimateBattleView(precedent, current);
        }

        public void ForceEnd()
        {
            OnBattleEnd(new BattleEventData()
            {
                winner = ETeam.Player
            });
        }

        private void OnBattleEnd(BattleEventData t)
        {
            Debug.Log($"Battle Ended, won by {t.winner}");
            if (_goToNextSceneOnEnd)
            {
                if (t.winner == ETeam.Enemy)
                {
                    _levels.Reset();
                    SceneFlow.LoadScene(SceneFlow.EScene.GameOver);
                }
                else
                {
                    _levels.Increment();
                    if (_skipShop)
                    {
                        //TODO hot reload scene intelligently instead if needed
                        SceneFlow.LoadScene(SceneFlow.EScene.Battle);
                    }
                    else
                        SceneFlow.LoadScene(SceneFlow.EScene.SquadMenu);
                }
            }
        }

        private void AnimateBattleView(Level precedent, Level current)
        {
            _battleView.transform.position = precedent.Position;
            _battleView.transform.eulerAngles = precedent.Rotation;
            _battleView.transform.LeanMove(current.Position, _duration).setEase(_ease);
            _battleView.transform.LeanRotate(current.Rotation, _duration).setEase(_ease);
            _camera.transform.LeanMoveLocalX( _battleViewInitializer.Grid.cellSize.x * current.Map.Size.x/2f,_duration);
        }
    }
}