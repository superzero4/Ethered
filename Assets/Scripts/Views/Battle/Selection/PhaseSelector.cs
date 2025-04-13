using System;
using BattleSystem;
using Common.Events;
using Common.Events.UserInterface;
using NaughtyAttributes;
using UnityEngine;

namespace Views.Battle.Selection
{
    public class PhaseSelector : MonoBehaviour
    {
        public const int SelectableLayer = 6;
        [SerializeField] private PhaseEvent _onSelectedPhaseChanges = new();
        [SerializeField] [ReadOnly] private EPhase _phase;

        [Header("Tween")] [SerializeField, Range(0.01f, 10f)]
        private float _duration;

        [SerializeField] private LeanTweenType _easeType;

        public void Initialize(EPhase initPhase)
        {
            IPhaseView.ease = _easeType;
            IPhaseView.duration = _duration;
            _phase = initPhase;
            Invoke();
        }
        
        public void Subscribe(params IPhaseView[] view)
        {
            foreach (var v in view)
            {
                _onSelectedPhaseChanges.AddListener(v.OnPhaseChanged);
                _onSelectedPhaseChanges.AddListener(arg => v.Progress = arg.progress);
            }
        }


        private void Update()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                TogglePhase();
            }
        }

        public void TogglePhase()
        {
            _phase = _phase == EPhase.Ethered ? EPhase.Normal : EPhase.Ethered;
            Invoke();
        }

        private void Invoke()
        {
            IPhaseView.Invoke(gameObject, _phase, _onSelectedPhaseChanges.Invoke);
        }

        public LayerMask GetLayerMask()
        {
            return 0b1 << Layer();
        }

        private static int Layer()
        {
            return SelectableLayer;
        }

        public void SetLayer<T>(AElementView<T> element) where T : IBattleElement
        {
            element.gameObject.layer = Layer();
        }
    }
}