using BattleSystem;
using Common.Events.UserInterface;
using UnityEngine;
using Views.Battle;

namespace Views.Phase
{
    public class ScalingPhaseView : MonoBehaviour, IPhaseView
    {
        private void Awake()
        {
        }

        [SerializeField] private EPhase _phase;
        private EPhase _shownPhase;
        [SerializeField] private Transform _root;

        public void OnPhaseChanged(PhaseEventData data)
        {
            _shownPhase = data.targetPhase;
        }

        private float Remap(float value, bool invert)
        {
            //It animates out in half of the time and in in the other half
            return Mathf.Clamp01(((invert ? -1 : 1) * (value - .5f)) * 2);
        }

        public float Progress
        {
            set
            {
                switch (_phase)
                {
                    case EPhase.Normal:
                        value = Remap(value, true);
                        break;
                    case EPhase.Ethered:
                        value = Remap(value, false);
                        break;
                    default:
                        return; //If in both phase or non we don't alter the view
                }

                RawSet(value);
            }
            get => _root.localScale.x;
        }

        private void RawSet(float value)
        {
            _root.localScale = Vector3.one * value;
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
                if (_phase == value)
                    return;
                _phase = value;
                float start = _phase.Intersects(_shownPhase) || _shownPhase==EPhase.None ? 0 : 1;
                IPhaseView.Tween(gameObject, _phase, _phase == EPhase.Both ? Progress : start, 1 - start, RawSet);
            }
            get { return _phase; }
        }
    }
}