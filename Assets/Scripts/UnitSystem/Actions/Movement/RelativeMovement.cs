using BattleSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnitSystem.Actions.Bases
{
    [CreateAssetMenu(fileName = "RelativeMovement", menuName = "Actions/Movement/Relative")]
    public class RelativeMovement : BasicMovement
    {
        [FormerlySerializedAs("_destinationPhase")] [SerializeField] private ERelativePhase _phaseConstraint;

        public override EPhase TargetPhase(EPhase currentPhase) =>
            TargetDefinition.ConvertRelativePhase(_phaseConstraint, currentPhase);
    }
}