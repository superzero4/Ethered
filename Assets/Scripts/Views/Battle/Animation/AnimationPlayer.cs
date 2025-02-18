using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

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
            _animation = new AnimationSystem(_animationList[AnimationType.Idle], _animator, this);
        }

        private IEnumerator Start()
        {
            yield break;//Testing animations
            while (true)
            {
                Play(new AnimationPlayData(AnimationType.Attack, false).Append(
                    new AnimationPlayData(AnimationType.Idle, true)));
                yield return new WaitForSeconds(1);
            }
        }

        [Obsolete("Use Play(AnimationType) directly instead, queuing isn't working yet, will possibly be implemented if we need to queue animations from this call but for now it's not needed, it will play one shot and go back to defaultAnimation")]
        public void Play(AnimationPlayData toPlay, PositionIndexer? direction = null)
        {
            Play(toPlay.Type, direction);
            return;
            if (toPlay.OnEnd != null)
            {
                StartCoroutine(WaitForAnimationEnd(toPlay.OnEnd));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="direction">Not used yet, in case we wanna fine grain animations</param>
        public void Play(AnimationType type, PositionIndexer? direction = null)
        {
            var clip = _animationList[type];
            _animation.PlayOneShot(clip);
        }

        private IEnumerator WaitForAnimationEnd(AnimationPlayData after)
        {
            //yield return new WaitWhile(() => _animation.isPlaying);
            yield return null;
            Play(after);
        }
    }
}