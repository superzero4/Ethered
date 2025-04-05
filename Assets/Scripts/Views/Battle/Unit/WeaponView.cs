using UnityEngine;

namespace Views.Battle
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        [SerializeField] private Transform _muzzle;
        [SerializeField] private ParticleSystem _particle;

        public Transform Root => _root ?? transform;

        public void WeaponShoot(Vector3 lookAt)
        {
            if(_particle == null)
            {
                return;
            }

            _particle.transform.parent = null;
            _particle.transform.position = _muzzle.position;
            _particle.transform.rotation = _muzzle.rotation;
            _particle.Play();
        }
    }
}