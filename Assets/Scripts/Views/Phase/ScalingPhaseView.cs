using BattleSystem;
using Common.Events.UserInterface;
using UnityEngine;
using UnityEngine.Serialization;
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

        //On the displayed phase changed
        public void OnPhaseChanged(PhaseEventData data)
        {
            if (data.targetPhase != _shownPhase)
            {
                _shownPhase = data.targetPhase;
                //We cancel any running tween that would be working on the scale for this specific object, when this method is call we update all globally the objects that depends on a phase
                LeanTween.cancel(gameObject);
            }
            else if (data.progress == 0f || data.progress >= 1f)
            {
                
            }
        }

        //When we change the phase where the view lives in
        public EPhase Phase
        {
            set
            {
                if (_phase == value)
                    return;
                _phase = value;
                float start = StartValue();
                IPhaseView.Tween(gameObject, _phase, _phase == EPhase.Both ? Progress : start, 1 - start, RawSet);
            }
            get { return _phase; }
        }

        private int StartValue()
        {
            return _phase.Intersects(_shownPhase) || _shownPhase == EPhase.None ? 0 : 1;
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
                        //value = Mathf.Min(value, _progress);
                        value = Remap(value, true);
                        break;
                    case EPhase.Ethered:
                        //value = Mathf.Max(value, _progress);
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
            _progress = value;
            RawSet(value, _root);
        }

        private static void RawSet(float value, Transform target)
        {
            target.localScale = Vector3.one * value;
        }


        public Transform Root
        {
            set { _root = value; }
            get { return _root; }
        }

        private float _progress;
    }
}