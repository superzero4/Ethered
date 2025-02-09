using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class ClickableUI : InfoUI
    {
        [SerializeField] private Button _button;
        public UnityEvent OnClick => _button.onClick;

        protected virtual void Clicked()
        {
        }

        private void Awake()
        {
            Assert.IsTrue(_button != null);
            OnClick.AddListener(Clicked);
        }
    }
}