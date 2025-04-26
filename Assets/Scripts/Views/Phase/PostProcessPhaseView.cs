using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using Common.Events;
using Common.Events.UserInterface;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Views.Battle;
using Views.Battle.Selection;


namespace Views.Phase
{
    public class PostProcessPhaseView : MonoBehaviour, IPhaseView
    {
        private Camera _camera;
        [SerializeField] private PostProcessVolume _ppv;
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private Vector3 _startSize;
        [SerializeField] private Vector3 _endSize;
        [SerializeField, Range(0.001f, 1f)] private float blendDistanceMul = 0.8f;
        [SerializeField, Range(0, 1f),Tooltip("For editor test purpose only, isn't reaquired to process the data, the progress is passed by the event")] private float _progress = 0.5f;

        public void Init(Camera camera)
        {
            _camera = camera;
            _progress = 0;
            UpdateBlendDistance();
        }

        private void UpdateBlendDistance()
        {
            if (_camera == null || _collider == null)
                return;
            var dist = (_camera.transform.position - (_collider.transform.position + _collider.center)).magnitude;
            _ppv.blendDistance = dist * blendDistanceMul;
            _endSize = 2 * dist * Vector3.one;
            Progress = _progress;
        }

        public float Progress
        {
            set
            {
                _progress = value;
                _collider.size = Vector3.Lerp(_startSize, _endSize, value);
            }
        }

        public Camera Camera1 => _camera;

        private void OnValidate()
        {
            if(_camera == null)
                _camera = Camera.main;
            UpdateBlendDistance();
        }
    }
}