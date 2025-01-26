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
    }
}