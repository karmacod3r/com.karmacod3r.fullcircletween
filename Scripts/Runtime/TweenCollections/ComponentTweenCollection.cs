using FullCircleTween.Attributes;
using FullCircleTween.Core;
using FullCircleTween.Extensions;
using UnityEngine;
using UnityEngine.Scripting;

namespace FullCircleTween.TweenCollections
{
    [TweenCollection]
    public static class ComponentTweenCollection
    {
        [Preserve] public static Tween<bool> TweenGameObjectActive(this Component target, bool toValue, float duration)
            => target != null && target.gameObject != null ? target.To(() => target.gameObject.activeSelf, value => target.gameObject.SetActive(value), toValue, duration) : null;
    }
}