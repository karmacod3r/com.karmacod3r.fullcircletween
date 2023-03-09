using System;
using System.Runtime.CompilerServices;
using FullCircleTween.Core;
using UnityEngine;

namespace FullCircleTween.Extensions
{
    public static class ComponentExtension
    {
        public static Tween<float> To(this Component target, Func<float> getter, Action<float> setter, float toValue, float duration, [CallerMemberName] string memberName = "")
            => Tween<float>.To(target, getter, setter, toValue, duration, memberName);

        public static Tween<int> To(this Component target, Func<int> getter, Action<int> setter, int toValue, float duration, [CallerMemberName] string memberName = "")
            => Tween<int>.To(target, getter, setter, toValue, duration, memberName);

        public static Tween<double> To(this Component target, Func<double> getter, Action<double> setter, double toValue, float duration, [CallerMemberName] string memberName = "")
            => Tween<double>.To(target, getter, setter, toValue, duration, memberName);

        public static Tween<bool> To(this Component target, Func<bool> getter, Action<bool> setter, bool toValue, float duration, [CallerMemberName] string memberName = "")
            => Tween<bool>.To(target, getter, setter, toValue, duration, memberName);

        public static Tween<Vector2> To(this Component target, Func<Vector2> getter, Action<Vector2> setter, Vector2 toValue, float duration, [CallerMemberName] string memberName = "")
            => Tween<Vector2>.To(target, getter, setter, toValue, duration, memberName);

        public static Tween<Vector3> To(this Component target, Func<Vector3> getter, Action<Vector3> setter, Vector3 toValue, float duration, [CallerMemberName] string memberName = "")
            => Tween<Vector3>.To(target, getter, setter, toValue, duration, memberName);

        public static Tween<Vector4> To(this Component target, Func<Vector4> getter, Action<Vector4> setter, Vector4 toValue, float duration, [CallerMemberName] string memberName = "")
            => Tween<Vector4>.To(target, getter, setter, toValue, duration, memberName);

        public static Tween<Quaternion> To(this Component target, Func<Quaternion> getter, Action<Quaternion> setter, Quaternion toValue, float duration, [CallerMemberName] string memberName = "")
            => Tween<Quaternion>.To(target, getter, setter, toValue, duration, memberName);
        
        public static Tween<Color> To(this Component target, Func<Color> getter, Action<Color> setter, Color toValue, float duration, [CallerMemberName] string memberName = "")
            => Tween<Color>.To(target, getter, setter, toValue, duration, memberName);

        public static Tween<Color32> To(this Component target, Func<Color32> getter, Action<Color32> setter, Color32 toValue, float duration, [CallerMemberName] string memberName = "")
            => Tween<Color32>.To(target, getter, setter, toValue, duration, memberName);
        
        public static Tween<Vector3> SlerpTo(this Component target, Func<Vector3> getter, Action<Vector3> setter, Vector3 toValue, float duration, [CallerMemberName] string memberName = "")
            => Tween<Vector3>.SlerpTo(target, getter, setter, toValue, duration, memberName);
        
        public static Tween<Quaternion> SlerpTo(this Component target, Func<Quaternion> getter, Action<Quaternion> setter, Quaternion toValue, float duration, [CallerMemberName] string memberName = "")
            => Tween<Quaternion>.SlerpTo(target, getter, setter, toValue, duration, memberName);
    }
}