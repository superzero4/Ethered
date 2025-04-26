using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Visuals
{
    public interface IIcon
    {
        public VisualInformations VisualInformations { get; }
        public IEnumerable<IconText> IconTexts { get; }

        public enum IconType
        {
            None = 0,
            Range = 1,
            Power = 2,
            Phase = 3,
            RelativePhase = 4,
            Distance = 5,
            Health = 6,
            Text = 7,
        }

        public static Dictionary<IconType, IconText> Icons = null;

        [Serializable]
        public struct IconText
        {
            public Sprite icon;
            public Color color;
            public bool forceExpand;
            [NonSerialized] public string text;


            public IconText(string prefix, string text) : this(prefix + " : " + text)
            {
            }

            public IconText(IconType prefix, string text, bool forceExpand = false, Color? color = null)
            {
                this = Icons[prefix];
                this.text = text;
                if (color.HasValue)
                    this.color = color.Value;
                this.forceExpand = forceExpand;
            }

            public IconText(string text)
            {
                this.text = text;
                icon = null;
                color = Color.white;
                forceExpand = false;
            }
        }
    }
}