using UnityEngine;

namespace FullCircleTween.Core
{
    public static class Ease
    {
        public static float Linear(float t) => t;
        public static float InQuad(float t) => t * t;
        public static float OutQuad(float t) => t * (2f - t);
        public static float InOutQuad(float t) => t < .5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
        public static float InCubic(float t) => t * t * t;
        public static float OutCubic(float t) => (--t) * t * t + 1f;
        public static float InOutCubic(float t) => t < .5f ? 4f * t * t * t : (t - 1f) * (2f * t - 2f) * (2f * t - 2f) + 1f;
        public static float InQuart(float t) => t * t * t * t;
        public static float OutQuart(float t) => 1f - (--t) * t * t * t;
        public static float InOutQuart(float t) => t < .5f ? 8f * t * t * t * t : 1f - 8f * (--t) * t * t * t;
        public static float InQuint(float t) => t * t * t * t * t;
        public static float OutQuint(float t) => 1f + (--t) * t * t * t * t;
        public static float InOutQuint(float t) => t < .5f ? 16f * t * t * t * t * t : 1f + 16f * (--t) * t * t * t * t;
    }

    public delegate float EasingFunction(float t);
}