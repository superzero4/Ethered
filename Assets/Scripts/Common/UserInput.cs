using System;
using Common.Events.UserInteraction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Common
{
    public class UserInput : MonoBehaviour
    {
        [SerializeField] private KeyCode[] confirmKey = new KeyCode[]
            { KeyCode.KeypadEnter, KeyCode.Return, KeyCode.Space };

        private UnityEvent _confirm = new();
        public UnityEvent Confirm => _confirm;
        [SerializeField] private KeyCode[] endTurn = new[] { KeyCode.Backspace };
        private UnityEvent _endTurn = new();
        public UnityEvent EndTurn => _endTurn;
        [SerializeField] private KeyCode action1 = KeyCode.Alpha1;
        [SerializeField, Range(0, 10)] private int _maxActions = 5;
        private UnityEvent<int> _action0 = new();
        public UnityEvent<int> Action0 => _action0;
        [SerializeField] private KeyCode[] resetKey = new[] { KeyCode.Escape };
        [SerializeField, Range(1, 4)] private int ResetMouseButton = 1;
        private ResetEvent _reset = new();
        public ResetEvent Reset => _reset;
        public void ForceReset()
        {
            _reset.Invoke();
        }
        [SerializeField, Range(0, 4)] private int _principalMouseButton = 0;
        private UnityEvent _mouseButton = new();
        public UnityEvent MouseButton => _mouseButton;
        public void ForceMouse()
        {
            _mouseButton.Invoke();
        }
        private void Update()
        {
            CheckKey(endTurn, _endTurn);
            CheckKey(confirmKey, _confirm);
            for (int i = 0; i < _maxActions; i++)
                if (Input.GetKeyDown(action1 + i))
                    _action0.Invoke(i);
            if (Input.GetMouseButtonDown(ResetMouseButton))
                _reset.Invoke();
            for (var i = 0; i < resetKey.Length; i++)
                if (Input.GetKeyDown(resetKey[i]))
                    _reset.Invoke();
            if (Input.GetMouseButtonDown(_principalMouseButton))
                _mouseButton.Invoke();
        }

        private void CheckKey(KeyCode[] keys, UnityEvent action)
        {
            for (int i = 0; i < keys.Length; i++)
                if (Input.GetKeyDown(keys[i]))
                    action.Invoke();
        }

        private void AddResetableElement(IReset resetable) => _reset.AddListener(resetable.Reset);

        public void AddResetables(params IReset[] resetable)
        {
            foreach (var resetable1 in resetable)
            {
                AddResetableElement(resetable1);
            }
        }
    }
}