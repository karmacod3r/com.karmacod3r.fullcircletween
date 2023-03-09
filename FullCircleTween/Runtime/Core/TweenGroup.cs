using System;
using System.Collections.Generic;
using System.Linq;
using FullCircleTween.Core.Interfaces;
using UnityEngine;

namespace FullCircleTween.Core
{
    public class TweenGroup : ITween
    {
        private List<ITween> tweens = new List<ITween>();

        private object target;
        private bool playing;
        private bool completed;
        private float delay;
        private float playHeadSeconds;
        private float appendOffset;

        public bool IsPlaying => playing;
        public bool Completed => completed;
        public object Target => target;
        public float PlayHeadSeconds => playHeadSeconds;
        public float Duration
        {
            get
            {
                var duration = 0f;
                foreach (var tween in tweens)
                {
                    duration = Mathf.Max(duration, tween.Delay + tween.Duration);
                }

                return duration;
            }
        }

        public float Delay => delay;

        public event Action onComplete;
        private event Action thenEvent;

        public TweenGroup(object target, IEnumerable<ITween> tweens = null)
        {
            this.target = target;
            AddTweens(tweens);            
            Play();
        }

        public TweenGroup(IEnumerable<ITween> tweens = null)
        {
            target = this;
            AddTweens(tweens);
            Play();
        }

        public TweenGroup Add(ITween tween)
        {
            if (tween == null) return this;
            
            tween.Pause();
            tween.SetTarget(target);
            tweens.Add(tween);
            
            SortTweens();
            
            return this;
        }

        private void SortTweens()
        {
            tweens.Sort((a, b) =>
            {
                var rightA = a.Delay + a.Duration;
                var rightB = b.Delay + b.Duration;
                return rightA < rightB ? -1 : rightA == rightB ? 0 : 1;
            });
        }

        public TweenGroup Append(ITween tween)
        {
            if (tween == null) return this;
            
            tween.SetDelay(Duration);
            Add(tween);
            return this;
        }

        public TweenGroup Remove(ITween tween)
        {
            tweens.Remove(tween);
            return this;
        }

        public TweenGroup AddTweens(IEnumerable<ITween> tweens)
        {
            if (tweens == null) return this;
            
            foreach (var tween in tweens)
            {
                Add(tween);
            }

            return this;
        }

        public void Evaluate(float seconds)
        {
            foreach (var tween in tweens)
            {
                tween.Evaluate(seconds - delay);
            }
        }

        public void Advance(float deltaSeconds)
        {
            var position = playHeadSeconds + deltaSeconds;
            var d = playHeadSeconds <= delay
                ? position > delay ? position - delay : 0f
                : deltaSeconds;

            var totalDuration = Duration + delay;
            playHeadSeconds = Mathf.Clamp(position, 0f, totalDuration);
            foreach (var tween in tweens)
            {
                tween.Pause();
                tween.Advance(d);
            }

            if (position >= totalDuration)
            {
                CompleteTween();
            }
        }

        public void Skip()
        {
            Seek(delay + Duration);
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

            Pause();
        }

        public void Kill()
        {
            foreach (var tween in tweens)
            {
                tween.Kill();
            }

            tweens.Clear();
        }

        public void Play()
        {
            if (completed)
            {
                Seek(0);
                completed = false;
            }
            playing = true;
            TweenManager.AddTween(this);
        }

        public void Pause()
        {
            playing = false;
            TweenManager.RemoveTween(this);
        }

        public void Seek(float seconds)
        {
            playHeadSeconds = seconds;
            foreach (var tween in tweens)
            {
                tween.Seek(seconds - delay);
            }
        }

        public ITween SetDelay(float value)
        {
            delay = value;
            return this;
        }

        public ITween SetEasing(EasingFunction value)
        {
            foreach (var tween in tweens)
            {
                tween.SetEasing(value);
            }

            return this;
        }
        
        public ITween SetTarget(object value)
        {
            target = value;
            foreach (var tween in tweens)
            {
                tween.SetTarget(target);
            }
            
            return this;
        }

        public ITween Then(Action callback)
        {
            throw new NotImplementedException();
        }
    }
}