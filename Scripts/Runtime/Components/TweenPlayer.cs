using FullCircleTween.Core;
using FullCircleTween.Properties;
using UnityEngine;

namespace FullCircleTween.Components
{
    public class TweenPlayer : MonoBehaviour
    {
        [SerializeField] private TweenGroupClip tweenGroup;

        public TweenGroupClip TweenGroup => tweenGroup;

        public void Add(TweenClip clip)
        {
            tweenGroup.Add(clip);
        }

        public void Remove(TweenClip clip)
        {
            tweenGroup.Remove(clip);
        }

        public void Play()
        {
            tweenGroup.Play(this);
        }

        public void Kill()
        {
            tweenGroup.Kill();
        }

        public void Pause()
        {
            tweenGroup.Pause();
        }

        public TweenGroup Create() => tweenGroup.Create(this);
    }
}