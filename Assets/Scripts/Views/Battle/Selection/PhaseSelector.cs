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
        private LTDescr tween;
        
        public void Subscribe(params IPhaseView[] view)
        {
            foreach (var v in view)
            {
                _onSelectedPhaseChanges.AddListener(v.OnPhaseSelected);
                _onSelectedPhaseChanges.AddListener(arg => v.Progress = arg.progress);
            }
        }


        private void Update()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                _phase = _phase == EPhase.Ethered ? EPhase.Normal : EPhase.Ethered;
                Invoke();
            }
        }

        private void Invoke()
        {
            LeanTween.cancel(gameObject);
            var data = new PhaseEventData() { targetPhase = _phase };
            if (_phase == EPhase.Ethered)
                tween = Tween(0, 1f);
            else
                tween = Tween(1, 0);
        }


        public LTDescr Tween(float start, float end)
        {
            return LeanTween.value(gameObject, start, end, _duration).setEase(_easeType).setOnUpdate(val =>
            {
                var data = new PhaseEventData() { targetPhase = _phase, progress = val };
                _onSelectedPhaseChanges.Invoke(data);
            });
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

        public void Initialize(EPhase initPhase)
        {
            _phase = initPhase;
            _onSelectedPhaseChanges.Invoke(new PhaseEventData(_phase));
        }
    }
}