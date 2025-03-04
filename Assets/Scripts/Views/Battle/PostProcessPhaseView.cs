using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using Common.Events;
using Common.Events.UserInterface;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


namespace Views.Battle
{
    public class PostProcessPhaseView : MonoBehaviour, IPhaseView
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private PostProcessVolume _ppv;
        [SerializeField] private BoxCollider _collider;
        private Vector3 _startSize;
        [SerializeField] private Vector3 _endSize;
        [SerializeField, Range(0.001f, 4f)] private float step = 0.1f;
        [SerializeField, Range(0.001f, 1f)] private float blendDistanceMul = 0.8f;
        private Coroutine _coroutine;

        private void Awake()
        {
            _startSize = _collider.size;
            UpdateBlendDistance();
        }

        private void UpdateBlendDistance()
        {
            var dist = (_camera.transform.position - (transform.position + _collider.center)).magnitude;
            _ppv.blendDistance = dist * blendDistanceMul;
            _endSize = _startSize + dist * Vector3.one;
        }

        private void OnValidate()
        {
            UpdateBlendDistance();
        }

        public void OnPhaseSelected(PhaseEventData arg0)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            if (arg0.phase == EPhase.Ethered)
                _coroutine = StartCoroutine(PostProcessGrow());
            else
                _coroutine = StartCoroutine(PostProcessShrink());
        }

        private IEnumerator PostProcessGrow()
        {
            while (_collider.size.x < _endSize.x)
            {
                yield return new WaitForEndOfFrame();
                _collider.size += Vector3.one * (step * Time.deltaTime);
            }

            _collider.size = _endSize;
        }

        private IEnumerator PostProcessShrink()
        {
            while (_collider.size.x > _startSize.x)
            {
                yield return new WaitForEndOfFrame();
                _collider.size -= (Time.deltaTime * step) * Vector3.one;
            }

            _collider.size = _startSize;
        }
    }
}