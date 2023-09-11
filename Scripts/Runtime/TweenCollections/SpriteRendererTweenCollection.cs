using FullCircleTween.Attributes;
using FullCircleTween.Core;
using FullCircleTween.Extensions;
using UnityEngine;
using UnityEngine.Scripting;

namespace FullCircleTween.TweenCollections
{
    public static class SpriteRendererTweenCollection
    {
        [Preserve] public static Tween<float> TweenFade(this SpriteRenderer target, float toValue, float duration)
            => target.To(() => target.color.a, (value) => target.color = target.color.SetAlpha(value), toValue, duration);
        
        [TweenPropertyPath("m_Color")]
        [Preserve] public static Tween<Color> TweenColor(this SpriteRenderer target, Color toValue, float duration)
            => target.To(() => target.color, value => target.color = value, toValue, duration);
        
        [Preserve] public static Tween<Color> TweenRgb(this SpriteRenderer target, Color toValue, float duration)
            => target.To(() => target.color, value => target.color = target.color.SetRgb(value), toValue, duration);
    }
}