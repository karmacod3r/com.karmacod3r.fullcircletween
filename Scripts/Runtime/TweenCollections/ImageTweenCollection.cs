using FullCircleTween.Attributes;
using FullCircleTween.Core;
using FullCircleTween.Extensions;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

namespace FullCircleTween.TweenCollections
{
    [TweenCollection]
    public static class ImageTweenCollection
    {
        // TODO: enable recording of subfields. E.g. Image.color.a
        [Preserve] public static Tween<float> TweenFade(this Image target, float toValue, float duration)
            => target.To(() => target.color.a, (value) => target.color = target.color.SetAlpha(value), toValue, duration);
        
        // TODO: Color picker drag doesn't update the recording
        [TweenPropertyPath("m_Color")]
        [Preserve] public static Tween<Color> TweenColor(this Image target, Color toValue, float duration)
            => target.To(() => target.color, value => target.color = value, toValue, duration);
        
        [Preserve] public static Tween<Color> TweenRgb(this Image target, Color toValue, float duration)
            => target.To(() => target.color, value => target.color = target.color.SetRgb(value), toValue, duration);
    }
}