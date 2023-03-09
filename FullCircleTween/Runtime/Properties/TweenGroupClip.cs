using System;
using System.Collections.Generic;
using System.Linq;
using FullCircleTween.Core;
using FullCircleTween.Core.Interfaces;
using UnityEngine;

namespace FullCircleTween.Properties
{
    [Serializable]
    public class TweenGroupClip
    {
        public float delay;
        [SerializeField] private EditableList<TweenClip> tweenClips = new EditableList<TweenClip>();
        
        private TweenGroup tweenGroup;
        private float pauseTime;

        public TweenGroupClip(TweenClip clip)
        {
            Add(clip);
        }
        
        public TweenGroupClip(IEnumerable<TweenClip> clips)
        {
            foreach (var clip in clips)
            {
                Add(clip);
            }
        }
        
        public void Add<T>(Component target, string tweenMethodName, T toValue, float duration, float delay = 0f)
        {
            tweenClips.Add(TweenClip.Create(target, tweenMethodName, toValue, duration, delay));
        }
        
        public void Add<T>(Tween<T> tween)
        {
            if (! (tween.Target is Component)) return;
            
            tween.Pause();
            Add(TweenClip.Create(tween.Target as Component, tween.memberName, tween.toValue, tween.Duration, tween.Delay));
        }
        
        public void Add(TweenClip clip)
        {
            if (clip == null) return;
            tweenClips.Add(clip);
        }
        
        public void Append<T>(Component target, string tweenMethodName, T toValue, float duration)
        {
            var offset = tweenClips.Sum(clip => clip.delay + clip.duration);
            tweenClips.Add(TweenClip.Create(target, tweenMethodName, toValue, duration, offset));
        }

        public void Remove(TweenClip clip)
        {
            tweenClips.Remove(clip);
        }

        public ITween Play(Component context)
        {
            if (tweenGroup != null)
            {
                tweenGroup.onComplete -= OnTweenComplete;
                tweenGroup.Kill();
            }

            tweenGroup = Create(context);
            tweenGroup.onComplete += OnTweenComplete;
            tweenGroup.Seek(pauseTime);
            tweenGroup.SetDelay(delay);
            pauseTime = 0;
            
            return tweenGroup;
        }

        public TweenGroup Create(Component context) => new TweenGroup(tweenClips.Select(clip => clip.CreateTween(context)));

        private void OnTweenComplete()
        {
            pauseTime = 0;
        }

        public void Kill()
        {
            tweenGroup?.Kill();
            tweenGroup = null;
            pauseTime = 0;
        }

        public void Pause()
        {
            if (tweenGroup == null || !tweenGroup.IsPlaying) return;
            tweenGroup.Pause();
            pauseTime = tweenGroup.PlayHeadSeconds;
        }        
    }
}