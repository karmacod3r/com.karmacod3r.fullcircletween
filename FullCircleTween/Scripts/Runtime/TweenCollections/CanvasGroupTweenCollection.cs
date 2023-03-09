using FullCircleTween.Attributes;
using FullCircleTween.Core;
using FullCircleTween.Extensions;
using UnityEngine;
using UnityEngine.Scripting;

namespace FullCircleTween.TweenCollections
{
    [TweenCollection]
    public static class CanvasGroupTweenCollection
    {
        [Preserve] public static Tween<float> TweenFade(this CanvasGroup target, float toValue, float duration)
            => target.To(() => target.alpha, (value) => target.alpha = value, toValue, duration);
    }
}