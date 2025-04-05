using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Views.Battle.Animation;
using Random = UnityEngine.Random;

namespace Views.Battle
{
    public class UnitSkin : MonoBehaviour
    {
        [SerializeField] private AnimationPlayer _animationPlayer;
        [SerializeField] private SkinnedMeshRenderer[] _helmets;
        [SerializeField] private SkinnedMeshRenderer _helmet;
        [SerializeField] private Renderer[] _renderers;
        private const int _highlightMaterialIndex = 0;
        private const int _highlightMaterialIndexHelmet = 1;
        [SerializeField] private WeaponView[] _weapons;
        [SerializeField] private int _weaponIndex;
        public WeaponView Weapon => _weapons[_weaponIndex];
        //private WeaponType _weaponType;
        public enum WeaponType
        {
            None=0,
            Pistol=1,
        }   

        public AnimationPlayer AnimationPlayer
        {
            get { return _animationPlayer; }
            set { _animationPlayer = value; }
        }

        private void Awake()
        {
            //_helmetIndex = ;
            //_weaponType = WeaponType.None;
        }
        public void SetRandomSkin()
        {
            var weapons = Enum.GetValues(typeof(WeaponType));
            SetSkin(Random.Range(0, _helmets.Length), Random.ColorHSV(), weapons.Length > 0 ? (WeaponType)weapons.GetValue(Random.Range(0, weapons.Length)) : WeaponType.None);
        }

        public void SetSkin(Color color)
        {
            SetSkin(UnityEngine.Random.Range(0, _helmets.Length), color, WeaponType.Pistol);
        }

        public void SetSkin(int helmetIndex, Color color, WeaponType weapon)
        {
            _weaponIndex = (int)weapon;
            Assert.IsTrue(_helmets != null && _helmets.Length > 0, "No helmets assigned");
            Assert.IsTrue(helmetIndex >= 0 && helmetIndex < _helmets.Length, "Invalid helmet index");
            for (int i = 0; i < _helmets.Length; i++)
            {
                var h = _helmets[i];
                if (i == helmetIndex)
                {
                    _helmet = h;
                    h.gameObject.SetActive(true);
                    h.materials[_highlightMaterialIndexHelmet].color = color;
                }
                else
                {
                    h.gameObject.SetActive(false);
                }
            }
            if (_renderers != null)
            {
                for (int i = 0; i < _renderers.Length; i++)
                {
                    _renderers[i].materials[_highlightMaterialIndex].color = color;
                }
            }
            if (_weapons != null)
            {
                for (int i = 0; i < _weapons.Length; i++)
                {
                    var w = _weapons[i];
                    w.Root.gameObject.SetActive(i == _weaponIndex);
                    //if (i == (int)weapon)
                    //{
                    //    w.materials[_highlightMaterialIndex].color = color;
                    //}
                }
            }
        }

        public void ToggleVisibility(bool state)
        {
            foreach (var renderer in _renderers)
                renderer.enabled = state;
           _helmet.enabled = state;
        }
    }
}