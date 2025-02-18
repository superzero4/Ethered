using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using NaughtyAttributes;
using UnityEngine;

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

    [RequireComponent(typeof(UnityEngine.Animation))]
    public class AnimationPlayer : MonoBehaviour
    {
        [InfoBox("The animation list this character is using, specific for one character, weapon type, global...")]
        [SerializeField]
        private AnimationList _animationList;

        [SerializeField] private UnityEngine.Animation _animation;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="direction">Not used yet, in case we wanna fine grain animations</param>
        public void Play(AnimationPlayData toPlay, PositionIndexer? direction = null)
        {
            var clip = _animationList[toPlay.Type];
            _animation.clip = clip;
            _animation.wrapMode = toPlay.Loop ? WrapMode.Loop : WrapMode.Once;
            _animation.Play();
            if (toPlay.OnEnd != null)
                StartCoroutine(WaitForAnimationEnd(toPlay.OnEnd));
        }

        private IEnumerator WaitForAnimationEnd(AnimationPlayData after)
        {
            yield return new WaitWhile(() => _animation.isPlaying);
            Play(after);
        }
    }
}