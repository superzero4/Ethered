using BattleSystem;
using Common.GlobalFlow;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;
using Views.Battle;
using Views.Battle.Selection;

namespace LevelSystem
{
    public class LevelCollectionBattleInitializer : MonoBehaviour
    {
        private static bool _flag;
        private ILevelCollection _levels;

        [Header("References")] [SerializeField]
        private Object _levelsHolder;

        [SerializeField] private BattleViewInitializer _battleViewInitializer;
        [SerializeField] private BattleView _battleView;
        [SerializeField] private Selector _selector;
        [SerializeField] private PostProcessPhaseView _postProcess;
        [Header("Settings")] [SerializeField] private bool _goToNextSceneOnEnd = true;
        [SerializeField] private bool _skipShop = true;
        [FormerlySerializedAs("duration")]
        [Header("Intro")] 
        [SerializeField,UnityEngine.Range(0,10f)] private float _duration;
        [SerializeField] private LeanTweenType _ease = LeanTweenType.easeInOutCubic;
        private void Awake()
        {
            // Initialize the level collection
            Assert.IsTrue(_levelsHolder != null && _levelsHolder is ILevelCollection,
                " _levelsHolder is null or not of type ILevelCollection");
            _levels = _levelsHolder as ILevelCollection;
            if (!_flag)
            {
                _levels.Reset();
                _flag = true;
            }

            SetBattle();
        }

        private void SetBattle()
        {
            var current = _levels.Current;
            var precedent = _levels.Precedent;

            _selector.Phase.Subscribe(_postProcess);
            _postProcess.Init();
            Battle battle = _battleViewInitializer.Init(current, _selector.Phase);
            _battleView.transform.position = precedent.Position;
            _battleView.transform.eulerAngles = precedent.Rotation;
            _battleView.transform.LeanMove(current.Position, _duration).setEase(_ease);
            _battleView.transform.LeanRotate(current.Rotation, _duration).setEase(_ease);
            _battleView.Init(battle, _selector);
            battle.BattleEnd.AddListener(t =>
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
            });
        }
    }
}