using System;
using NaughtyAttributes;
using UnityEngine;

namespace Common.Visuals
{
    [Serializable]
    public struct VisualInformations
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Color _color;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        
        
        public Sprite Sprite
        {
            get => _sprite;
        }

        public Color Color
        {
            get => _color;
            set => _color = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Description
        {
            get => _description;
        }
        
        [Button]
        private void ColorMaxAlpha()
        {
            _color.a = 1;
        }
        //Dynamic default that will be use as a real cohrent default when fallbacking from a null or C# default
        public static VisualInformations Default;

    }
}