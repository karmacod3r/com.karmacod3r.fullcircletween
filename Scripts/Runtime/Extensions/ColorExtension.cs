using UnityEngine;

namespace FullCircleTween.Extensions
{
    public static class RgbExtension
    {
        public static Color SetRgb(this Color color, Color value)
        {
            color.r = value.r;
            color.g = value.g;
            color.b = value.b;

            return color;
        }
        
        public static Color SetAlpha(this Color target, float alpha)
        {
            target.a = alpha;
            
            return target;
        }
    }
}