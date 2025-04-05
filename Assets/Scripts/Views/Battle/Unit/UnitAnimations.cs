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

        public void UpdateHealth(UnitHitData arg0)
        {
            _animationPlayer.Play(arg0.unit.HealthInfo.CurrentHealth > arg0.oldHealth
                ? AnimationType.Healed
                : AnimationType.Hurt);
        }

        public void Attack(UnitAttackData arg0, Vector3 worldPos, Action onLaunched)
        {
            System.Action weapon;
            AnimationType animationType;
            if (arg0.needLos)
            {
                weapon = () =>
                {
                    _weapon.WeaponShoot(new Vector3(arg0.direction.x, 0, arg0.direction.y));
                    onLaunched?.Invoke();
                };

                animationType = AnimationType.Shoot;
            }
            else
            {
                weapon = () => _weapon.Cast(worldPos);
                animationType = AnimationType.Cast;
            }

            _animationPlayer.Play(animationType, false, weapon);
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