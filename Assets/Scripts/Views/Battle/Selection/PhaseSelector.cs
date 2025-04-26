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
        [SerializeField] private PhaseEvent _onSelectedPhaseChanges = new();
        [SerializeField] [ReadOnly] private EPhase _phase;

        [Header("Tween")] [SerializeField, Range(0.01f, 10f)]
        private float _duration;

        [SerializeField] private LeanTweenType _easeType;

        public EPhase Phase => _phase;

        public void Initialize(EPhase initPhase)
        {
            IPhaseView.ease = _easeType;
            IPhaseView.duration = _duration;
            Invoke(initPhase);
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
            Invoke(_phase == EPhase.Normal ? EPhase.Ethered : EPhase.Normal);
        }

        private void Invoke(EPhase target)
        {
            if (IPhaseView.Invoke(gameObject, target, _onSelectedPhaseChanges.Invoke))
                _phase = target;
        }
    }
}