using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using Common.Events;
using Common.Events.UserInterface;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Views.Battle.Selection;


namespace Views.Battle
{
    public class PostProcessPhaseView : MonoBehaviour, IPhaseView
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private PostProcessVolume _ppv;
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private Vector3 _startSize;
        [SerializeField] private Vector3 _endSize;
        [SerializeField, Range(0.001f, 1f)] private float blendDistanceMul = 0.8f;
        [SerializeField, Range(0, 1f)] private float _progress = 0.5f;

        [Header("Tween")] [SerializeField, Range(0.01f, 10f)]
        private float _duration;

        [SerializeField] private LeanTweenType _easeType;
        private LTDescr tween;

        public void Init()
        {
            _progress = 0;
            UpdateBlendDistance();
        }

        private void UpdateBlendDistance()
        {
            var dist = (_camera.transform.position - (_collider.transform.position + _collider.center)).magnitude;
            _ppv.blendDistance = dist * blendDistanceMul;
            _endSize = 2 * dist * Vector3.one;
            ColliderSize = _progress;
        }

        private float ColliderSize
        {
            get => Mathf.InverseLerp(_startSize.x, _endSize.y, _collider.size.x);
            set => _collider.size = Vector3.Lerp(_startSize, _endSize, value);
        }

        private void OnValidate()
        {
            UpdateBlendDistance();
        }

        public void OnPhaseSelected(PhaseEventData arg0)
        {
            LeanTween.cancel(_ppv.gameObject);
            if (arg0.phase == EPhase.Ethered)
                tween = PostProcessGrow();
            else
                tween = PostProcessShrink();
        }

        private LTDescr PostProcessGrow()
        {
            return LeanTween.value(_ppv.gameObject, ColliderSize, 1f, _duration).setEase(_easeType).setOnUpdate((float val) =>
            {
                _progress = val;
                ColliderSize = _progress;
            });
        }

        private LTDescr PostProcessShrink()
        {
            return LeanTween.value(_ppv.gameObject, ColliderSize, 0f, _duration).setEase(_easeType).setOnUpdate((float val) =>
            {
                _progress = val;
                ColliderSize = _progress;
            });
        }
    }
}