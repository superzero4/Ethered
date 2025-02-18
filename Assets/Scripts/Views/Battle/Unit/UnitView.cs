using Common.Events;
using NaughtyAttributes;
using UI.Battle;
using UnitSystem;
using UnityEngine;
using UnityEngine.Assertions;
using Views.Battle.Animation;

namespace Views.Battle
{
    public class UnitView : AElementView<Unit>
    {
        [Header("Unit")] [SerializeField] private HealthUI _healthUI;
        [SerializeField] private AnimationPlayer _animationPlayer;
        [SerializeField] [ReadOnly] private Grid _grid;
        private AnimationPlayData _idle;

        protected override void Init(Grid grid)
        {
            _idle = new AnimationPlayData(AnimationType.Idle, true);
            Assert.IsNotNull(_healthUI, "No HealthUI assigned");
            base.Init(grid);
            _grid = grid;
            Data.OnUnitMoves.AddListener(Move);
            Data.OnUnitHealthChange.AddListener(UpdateHealth);
            Data.OnUnitHealthChange.AddListener(_healthUI.UpdateHealth);
            _animationPlayer.Play(_idle, null);
        }

        private void UpdateHealth(UnitHitData arg0)
        {
            _animationPlayer.Play(new AnimationPlayData(
                        arg0.unit.HealthInfo.CurrentHealth > arg0.oldHealth ? AnimationType.Healed : AnimationType.Hurt,
                        false)
                    .Append(_idle),
                arg0.direction);
        }

        private void Move(UnitMovementData arg0)
        {
            _animationPlayer.Play(new AnimationPlayData(AnimationType.Move, false).Append(_idle), null);
            SnapToCorrectPosition(_grid);
            PickColor();
        }
    }
}