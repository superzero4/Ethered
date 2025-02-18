using System;
using BattleSystem;
using Common.Events;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UI.Battle
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private Image _slider;

        public void Awake()
        {
            Assert.IsTrue(_slider != null && _slider.fillMethod == Image.FillMethod.Horizontal ||
                          _slider.fillMethod == Image.FillMethod.Vertical);
        }

        private void UpdateHealth(float current, float max)
        {
            _slider.fillAmount = current / max;
        }

        public void UpdateHealth(IHealth health)
        {
            UpdateHealth(health.CurrentHealth, health.MaxHealth);
        }

        public void UpdateHealth(UnitHitData hitData)
        {
            //TODO animate from old to new value
            //healthData.oldHealth;
            UpdateHealth(hitData.unit);
        }
    }
}