using System;
using FullCircleTween.Core;
using FullCircleTween.Core.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FullCircleTween.Properties
{
    [Serializable]
    public class TweenClip
    {
        [SerializeField] public float delay;
        [SerializeField] public TweenTarget tweenTarget;
        [SerializeField] public string tweenMethodName;
        [SerializeField] public TweenClipValue toValue;
        [SerializeField] public float duration = 0.2f;

        public ITween CreateTween(Component context)
        {
            var target = tweenTarget.GetValue(context);
            if (target == null) return null;

            var tween = TweenMethodCache.CreateTween(tweenTarget.GetValue(context), tweenMethodName, toValue, duration);
            return tween?.SetDelay(delay);
        }

        public static TweenClip Create<T>(Component target, string tweenMethodName, T toValue, float duration, float delay = 0f)
        {
            return new TweenClip
            {
                tweenTarget = new TweenTarget(target),
                tweenMethodName = tweenMethodName,
                delay = delay,
                duration = duration,
                toValue = new TweenClipValue(toValue)
            };
        }

        public object GetCurrentTweenValue(Component context)
        {
            var tween = CreateTween(context);
            var value = tween?.GetCurrentValue();
            tween.Kill();
            
            return value;
        }
    }
}