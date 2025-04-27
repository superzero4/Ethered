using System;
using Common.Events.Combat;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Views.Battle.Animation;

namespace Views.Battle
{
    public class UnitAnimations : MonoBehaviour
    {
        [FormerlySerializedAs("_rotationSpeed")] [Header("Settings")] [SerializeField, Range(0.001f, 10f)]
        private float _rotationTime = 0.5f;

        [FormerlySerializedAs("_moveSpeed")] [SerializeField, Range(0.001f, 4f)]
        private float _moveTime = 0.5f;

        [SerializeField, Range(0.001f, 4f)] private float _deathTime = 0.5f;

        [SerializeField, ReadOnly] private UnitSkin _skin;
        public AnimationPlayer _animationPlayer => _skin.AnimationPlayer;
        private WeaponView _weapon => _skin.Weapon;
        public float RotationTime => _rotationTime;
        public float MoveTime => _moveTime;

        public void Init(UnitSkin skin)
        {
            _skin = skin;
            _animationPlayer.Play(AnimationType.Idle, true);
        }

        public void UpdateHealth(UnitHitData arg0, Transform root)
        {
            AnimationType val;
            var curr = arg0.unit.HealthInfo.CurrentHealth;
            Action onComplete = null;
            bool backToIdle = true;
            if (curr == 0)
            {
                val = AnimationType.Death;
                onComplete = () =>
                {
                    //LeanTween.scale(root.gameObject, Vector3.zero, _deathTime).setOnComplete(() => _skin.gameObject.SetActive(false));
                };
                backToIdle = false;
            }
            else if (arg0.hitValue < 0)
                val = AnimationType.Healed;
            else
                val = AnimationType.Hurt;

            _animationPlayer.Play(val, !backToIdle, onComplete);
        }

        public void Attack(UnitAttackData arg0, Vector3 worldPos, float delay, Action onArrived)
        {
            System.Action onTrigger = onArrived;
            AnimationType animationType;
            if (!arg0.isOffensive || (!arg0.needLos && !arg0.IsCloseQuarter))
            {
                onTrigger += () => _weapon.Cast(worldPos);
                animationType = AnimationType.Cast;
            }
            else if (arg0.IsCloseQuarter)
            {
                animationType = AnimationType.Attack;
            }
            else
            {
                animationType = AnimationType.Shoot;
                onTrigger += () => { _weapon.WeaponShoot(worldPos, delay); };
            }

            _animationPlayer.Play(animationType, false, onTrigger);
        }


        public void Move()
        {
            _animationPlayer.Play(AnimationType.Move, _moveTime);
        }

        public float Delay(float targMagnitude)
        {
            return targMagnitude / _weapon.ProjectileSpeed;
        }

        public void Turn(bool left)
        {
            _animationPlayer.Play(left ? AnimationType.TurnL : AnimationType.TurnR, false);
        }
    }
}