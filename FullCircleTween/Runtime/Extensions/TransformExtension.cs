using FullCircleTween.Core;
using UnityEngine;

namespace FullCircleTween.Extensions
{
    public static class TransformExtension
    {
        public static void SetPositionX(this Transform target, float value)
        {
            var p = target.position;
            p.x = value;
            target.position = p;
        }

        public static void SetPositionY(this Transform target, float value)
        {
            var p = target.position;
            p.y = value;
            target.position = p;
        }

        public static void SetPositionZ(this Transform target, float value)
        {
            var p = target.position;
            p.z = value;
            target.position = p;
        }
        
        public static void SetLocalPositionX(this Transform target, float value)
        {
            var p = target.localPosition;
            p.x = value;
            target.localPosition = p;
        }

        public static void SetLocalPositionY(this Transform target, float value)
        {
            var p = target.localPosition;
            p.y = value;
            target.localPosition = p;
        }

        public static void SetLocalPositionZ(this Transform target, float value)
        {
            var p = target.localPosition;
            p.z = value;
            target.localPosition = p;
        }
        
        public static void SetScale(this Transform target, float value)
        {
            target.localScale = new Vector3(value, value, value);
        }

        public static void SetScaleX(this Transform target, float value)
        {
            var p = target.localScale;
            p.x = value;
            target.localScale = p;
        }

        public static void SetScaleY(this Transform target, float value)
        {
            var p = target.localScale;
            p.y = value;
            target.localScale = p;
        }

        public static void SetScaleZ(this Transform target, float value)
        {
            var p = target.localScale;
            p.z = value;
            target.localScale = p;
        }

        public static void SetRotationX(this Transform target, float value)
        {
            var p = target.rotation.eulerAngles;
            p.x = value;
            target.eulerAngles = p;
        }

        public static void SetRotationY(this Transform target, float value)
        {
            var p = target.rotation.eulerAngles;
            p.y = value;
            target.eulerAngles = p;
        }

        public static void SetRotationZ(this Transform target, float value)
        {
            var p = target.rotation.eulerAngles;
            p.z = value;
            target.eulerAngles = p;
        }

        public static void SetLocalRotationX(this Transform target, float value)
        {
            var p = target.localRotation.eulerAngles;
            p.x = value;
            target.eulerAngles = p;
        }

        public static void SetLocalRotationY(this Transform target, float value)
        {
            var p = target.localRotation.eulerAngles;
            p.y = value;
            target.eulerAngles = p;
        }

        public static void SetLocalRotationZ(this Transform target, float value)
        {
            var p = target.localRotation.eulerAngles;
            p.z = value;
            target.eulerAngles = p;
        }
    }
}