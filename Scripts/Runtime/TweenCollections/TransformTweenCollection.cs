using FullCircleTween.Attributes;
using FullCircleTween.Core;
using FullCircleTween.Extensions;
using UnityEngine;
using UnityEngine.Scripting;

namespace FullCircleTween.TweenCollections
{
    [TweenCollection]
    public static class TransformTweenCollection
    {
        [Preserve] public static Tween<Vector3> TweenPosition(this Transform target, Vector3 toValue, float duration)
            => target.To(() => target.position, value => target.position = value, toValue, duration);

        [Preserve] public static Tween<float> TweenPositionX(this Transform target, float toValue, float duration)
            => target.To(() => target.position.x, target.SetPositionX, toValue, duration);

        [Preserve] public static Tween<float> TweenPositionY(this Transform target, float toValue, float duration)
            => target.To(() => target.position.y, target.SetPositionY, toValue, duration);

        [Preserve] public static Tween<float> TweenPositionZ(this Transform target, float toValue, float duration)
            => target.To(() => target.position.z, target.SetPositionZ, toValue, duration);

        [Preserve] public static Tween<Vector3> TweenLocalPosition(this Transform target, Vector3 toValue, float duration)
            => target.To(() => target.localPosition, value => target.localPosition = value, toValue, duration);

        [Preserve] public static Tween<float> TweenLocalPositionX(this Transform target, float toValue, float duration)
            => target.To(() => target.localPosition.x, target.SetLocalPositionX, toValue, duration);

        [Preserve] public static Tween<float> TweenLocalPositionY(this Transform target, float toValue, float duration)
            => target.To(() => target.localPosition.y, target.SetLocalPositionY, toValue, duration);

        [Preserve] public static Tween<float> TweenLocalPositionZ(this Transform target, float toValue, float duration)
            => target.To(() => target.localPosition.z, target.SetLocalPositionZ, toValue, duration);

        [Preserve] public static Tween<float> TweenScale(this Transform target, float toValue, float duration)
            => target.To(() => target.localScale.x, target.SetScale, toValue, duration);

        [Preserve] public static Tween<Vector3> TweenScale(this Transform target, Vector3 toValue, float duration)
            => target.To(() => target.localScale, value => target.localScale = value, toValue, duration);

        [Preserve] public static Tween<float> TweenScaleX(this Transform target, float toValue, float duration)
            => target.To(() => target.localScale.x, target.SetScaleX, toValue, duration);

        [Preserve] public static Tween<float> TweenScaleY(this Transform target, float toValue, float duration)
            => target.To(() => target.localScale.y, target.SetScaleY, toValue, duration);

        [Preserve] public static Tween<float> TweenScaleZ(this Transform target, float toValue, float duration)
            => target.To(() => target.localScale.z, target.SetScaleZ, toValue, duration);

        [Preserve] public static Tween<Vector3> TweenLocalRotation(this Transform target, Vector3 toValue, float duration)
            => target.To(() => target.localRotation.eulerAngles, value => target.localRotation = Quaternion.Euler(value), toValue, duration);

        [Preserve] public static Tween<float> TweenLocalRotationX(this Transform target, float toValue, float duration)
            => target.To(() => target.rotation.eulerAngles.x, target.SetLocalRotationX, toValue, duration);

        [Preserve] public static Tween<float> TweenLocalRotationY(this Transform target, float toValue, float duration)
            => target.To(() => target.rotation.eulerAngles.y, target.SetLocalRotationY, toValue, duration);

        [Preserve] public static Tween<float> TweenLocalRotationZ(this Transform target, float toValue, float duration)
            => target.To(() => target.rotation.eulerAngles.z, target.SetLocalRotationZ, toValue, duration);

        [Preserve] public static Tween<Vector3> TweenRotation(this Transform target, Vector3 toValue, float duration)
            => target.To(() => target.rotation.eulerAngles, value => target.rotation = Quaternion.Euler(value), toValue, duration);

        [Preserve] public static Tween<float> TweenRotationX(this Transform target, float toValue, float duration)
            => target.To(() => target.rotation.eulerAngles.x, target.SetRotationX, toValue, duration);

        [Preserve] public static Tween<float> TweenRotationY(this Transform target, float toValue, float duration)
            => target.To(() => target.rotation.eulerAngles.y, target.SetRotationY, toValue, duration);

        [Preserve] public static Tween<float> TweenRotationZ(this Transform target, float toValue, float duration)
            => target.To(() => target.rotation.eulerAngles.z, target.SetRotationZ, toValue, duration);
    }
}