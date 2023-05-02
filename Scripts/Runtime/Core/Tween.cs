using System;
using System.Linq;
using System.Threading.Tasks;
using FullCircleTween.Components;
using FullCircleTween.Core.Interfaces;
using UnityEngine;

namespace FullCircleTween.Core
{
    //TODO: add awaitable completion
    public class Tween<T> : ITween
    {
        public delegate T TweenLerpMethod(T fromValue, T toValue, float blend);

        internal string memberName;
        
        private TweenLerpMethod lerpMethod;
        internal object target;
        private Func<T> getter;
        private Action<T> setter;
        private float delay;
        private float duration;
        internal T fromValue;
        internal T toValue;
        private EasingFunction easingFunction = Ease.OutQuad;

        private bool cachedFromValue;
        private bool playing;
        private bool completed;
        private float playHeadSeconds;

        public event Action onComplete;
        private event Action thenEvent;
        
        public bool IsPlaying => playing;
        public bool Completed => completed;
        public object Target => target;
        public float PlayHeadSeconds => playHeadSeconds;
        public float Duration => duration;
        public float Delay => delay;

        private Tween(TweenLerpMethod lerpMethod, object target, Func<T> getter, Action<T> setter, T toValue, float duration, string memberName = "")
        {
            Initialize(lerpMethod, target, getter, setter, toValue, duration, memberName);
        }

        internal void Initialize(TweenLerpMethod lerpMethod, object target, Func<T> getter, Action<T> setter, T toValue, float duration, string memberName)
        {
            this.lerpMethod = lerpMethod;
            this.target = target;
            this.getter = getter;
            this.setter = setter;
            this.toValue = toValue;
            this.duration = duration;
            this.memberName = memberName;

            Play();
        }

        public void Play()
        {
            if (completed)
            {
                completed = false;
                Seek(0);
            }
            playing = true;
            cachedFromValue = false;
            TweenManager.AddTween(this);
        }

        public void Pause()
        {
            playing = false;
            TweenManager.RemoveTween(this);
        }

        public void Skip()
        {
            Seek(delay + duration);
        }

        public void Kill()
        {
            Pause();
            cachedFromValue = false;
        }

        public void Seek(float seconds)
        {
            playHeadSeconds = Mathf.Clamp(seconds, 0f, duration + delay);
            Evaluate(seconds);
        }

        public ITween Then(Action callback)
        {
            if (!completed && duration + delay > 0f)
            {
                thenEvent += callback;
            }
            else
            {
                callback();
            }

            return this;
        }

        public ITween SetEasing(EasingFunction value)
        {
            easingFunction = value;
            return this;
        }

        public ITween SetDelay(float value)
        {
            delay = value;
            return this;
        }
        
        public ITween SetFrom(T value)
        {
            fromValue = value;
            cachedFromValue = true;
            return this;
        }

        public ITween SetTarget(object value)
        {
            target = value;
            return this;
        }

        public void Evaluate(float seconds)
        {
            if (seconds < delay) return;

            if (!cachedFromValue)
            {
                cachedFromValue = true;
                fromValue = getter();
            }

            var pos = duration > 0 
                ? Mathf.Clamp01((seconds - delay) / duration) 
                : seconds >= delay ? 1 : 0;
            var blend = easingFunction(Mathf.Clamp(pos, 0f, 1f));
            setter(lerpMethod(fromValue, toValue, blend));
            
            if (playing && seconds >= duration + delay)
            {
                CompleteTween();
            }
        }

        public void Advance(float deltaSeconds)
        {
            var position = playHeadSeconds + deltaSeconds;
            Seek(position);
        }

        private void CompleteTween()
        {
            completed = true;
            onComplete?.Invoke();

            if (thenEvent != null)
            {
                thenEvent.Invoke();
                
                // remove all listeners
                foreach (var d in thenEvent.GetInvocationList())
                {
                    thenEvent -= (Action) d;
                }
            }

            TweenManager.RemoveTween(this);
        }

        public static Tween<float> To(object target, Func<float> getter, Action<float> setter, float toValue, float duration, string memberName = "")
            => Tween<float>.To(LerpMethods.Lerp, target, getter, setter, toValue, duration, memberName);

        public static Tween<int> To(object target, Func<int> getter, Action<int> setter, int toValue, float duration, string memberName = "")
            => Tween<int>.To(LerpMethods.Lerp, target, getter, setter, toValue, duration, memberName);

        public static Tween<double> To(object target, Func<double> getter, Action<double> setter, double toValue, float duration, string memberName = "")
            => Tween<double>.To(LerpMethods.Lerp, target, getter, setter, toValue, duration, memberName);

        public static Tween<bool> To(object target, Func<bool> getter, Action<bool> setter, bool toValue, float duration, string memberName = "")
            => Tween<bool>.To(LerpMethods.Lerp, target, getter, setter, toValue, duration, memberName);

        public static Tween<Vector2> To(object target, Func<Vector2> getter, Action<Vector2> setter, Vector2 toValue, float duration, string memberName = "")
            => Tween<Vector2>.To(LerpMethods.Lerp, target, getter, setter, toValue, duration, memberName);

        public static Tween<Vector3> To(object target, Func<Vector3> getter, Action<Vector3> setter, Vector3 toValue, float duration, string memberName = "")
            => Tween<Vector3>.To(LerpMethods.Lerp, target, getter, setter, toValue, duration, memberName);

        public static Tween<Vector4> To(object target, Func<Vector4> getter, Action<Vector4> setter, Vector4 toValue, float duration, string memberName = "")
            => Tween<Vector4>.To(LerpMethods.Lerp, target, getter, setter, toValue, duration, memberName);

        public static Tween<Quaternion> To(object target, Func<Quaternion> getter, Action<Quaternion> setter, Quaternion toValue, float duration, string memberName = "")
            => Tween<Quaternion>.To(LerpMethods.Lerp, target, getter, setter, toValue, duration, memberName);

        public static Tween<Color> To(object target, Func<Color> getter, Action<Color> setter, Color toValue, float duration, string memberName = "")
            => Tween<Color>.To(LerpMethods.Lerp, target, getter, setter, toValue, duration, memberName);

        public static Tween<Color32> To(object target, Func<Color32> getter, Action<Color32> setter, Color32 toValue, float duration, string memberName = "")
            => Tween<Color32>.To(LerpMethods.Lerp, target, getter, setter, toValue, duration, memberName);

        public static Tween<Vector3> SlerpTo(object target, Func<Vector3> getter, Action<Vector3> setter, Vector3 toValue, float duration, string memberName = "")
            => Tween<Vector3>.To(LerpMethods.Slerp, target, getter, setter, toValue, duration, memberName);

        public static Tween<Quaternion> SlerpTo(object target, Func<Quaternion> getter, Action<Quaternion> setter, Quaternion toValue, float duration, string memberName = "")
            => Tween<Quaternion>.To(LerpMethods.Slerp, target, getter, setter, toValue, duration, memberName);


        public static Tween<T> To(TweenLerpMethod lerpMethod, object target, Func<T> getter, Action<T> setter, T toValue, float duration, string memberName = "")
        {
            // TODO: implement pooling
            return new Tween<T>(lerpMethod, target, getter, setter, toValue, duration, memberName);
        }
    }

    public static class Tween
    {
        public static ITween DelayedCall(object target, float delay, Action callback)
        {
            var time = 0f;
            return Tween<float>.To(target, () => time, (value) => time = value, delay, delay)
                .SetEasing(Ease.Linear)
                .Then(callback);
        }
    }
}