using System;
using BattleSystem;
using Common.Events;
using Common.Events.UserInterface;
using UnityEngine;

namespace Views.Battle
{
    public interface IPhaseView
    {
        private static float _progress;
        public static float duration;
        public static LeanTweenType ease;

        // ReSharper disable Unity.PerformanceAnalysis
        public static void Invoke(GameObject gameObject, EPhase _phase, Action<PhaseEventData> _onSelectedPhaseChanges)
        {
            LeanTween.cancel(gameObject);
            var data = new PhaseEventData() { targetPhase = _phase };
            Action<float> _onUpdate = (float val) =>
            {
                _progress = val;
                data.progress = val;
                _onSelectedPhaseChanges.Invoke(data);
            };
            if (_phase == EPhase.Ethered)
                Tween(gameObject, _phase, _progress, 1f, _onUpdate);
            else
                Tween(gameObject, _phase, _progress, 0f, _onUpdate);
        }

        public static LTDescr Tween(GameObject gameObject, EPhase _phase, float start, float end,
            Action<float> _onSelectedPhaseChanges)
        {
            float dur = duration * (Mathf.Abs(start - end));
            return LeanTween.value(gameObject, start, end, dur).setEase(ease).setOnUpdate(_onSelectedPhaseChanges)
                .setOnComplete(
                    () => _onSelectedPhaseChanges(end));
        }

        void OnPhaseChanged(PhaseEventData data)
        {
        }

        float Progress { set; }
    }
}