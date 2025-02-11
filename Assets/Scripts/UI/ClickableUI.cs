using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public abstract class ClickableUI<EventArg> : InfoUI
    {
        [SerializeField] private Button _button;
        [SerializeField] private UnityEvent<EventArg> _onClick = new();

        public UnityEvent<EventArg> OnClick => _onClick;
        /// <summary>
        /// This is supposed to handle any internal logic/changes that should happen on click, it has the same information than the external that would subscribe to the method, possibly empty
        /// </summary>
        /// <param name="args"></param>
        protected abstract void Clicked(EventArg args);

        private void Awake()
        {
            Assert.IsTrue(_button != null);
            //We forward the event through another event with be uses externally, and we also call a abstract method for OnClick logic internal to this class
            _button.onClick.AddListener(() => _onClick.Invoke(GetArgs()));
            _onClick.AddListener(Clicked);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>The arguments of specified type that will be raised with the event, typically a status represented by a field or other logic, continaing information about what was pressed</returns>
        protected abstract EventArg GetArgs();
    }
    /// <summary>
    /// Simple helper class for No Args clickable UI, we use mother class with bool, just because we are forced to provide a type, but we don't use it
    /// </summary>
    public class ClickableUI : ClickableUI<bool>{
        public void AddListener(Action action)
        {
            OnClick.AddListener(_ => action());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">Ignored boolean, always true, doesn't contains any information, do not considere it as a parameter</param>
        protected override void Clicked(bool args)
        {
            
        }

        protected override bool GetArgs()
        {
            return true;
        }
    }
}