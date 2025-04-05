using System;
using UnityEngine;

namespace Views.Battle
{
    public class WeaponView : MonoBehaviour
    {
        [Serializable]
        private struct ParticleSettings
        {
            [Range(0, 1000)] public int nbParticles;
            [Range(0, 1000)] public float speed;
            [Range(0, 2f)] public float lifetime;
            public Color color;
        }

        [SerializeField] private Transform _root;
        [SerializeField] private Transform _muzzle;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private ParticleSettings _shoot;
        [Range(0, 10)] public float height;
        [SerializeField] private ParticleSettings _cast;

        public Transform Root => _root ?? transform;

        public float ProjectileSpeed => _particle.main.startSpeed.constant;

        private void Apply(ParticleSettings settings)
        {
            var main = _particle.main;
            main.startSpeed = settings.speed;
            if (settings.color != default)
                main.startColor = settings.color;
            var emission = _particle.emission;
            emission.SetBurst(0, new ParticleSystem.Burst(0, settings.nbParticles));
            main.startLifetime = settings.lifetime;
        }

        public void WeaponShoot(Vector3 lookAt)
        {
            Apply(_shoot);
            _particle.transform.parent = null;
            _particle.transform.position = _muzzle.position;
            _particle.transform.rotation = _muzzle.rotation;
            _particle.Play();
        }

        public void Cast(Vector3 target)
        {
            Apply(_cast);
            _particle.transform.parent = null;
            _particle.transform.position = target + Vector3.up * height;
            _particle.transform.rotation = Quaternion.LookRotation(Vector3.down);
            _particle.Play();
        }
    }
}