using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Views.Battle.Animation;

namespace Views.Battle
{
    public class UnitSkin : MonoBehaviour
    {
        [SerializeField] private AnimationPlayer _animationPlayer;
        [SerializeField] private SkinnedMeshRenderer[] _helmets;
        [SerializeField] private SkinnedMeshRenderer _helmet;
        private int _helmetIndex;
        [SerializeField] private SkinnedMeshRenderer[] _renderers;
        private int _highlightMaterialIndex = 0;
        private int _highlightMaterialIndexHelmet = 1;

        public AnimationPlayer AnimationPlayer
        {
            get { return _animationPlayer; }
            set { _animationPlayer = value; }
        }

        private void Awake()
        {
            _helmetIndex = UnityEngine.Random.Range(0, _helmets.Length);
        }
        public void SetRandomSkin()
        {
            SetSkin(Random.Range(0, _helmets.Length), Random.ColorHSV());
        }

        public void SetSkin(Color color)
        {
            SetSkin(_helmetIndex, color);
        }

        public void SetSkin(int helmetIndex, Color color)
        {
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
        }

        public void ToggleVisibility(bool state)
        {
            foreach (var renderer in _renderers)
                renderer.enabled = state;
           _helmet.enabled = state;
        }
    }
}