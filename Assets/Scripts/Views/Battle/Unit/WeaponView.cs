using System;
using System.Collections;
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
            public float spreadAngle;
            public Color color;
        }

        private void Awake()
        {
            if (_particle != null)
                _particle.transform.parent = null;
        }

        [SerializeField] private Transform _root;
        [SerializeField] private Transform _muzzle;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private ParticleSettings _shoot;
        [Range(0, 10)] public float height;
        [SerializeField] private ParticleSettings _cast;

        public Transform Root => _root ?? transform;

        public float ProjectileSpeed => _shoot.speed;

        private void Apply(ParticleSettings settings)
        {
            var main = _particle.main;
            main.startSpeed = settings.speed;
            if (settings.color != default)
                main.startColor = settings.color;
            var emission = _particle.emission;
            main.startLifetime = settings.lifetime;
            emission.SetBurst(0, new ParticleSystem.Burst(0, settings.nbParticles));
        }

        public void WeaponShoot(Vector3 targetPos, float lifeTimeOverride)
        {
            var settings = _shoot;
            settings.lifetime = lifeTimeOverride;
            Apply(settings);
            _particle.transform.position = _muzzle.position;
            _particle.transform.rotation = Quaternion.LookRotation(targetPos - _muzzle.position);
            _particle.Play();
        }

        private IEnumerator StopAfter(float delay)
        {
            yield return new WaitForSeconds(delay);
            _particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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