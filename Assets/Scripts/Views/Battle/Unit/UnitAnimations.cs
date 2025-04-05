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
        [FormerlySerializedAs("_rotationSpeed")] [Header("Settings")] [SerializeField, Range(0.001f, 1f)]
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
            _animationPlayer.Play(AnimationType.Idle, () => false);
        }

        public void UpdateHealth(UnitHitData arg0)
        {
            _animationPlayer.Play(
                arg0.unit.HealthInfo.CurrentHealth > arg0.oldHealth ? AnimationType.Healed : AnimationType.Hurt, null,
                null,
                arg0.direction);
        }

        public void Attack(UnitAttackData arg0, Action onLaunched)
        {
            System.Action weapon = () =>
            {
                _weapon.WeaponShoot(new Vector3(arg0.direction.x, 0, arg0.direction.y));
                onLaunched?.Invoke();
            };
            _animationPlayer.Play(AnimationType.Shoot, null, weapon, arg0.direction);
        }


        public void Move(Func<bool> func)
        {
            _animationPlayer.Play(AnimationType.Move, func, null);
        }

        public float Delay(float targMagnitude)
        {
            return targMagnitude / _weapon.ProjectileSpeed;
        }
    }
}