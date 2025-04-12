using BattleSystem;
using Common.Events.UserInterface;
using UnityEngine;
using Views.Battle;

namespace Views.Phase
{
    public class ScalingPhaseView : MonoBehaviour, IPhaseView
    {
        [SerializeField] private EPhase _phase;
        [SerializeField] private Transform _root;
        public float Progress
        {
            set
            {
                switch (_phase)
                {
                    case EPhase.Normal:
                        value = Mathf.Clamp01((-value + .5f) * 2);
                        break;
                    case EPhase.Ethered:
                        value = Mathf.Clamp01((value - .5f) * 2);
                        break;
                    default:
                        return; //If in both phase or non we don't alter the view
                        break;
                }

                //It animates out in half of the time and in in the other half
                _root.localScale = Vector3.one * value;
            }
        }

        public Transform Root
        {
            set { _root = value; }
            get { return _root; }
        }

        public EPhase Phase
        {
            set
            {
                //TODO trigger the animation also if the phase of the object itself (and not the view) changes
                _phase = value;
            }
            get { return _phase; }
        }
    }
}