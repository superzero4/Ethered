using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;
using Action = System.Action;

namespace Views.Battle.Animation
{
    public class AnimationPlayData
    {
        private AnimationType _type;
        private bool _loop;
        private AnimationPlayData _onEnd;

        /// <summary>
        /// Use append at the end of constructor to append multiple other animations
        /// </summary>
        /// <param name="type"></param>
        /// <param name="loop"></param>
        public AnimationPlayData(AnimationType type, bool loop)
        {
            _type = type;
            _loop = loop;
        }

        public AnimationType Type => _type;

        public bool Loop => _loop;

        /// <summary>
        /// Use append for setter
        /// </summary>
        public AnimationPlayData OnEnd => _onEnd;

        public AnimationPlayData Append(AnimationPlayData toAppend)
        {
            AnimationPlayData last = this;
            while (last.OnEnd != null)
            {
                last = last.OnEnd;
            }

            last._onEnd = toAppend;
            return this;
        }
    }

    [RequireComponent(typeof(UnityEngine.Animator))]
    public class AnimationPlayer : MonoBehaviour
    {
        [InfoBox("The animation list this character is using, specific for one character, weapon type, global...")]
        [SerializeField]
        private AnimationList _animationList;

        [SerializeField] private Animator _animator;
        private AnimationSystem _animation;

        private void Awake()
        {
            Assert.IsFalse(_animator.applyRootMotion, "Animator shouldn't have applyRootMotion enabled");
            _animation = new AnimationSystem(_animationList[AnimationType.Idle].Clip, _animator, this);
        }

        private IEnumerator Start()
        {
            yield break; //Testing animations
            while (true)
            {
                Play(AnimationType.Attack, null);
                yield return new WaitForSeconds(1);
            }
        }

        Coroutine _currentCoroutine;

        public void Play(AnimationType type, bool loop = false, Action onAnimationEvent = null, Action onEnd = null)
        {
            var clip = _animationList[type];
            _animation.PlayOneShot(clip.Clip, loop);
            if (onAnimationEvent != null)
            {
                if (clip.TriggerTime > 0)
                {
                    StartCoroutine(WaitForTrigger(onAnimationEvent, clip.TriggerTime, clip.Clip.length));
                }
                else
                {
                    onAnimationEvent?.Invoke();
                }
            }

            if (onEnd != null)
                StartCoroutine(WaitForTrigger(onEnd, 1f, clip.Clip.length));
        }

        public void Play(AnimationType type, Func<bool> stopWhen, Action onAnimationEvent = null)
        {
            Play(type, true, onAnimationEvent);
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(EndAfter(stopWhen));
        }

        public void Play(AnimationType type, float time, Action onAnimationEvent = null)
        {
            Play(type, true, onAnimationEvent);
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(EndAfter(time));
        }

        private IEnumerator EndAfter(Func<bool> stopWhen)
        {
            yield return new WaitUntil(stopWhen);
            _animation.BlendOutNow();
        }

        private IEnumerator EndAfter(float time)
        {
            yield return new WaitForSeconds(time);
            _animation.BlendOutNow();
        }

        private IEnumerator WaitForTrigger(System.Action onTrigger, float time, float animationDuration)
        {
            float elpasedTime = 0;
            while ((elpasedTime / animationDuration) < time)
            {
                elpasedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            onTrigger?.Invoke();
        }

        private void OnDestroy()
        {
            _animation?.Destroy();
        }
    }
}