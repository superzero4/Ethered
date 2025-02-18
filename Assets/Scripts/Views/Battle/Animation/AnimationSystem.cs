using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Views.Battle.Animation
{
    public class AnimationSystem
    {
        private MonoBehaviour runner;
        PlayableGraph playableGraph;
        private readonly AnimationMixerPlayable animationMixer;

        AnimationClipPlayable oneShotPlayable;

        Coroutine blendInHandle;
        Coroutine blendOutHandle;

        public AnimationSystem(AnimationClip idleClip, Animator animator, MonoBehaviour runner)
        {
            this.runner = runner;
            playableGraph = PlayableGraph.Create("AnimationSystem");

            AnimationPlayableOutput playableOutput =
                AnimationPlayableOutput.Create(playableGraph, "Animation", animator);

            animationMixer = AnimationMixerPlayable.Create(playableGraph, 2);
            playableOutput.SetSourcePlayable(animationMixer);

            playableGraph.GetRootPlayable(0).SetInputWeight(0, 1f);

            AnimationClipPlayable idlePlayable = AnimationClipPlayable.Create(playableGraph, idleClip);

            idlePlayable.GetAnimationClip().wrapMode = WrapMode.Loop;

            animationMixer.ConnectInput(0, idlePlayable, 0);

            playableGraph.Play();
        }

        public void PlayOneShot(AnimationClip oneShotClip)
        {
            if (oneShotPlayable.IsValid() && oneShotPlayable.GetAnimationClip() == oneShotClip) return;

            InterruptOneShot();
            oneShotPlayable = AnimationClipPlayable.Create(playableGraph, oneShotClip);
            animationMixer.ConnectInput(1, oneShotPlayable, 0);
            animationMixer.SetInputWeight(1, 1f);

            // Calculate blendDuration as 10% of clip length,
            // but ensure that it's not less than 0.1f or more than half the clip length
            float blendDuration = Mathf.Clamp(oneShotClip.length * 0.1f, 0.1f, oneShotClip.length * 0.5f);

            BlendIn(blendDuration);
            BlendOut(blendDuration, oneShotClip.length - blendDuration);
        }

        void BlendIn(float duration)
        {
            blendInHandle = runner.StartCoroutine(Blend(duration, blendTime =>
            {
                float weight = Mathf.Lerp(1f, 0f, blendTime);
                animationMixer.SetInputWeight(0, weight);
                animationMixer.SetInputWeight(1, 1f - weight);
            }));
        }

        void BlendOut(float duration, float delay)
        {
            blendOutHandle = runner.StartCoroutine(Blend(duration, blendTime =>
            {
                float weight = Mathf.Lerp(0f, 1f, blendTime);
                SetRelativeWeights(weight);
            }, delay, DisconnectOneShot));
        }

        IEnumerator Blend(float duration, Action<float> blendCallback, float delay = 0f,
            Action finishedCallback = null)
        {
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }

            float blendTime = 0f;
            while (blendTime < 1f)
            {
                blendTime += Time.deltaTime / duration;
                blendCallback(blendTime);
                yield return blendTime;
            }

            blendCallback(1f);

            finishedCallback?.Invoke();
        }

        void InterruptOneShot()
        {
            if (blendInHandle != null)
                runner.StopCoroutine(blendInHandle);
            if (blendOutHandle != null)
                runner.StopCoroutine(blendOutHandle);

            SetRelativeWeights(1f);

            if (oneShotPlayable.IsValid())
            {
                DisconnectOneShot();
            }
        }

        private void SetRelativeWeights(float weightOfFirstInput)
        {
            animationMixer.SetInputWeight(0, weightOfFirstInput);
            animationMixer.SetInputWeight(1, 1 - weightOfFirstInput);
        }

        void DisconnectOneShot()
        {
            animationMixer.DisconnectInput(1);
            playableGraph.DestroyPlayable(oneShotPlayable);
        }

        public void Destroy()
        {
            if (playableGraph.IsValid())
            {
                playableGraph.Destroy();
            }
        }
    }
}