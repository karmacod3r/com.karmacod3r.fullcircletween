using UnityEngine;

namespace FullCircleTween.Core
{
    public static class LerpMethods
    {
        public static float Lerp(float a, float b, float t) => a + (b - a) * t;
        public static int Lerp(int a, int b, float t) => a + Mathf.FloorToInt((b - a) * t);
        public static double Lerp(double a, double b, float t) => a + (b - a) * t;
        public static bool Lerp(bool a, bool b, float t) => t >= 1.0 && b;
        public static Vector2 Lerp(Vector2 a, Vector2 b, float t) => new Vector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
        public static Vector4 Lerp(Vector4 a, Vector4 b, float t) => new Vector4(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t, a.w + (b.w - a.w) * t);
        public static Quaternion Lerp(Quaternion a, Quaternion b, float t) => Quaternion.LerpUnclamped(a, b, t);
        public static Color Lerp(Color a, Color b, float t) => Color.LerpUnclamped(a, b, t);
        public static Color32 Lerp(Color32 a, Color32 b, float t) => Color.LerpUnclamped(a, b, t);
        
        public static Vector3 Slerp(Vector3 a, Vector3 b, float t) => Vector3.SlerpUnclamped(a, b, t);
        public static Quaternion Slerp(Quaternion a, Quaternion b, float t) => Quaternion.SlerpUnclamped(a, b, t);
    }
}