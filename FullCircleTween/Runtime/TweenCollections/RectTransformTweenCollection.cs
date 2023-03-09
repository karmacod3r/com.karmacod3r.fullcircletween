using FullCircleTween.Attributes;
using FullCircleTween.Core;
using FullCircleTween.Extensions;
using UnityEngine;
using UnityEngine.Scripting;

namespace FullCircleTween.TweenCollections
{
    [TweenCollection]
    public static class RectTransformTweenCollection
    {
        [Preserve] public static Tween<Vector2> TweenAnchoredPosition(this RectTransform target, Vector2 toValue, float duration)
            => target.To(() => target.anchoredPosition, value => target.anchoredPosition = value, toValue, duration);
        
        [Preserve] public static Tween<Vector2> TweenSizeDelta(this RectTransform target, Vector2 toValue, float duration)
            => target.To(() => target.sizeDelta, value => target.sizeDelta = value, toValue, duration);
        
        [Preserve] public static Tween<Vector2> TweenPivot(this RectTransform target, Vector2 toValue, float duration)
            => target.To(() => target.pivot, value => target.pivot = value, toValue, duration);
        
        [Preserve] public static Tween<Vector2> TweenAnchorMin(this RectTransform target, Vector2 toValue, float duration)
            => target.To(() => target.anchorMin, value => target.anchorMin = value, toValue, duration);
        
        [Preserve] public static Tween<Vector2> TweenAnchorMax(this RectTransform target, Vector2 toValue, float duration)
            => target.To(() => target.anchorMax, value => target.anchorMax = value, toValue, duration);
    }
}