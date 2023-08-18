using System;
using System.Collections.Generic;
using System.Linq;
using FullCircleTween.Core;
using FullCircleTween.Core.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FullCircleTween.Properties
{
    [Serializable]
    public class TweenGroupClip
    {
        public float delay;
        public EditableList<TweenClip> tweenClips = new EditableList<TweenClip>();

        public event Action onComplete;
        
        private TweenGroup tweenGroup;
        private float pauseTime;
        private Component context;
        
        public TweenGroupClip()
        {
        }
        
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

        public TweenGroup Play(Component context)
        {
            Play(context, Create(context));
            return tweenGroup;
        }

        public TweenGroup Play(Component context, TweenGroup group)
        {
            if (tweenGroup != null)
            {
                tweenGroup.onComplete -= OnTweenComplete;
                tweenGroup.Kill();
            }

            tweenGroup = group;
            this.context = context;
            tweenGroup.onComplete += OnTweenComplete;
            tweenGroup.SetDelay(delay);
            tweenGroup.Seek(pauseTime);
            pauseTime = 0;
            
            return tweenGroup;
        }

        public TweenGroup Create(Component context) => new TweenGroup(tweenClips.Select(clip => clip.CreateTween(context)));

        private void OnTweenComplete()
        {
            pauseTime = 0;
            onComplete?.Invoke();
            
#if UNITY_EDITOR
            if (context != null)
            {
                SetDirty(context.transform);   
            }
#endif
        }
        
        public static void SetDirty(Object target)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && target != null)
            {
                UnityEditor.EditorUtility.SetDirty(target);
            }
#endif
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