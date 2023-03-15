using FullCircleTween.Attributes;
using FullCircleTween.Core;
using FullCircleTween.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;

namespace FullCircleTween.TweenCollections
{
    [TweenCollection]
    public static class TMPTextTweenCollection
    {
        [Preserve] public static Tween<float> TweenFade(this TMP_Text target, float toValue, float duration)
            => target.To(() => target.color.a, value => target.color = target.color.SetAlpha(value), toValue, duration);
        
        [Preserve] public static Tween<Color> TweenColor(this TMP_Text target, Color toValue, float duration)
            => target.To(() => target.color, value => target.color = value, toValue, duration);
        
        [Preserve] public static Tween<Color> TweenRgb(this TMP_Text target, Color toValue, float duration)
            => target.To(() => target.color, value => target.color = target.color.SetRgb(value), toValue, duration);
    }
}