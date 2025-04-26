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
        public static LTDescr current;

        // ReSharper disable Unity.PerformanceAnalysis
        public static bool Invoke(GameObject gameObject, EPhase _phase, Action<PhaseEventData> _onSelectedPhaseChanges)
        {
            if (current != null && current.ratioPassed < 1)
                return false;
            var data = new PhaseEventData() { targetPhase = _phase };
            Action<float> _onUpdate = (float val) =>
            {
                _progress = val;
                data.progress = val;
                _onSelectedPhaseChanges.Invoke(data);
            };
            if (_phase == EPhase.Ethered)
                current = Tween(gameObject, _phase, _progress, 1f, _onUpdate);
            else
                current = Tween(gameObject, _phase, _progress, 0f, _onUpdate);
            return true;
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