using System;
using System.Collections.Generic;

namespace Common.Visuals
{
    public interface IIcon
    {
        public VisualInformations VisualInformations { get; }
        public IEnumerable<IconText> IconTexts { get; }
        
        [Serializable]
        public struct IconText
        {
            public IconText(string prefix, string text) : this(prefix +" : "+ text)
            {
            }
            public IconText(string text)
            {
                this.text = text;
            }
            public string text;
        }
    }
}