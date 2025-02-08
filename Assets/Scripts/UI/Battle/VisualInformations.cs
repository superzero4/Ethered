using System;
using UnityEngine;

namespace UI.Battle
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
        }

        public string Name
        {
            get => _name;
        }

        public string Description
        {
            get => _description;
        }
    }
}