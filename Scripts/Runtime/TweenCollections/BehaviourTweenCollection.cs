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
        // TODO: Recording enabled state doesn't work, because changing enabled state in inspector triggers TweenStateMachineEditor.onDisable
        [TweenPropertyPath("m_Enabled")]
        [Preserve] public static Tween<bool> TweenEnabled(this Behaviour target, bool toValue, float duration)
            => target.To(() => target.enabled, value => target.enabled = value, toValue, duration);
    }
}