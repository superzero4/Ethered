using System;
using BattleSystem;
using Common.Events;
using Common.Events.UserInteraction;
using Common.Events.UserInterface;
using UnityEngine;
using UnityEngine.Events;
using Views.Battle;

namespace UI.Battle
{
    public class PhaseUI : MonoBehaviour, IPhaseView
    {
        [SerializeField] private bool _showTargetPhase = true;
        [SerializeField] private UnityEngine.UI.Button _normal;
        [SerializeField] private UnityEngine.UI.Button _ethered;
        [SerializeField] private UnityEvent _onClick = new();

        public void Initialize()
        {
            _normal.onClick.AddListener(_onClick.Invoke);
            _ethered.onClick.AddListener(_onClick.Invoke);
        }

        public float Progress
        {
            set
            {
                if (_showTargetPhase)
                    value = 1 - value;
                _normal.image.color = SetAlpha(_normal.image.color, 1 - value);
                _ethered.image.color = SetAlpha(_ethered.image.color, value);
            }
        }

        public UnityEvent OnClick => _onClick;

        private Color SetAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}