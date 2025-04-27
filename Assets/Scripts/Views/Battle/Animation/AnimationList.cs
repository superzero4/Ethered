using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Views.Battle.Animation
{
    public enum AnimationType
    {
        Idle = 0,
        Move = 1,
        Attack = 2,
        Hurt = 3,
        Death = 4,
        Healed = 5,
        Shoot = 6,
        TurnL = 7,
        TurnR = 8,
        Cast = 9,
        Cancel = 10,
        Celebrate = 11,
    }

    [CreateAssetMenu(fileName = "AnimationList", menuName = "AnimationList", order = 0)]
    public class AnimationList : ScriptableObject
    {
        [Serializable]
        public struct AnimationWithTrigger
        {
            [SerializeField] private bool _disable;
            [SerializeField] private AnimationClip _clip;
            [SerializeField, Range(0, 1f)] private float _triggerTime;

            public AnimationWithTrigger(AnimationClip clip, float triggerTime = 0f, bool disable = false)
            {
                _clip = clip;
                _triggerTime = triggerTime;
                this._disable = disable;
            }

            public AnimationClip Clip => _clip;

            public float TriggerTime => _triggerTime;

            public bool Disable => _disable;
        }

        [Serializable]
        private struct AnimationKV
        {
            [SerializeField] private AnimationType _type;
            [SerializeField] private List<AnimationWithTrigger> _animations;

            public AnimationKV(AnimationType type, List<AnimationWithTrigger> animations)
            {
                _type = type;
                _animations = animations;
            }

            public AnimationType Type => _type;
            public List<AnimationWithTrigger> Animations => _animations;
        }

        [InfoBox(
            "This will be collapsed to a dictionary on first access, even if two KV has same keys, they will be merged into the same list, consider using helper function to load animation matching a given regex")]
        [SerializeField]
        private List<AnimationKV> _animationsList;

        private Dictionary<AnimationType, List<AnimationWithTrigger>> _animations;

        public AnimationWithTrigger this[AnimationType t]
        {
            get
            {
                if (_animations == null || _animations.Count == 0 ||
                    !_animations.ContainsKey(t))
                    BuildDictionnary();
                if (!_animations.ContainsKey(t))
                {
                    Debug.LogError(
                        $"Animation type {t} not found in list {this.name}, returning null animation, consider fixing it");
                    return default;
                }

                var l = _animations[t].Where(t => !t.Disable).ToArray();
                return l[UnityEngine.Random.Range(0, l.Length)];
            }
        }

        private void OnValidate()
        {
            BuildDictionnary();
        }

        private void BuildDictionnary()
        {
            _animations = new();
            foreach (var kv in _animationsList)
            {
                if (_animations.ContainsKey(kv.Type))
                    _animations[kv.Type].AddRange(kv.Animations);
                else
                    _animations.Add(kv.Type, new List<AnimationWithTrigger>(kv.Animations));
            }
        }
#if UNITY_EDITOR
        [Header("Fill helper")] [SerializeField]
        private string _search;

        [SerializeField] private AnimationType _typeToFillWithSearch;

        private const string root =
            "Assets/External/Character/RPG Character Animation Pack - ExplosiveLLC/Animations";

        [Button("Fill with search", EButtonEnableMode.Editor)]
        public void FillWithSearch()
        {
            var element = new AnimationKV(_typeToFillWithSearch, new List<AnimationWithTrigger>());
            foreach (var guid in AssetDatabase.FindAssets("t:AnimationClip", new[] { root }))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var anim = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                Regex regex = new Regex(_search);
                if (anim != null && regex.IsMatch(anim.name))
                    element.Animations.Add(new AnimationWithTrigger(anim));
            }

            if (element.Animations.Count > 0)
                _animationsList.Add(element);
        }
#endif
    }
}