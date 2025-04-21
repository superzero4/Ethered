using System;
using System.Linq;
using BattleSystem;
using Common.Events;
using Common.Events.UserInteraction;
using Common.Events.UserInterface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Views.Battle;

namespace UI.Battle
{
    public class PhaseUI : MonoBehaviour, IPhaseView
    {
        [SerializeField] private bool _invert = true;

        [SerializeField, Tooltip("Color lerp instead")]
        private bool _alphaFade = false;

        [SerializeField] private UnityEngine.UI.Button _normal;
        [SerializeField] private UnityEngine.UI.Image[] _normalImages;
        [SerializeField] private UnityEngine.UI.Button _ethered;
        [SerializeField] private UnityEngine.UI.Image[] _etheredImages;

        [SerializeField] private UnityEngine.UI.Image[] _commons;
        [SerializeField] private UnityEvent _onClick = new();
        [SerializeField] Color _normalColor;
        [SerializeField] Color _etheredColor;

        public void Initialize()
        {
            _normal.onClick.AddListener(_onClick.Invoke);
            _ethered.onClick.AddListener(_onClick.Invoke);
        }

        public float Progress
        {
            set
            {
                if (_invert)
                    value = 1 - value;
                var lerp = Color.Lerp(_normalColor, _etheredColor, value);
                if (_alphaFade)
                {
                    _normal.image.color = SetAlpha(_normal.image.color, 1 - value);
                    _ethered.image.color = SetAlpha(_ethered.image.color, value);
                }
                else
                {
                    var ilerp = Color.Lerp(_etheredColor, _normalColor, value);
                    foreach (var image in _normalImages)
                        image.color = lerp;

                    foreach (var image in _etheredImages)
                        image.color = ilerp;
                }

                foreach (var image in _commons)
                    image.color = lerp;
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