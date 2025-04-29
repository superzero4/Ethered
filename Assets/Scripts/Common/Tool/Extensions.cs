using UnityEngine;

namespace Common.Tool
{
    public static class Extensions
    {
        public static Color Alpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}