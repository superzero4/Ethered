using System.Collections;
using System.Linq;
using BattleSystem;
using Common.Events;
using Common.Events.Combat;
using Common.Events.UserInterface;
using NaughtyAttributes;
using UI.Battle;
using UnitSystem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Views.Battle.Animation;

namespace Views.Battle
{
    public class UnitView : AElementView<Unit>
    {
        [FormerlySerializedAs("_rotationSpeed")] [Header("Settings")] [SerializeField, Range(0.001f, 1f)]
        private float _rotationTime = 0.5f;

        [FormerlySerializedAs("_moveSpeed")] [SerializeField, Range(0.001f, 4f)]
        private float _moveTime = 0.5f;

        [Header("Unit")] [SerializeField] private HealthUI _healthUI;
        [Header("Visuals")] [SerializeField] private bool _showOnlyOnCorrectPhase = false;
        [SerializeField] private UnitSkin[] _skin;
        [SerializeField, Range(0, 3)] private int _skinIndex;
        private UnitSkin currentSkin => _skin[_skinIndex];
        private AnimationPlayer animationPlayer => currentSkin.AnimationPlayer;

        [Header("ReadOnly")] [SerializeField] [ReadOnly]
        private Grid _grid;

        [SerializeReference] private AnimationPlayData _idle;
        [SerializeField] private EPhase _displayedPhase;

        protected override void Init(Grid grid)
        {
            _skinIndex = Mathf.Clamp(_skinIndex, 0, _skin.Length - 1);
            for (int i = 0; i < _skin.Length; i++)
                _skin[i].gameObject.SetActive(i == _skinIndex);
            SetColor();
            SyncVisibility();
            _idle = new AnimationPlayData(AnimationType.Idle, true);
            Assert.IsNotNull(_healthUI, "No HealthUI assigned");
            base.Init(grid);
            _grid = grid;
            Data.OnUnitMoves.AddListener(Move);
            Data.OnUnitHealthChange.AddListener(UpdateHealth);
            Data.OnUnitHealthChange.AddListener(_healthUI.UpdateHealth);
            animationPlayer.Play(AnimationType.Idle, () => false);
        }

        protected override Color GetColor()
        {
            var color = base.GetColor();
            if (_data.Team == ETeam.Player)
            {
                switch (_data.Position.Phase)
                {
                    case EPhase.Normal: color = Color.blue; break;
                    case EPhase.Ethered: color = Color.cyan; break;
                    case EPhase.Both: color = Color.green; break;
                }
            }
            else if (_data.Team == ETeam.Enemy)
            {
                switch (_data.Position.Phase)
                {
                    case EPhase.Normal: color = Color.red; break;
                    case EPhase.Ethered: color = Color.magenta; break;
                    case EPhase.Both: color = Color.yellow; break;
                }
            }

            return color;
        }

        protected override void SetColor(Color color)
        {
            currentSkin.SetSkin(color);
        }

        public override void ToggleVisibility(bool state)
        {
            currentSkin.ToggleVisibility(state);
            _healthUI.ToggleVisibility(state);
        }

        public override void OnPhaseSelected(PhaseEventData arg0)
        {
            base.OnPhaseSelected(arg0);
            _displayedPhase = arg0.phase;
            SyncVisibility();
        }

        private void SyncVisibility()
        {
            if (_showOnlyOnCorrectPhase)
                ToggleVisibiltyFromPhase(_displayedPhase);
        }

        private void UpdateHealth(UnitHitData arg0)
        {
            animationPlayer.Play(
                arg0.unit.HealthInfo.CurrentHealth > arg0.oldHealth ? AnimationType.Healed : AnimationType.Hurt, null,
                arg0.direction);
            SyncVisibility();
        }
        override protected void RotationChanged(float newRot)
        {
            base.RotationChanged(newRot);
            _healthUI.transform.localRotation = Quaternion.Euler(0, -newRot, 0);
        }
        // ReSharper disable Unity.PerformanceAnalysis
        private void Move(UnitMovementData arg0)
        {
            var last = arg0.path.Path[0];
            Vector2Int lastDir = Vector2Int.zero;
            bool running = true;
            var seq = LeanTween.sequence();
            seq.append(() => animationPlayer.Play(AnimationType.Move, () => !running, null));
            foreach (var pos in arg0.path.Path.Skip(1))
            {
                var dir = pos.Position - last.Position;

                if (dir != lastDir)
                    seq.append(LeanTween.value(_root.gameObject, d => Rotation = LookAtRotation(d), lastDir, dir,
                        lastDir == dir ? _rotationTime : 0f));
                seq.append(() =>
                {
                    SetColor();
                    SyncVisibility();
                });
                seq.append(LeanTween.move(_root.gameObject, WorldPosition(_grid, pos), _moveTime));
                //Safe in case of rounding errors in tween
                last = pos;
                lastDir = dir;
            }

            seq.append(() => running = false);
        }
    }
}