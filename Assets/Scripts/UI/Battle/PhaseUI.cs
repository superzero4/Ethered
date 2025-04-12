using System;
using BattleSystem;
using Common.Events;
using Common.Events.UserInterface;
using UnityEngine;
using Views.Battle;

namespace UI.Battle
{
    public class PhaseUI : MonoBehaviour, IPhaseView
    {
        [SerializeField] private UnityEngine.UI.Image _normal;
        [SerializeField] private UnityEngine.UI.Image _ethered;

        public float Progress
        {
            set
            {
                _normal.color = SetAlpha(_normal.color, 1 - value);
                _ethered.color = SetAlpha(_ethered.color, value);
            }
        }

        private Color SetAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}