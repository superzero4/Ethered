using BattleSystem;
using UnityEditor;
using UnityEngine;

namespace UnitSystem.Actions.Bases
{
    [CreateAssetMenu(fileName = "AbsoluteMovement", menuName = "Actions/Movement/Absolute")]
    public class AbsoluteMovement : BasicMovement
    {
        [SerializeField] private EPhase _destinationPhase;
        public override EPhase TargetPhase(EPhase currentPhase) => _destinationPhase;
    }
}