using System;
using NaughtyAttributes;
using UnityEngine;

namespace Common.Visuals
{
    [Serializable]
    public struct VisualInformations
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Sprite _grayScale;
        [SerializeField] private Color _color;
        [SerializeField] private string _name;
        [SerializeField] private string _description;

        public Sprite Sprite
        {
            get => _sprite;
        }

        public Sprite GrayScale
        {
            get
            {
                //We generate the grayscale sprite once when needed, stays null otherwise
                if (_grayScale == null)
                    _grayScale = GenerateGrayscale();
                return _grayScale;
            }
        }

        private Sprite GenerateGrayscale()
        {
            if (_sprite == null)
                return null;
            var spriteTexture = _sprite.texture;
            Texture2D texture = new Texture2D(spriteTexture.width, spriteTexture.height);
            texture.SetPixels(spriteTexture.GetPixels());
            for (int i = 0; i < texture.width; i++)
            {
                for (int j = 0; j < texture.height; j++)
                {
                    Color color = texture.GetPixel(i, j);
                    float gray = color.grayscale;
                    texture.SetPixel(i, j, new Color(gray, gray, gray, color.a));
                }
            }

            texture.Apply();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            return sprite;
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