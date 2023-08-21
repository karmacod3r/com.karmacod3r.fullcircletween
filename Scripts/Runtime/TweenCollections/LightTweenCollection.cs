using FullCircleTween.Attributes;
using FullCircleTween.Core;
using UnityEngine;
using UnityEngine.Scripting;

namespace FullCircleTween.TweenCollections
{
    [TweenCollection]
    public static class LightTweenCollection
    {
        [TweenPropertyPath("m_Color")]
        [Preserve] public static Tween<Color> TweenColor(this Light target, Color toValue, float duration)
            => Tween<Color>.To(target, () => target.color, value => target.color = value, toValue, duration);

        [TweenPropertyPath("m_Intensity")]
        [Preserve] public static Tween<float> TweenIntensity(this Light target, float toValue, float duration)
            => Tween<float>.To(target, () => target.intensity, value => target.intensity = value, toValue, duration);
    }
}