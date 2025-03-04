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
        
        public PhaseEvent OnSelectedPhaseChanges => _onSelectedPhaseChanges;
        public void Subscribe(params IPhaseView[] view)
        {
            foreach (var v in view)
            {
                _onSelectedPhaseChanges.AddListener(v.OnPhaseSelected);
            }
        }
        public bool Contains(EPhase other)
        {
            return (_phase & other) != 0b0;
        }
        
        private void Update()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                _phase = _phase == EPhase.Ethered ? EPhase.Normal : EPhase.Ethered;
                _onSelectedPhaseChanges.Invoke(new PhaseEventData() { phase = _phase });
            }
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
            _onSelectedPhaseChanges.Invoke(new PhaseEventData() { phase = _phase });
        }
        
    }
}