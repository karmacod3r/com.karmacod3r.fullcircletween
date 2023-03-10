using FullCircleTween.Attributes;
using FullCircleTween.Core;
using FullCircleTween.Extensions;
using UnityEngine;
using UnityEngine.Scripting;

namespace FullCircleTween.TweenCollections
{
    [TweenCollection]
    public static class BehaviourTweenCollection
    {
        [Preserve] public static Tween<bool> TweenActive(this Behaviour target, bool toValue, float duration)
            => target.To(() => target.enabled, value => target.enabled = value, toValue, duration);
    }
}