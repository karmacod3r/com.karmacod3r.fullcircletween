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

        private TweenRunner parentRunner;
        private TweenRunner childRunner = new TweenRunner();

        private bool playing;
        private bool completed;
        private float delay;
        
        private float playHeadSeconds;

        public bool IsPlaying => playing;
        public bool Completed => completed;
        public object Target => this;
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

        public TweenGroup(IEnumerable<ITween> tweens = null)
        {
            parentRunner = TweenManager.Runner;
            
            Insert(tweens);
            Play();
        }

        public TweenGroup Insert(ITween tween, float offsetSeconds = 0)
        {
            if (tween == null) return this;

            tween.SetTarget(this);
            tween.SetDelay(tween.Delay + offsetSeconds);
            tween.SetRunner(childRunner);
            tweens.Add(tween);

            tween.Pause();
            if (playing)
            {
                tween.Play();
            }

            SortTweens();

            return this;
        }

        public TweenGroup Insert(IEnumerable<ITween> tweens, float offsetSeconds = 0)
        {
            if (tweens == null) return this;

            foreach (var tween in tweens)
            {
                Insert(tween, offsetSeconds);
            }

            return this;
        }

        private void SortTweens()
        {
            tweens.Sort((a, b) =>
            {
                var rightA = a.Delay + a.Duration;
                var rightB = b.Delay + b.Duration;
                return rightA.CompareTo(rightB); 
            });
        }

        public TweenGroup Append(ITween tween)
        {
            if (tween == null) return this;

            Insert(tween, Duration);
            return this;
        }

        public TweenGroup Append(IEnumerable<ITween> tweens)
        {
            if (tweens == null) return this;

            Insert(tweens, Duration);
            return this;
        }

        public TweenGroup Remove(ITween tween)
        {
            tweens.Remove(tween);
            return this;
        }

        public TweenGroup Remove(IEnumerable<ITween> tweens)
        {
            foreach (var tween in tweens)
            {
                Remove(tween);
            }

            return this;
        }

        public void Evaluate(float seconds)
        {
        }

        public void SetRunner(TweenRunner value)
        {
            parentRunner.Remove(this);
            parentRunner = value;
            
            if (playing && !completed)
            {
                parentRunner.Add(this);                    
            }
        }

        public object GetCurrentValue()
        {
            throw new NotImplementedException();
        }

        public void Skip()
        {
            Seek(delay + Duration);
        }

        public void Advance(float deltaSeconds)
        {
            Seek(playHeadSeconds + deltaSeconds);
        }

        public void Seek(float seconds)
        {
            playHeadSeconds = seconds;
            var totalDuration = Duration + delay;
            
            childRunner.Seek(playHeadSeconds - delay);

            if (playing && playHeadSeconds >= totalDuration)
            {
                CompleteTween();
            }
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
                    thenEvent -= (Action)d;
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
            parentRunner.Add(this);
            
            foreach (var tween in tweens)
            {
                tween.Play();
            }
        }

        public void Pause()
        {
            playing = false;
            parentRunner.Remove(this);
            
            foreach (var tween in tweens)
            {
                tween.Pause();
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
            return this;
        }

        public ITween Then(Action callback)
        {
            throw new NotImplementedException();
        }
        
        public void KillTweensOf(object target)
        {
            for (var i = tweens.Count - 1; i >= 0; i--)
            {
                var tween = tweens[i];

                if (tween is TweenGroup group)
                {
                    group.KillTweensOf(target);
                } else if (tween.Target == target)
                {
                    tween.Kill();
                }
            }
        }
    }
}