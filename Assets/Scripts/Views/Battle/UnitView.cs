using Common.Events;
using NaughtyAttributes;
using UI.Battle;
using UnitSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace Views.Battle
{
    public class UnitView : AElementView<Unit>
    {
        [Header("Unit")] [SerializeField] private HealthUI _healthUI;
        [SerializeField] [ReadOnly] private Grid _grid;

        protected override void Init(Grid grid)
        {
            Assert.IsNotNull(_healthUI, "No HealthUI assigned");
            base.Init(grid);
            _grid = grid;
            Data.OnUnitMoves.AddListener(Move);
            Data.OnUnitHealthChange.AddListener(UpdateHealth);
            Data.OnUnitHealthChange.AddListener(_healthUI.UpdateHealth);
        }

        private void UpdateHealth(UnitHealthData arg0)
        {
            //TDOO activate feedback using eventually using arg0.oldHealth value
        }

        private void Move(UnitMovementData arg0)
        {
            //TODO animate movement from arg0.oldPosition to _unit.Position
            SnapToCorrectPosition(_grid);
            PickColor();
        }
    }
}