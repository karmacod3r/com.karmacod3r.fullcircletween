using System;

namespace FullCircleTween.Core.Interfaces
{
    public interface ITween
    {
        bool IsPlaying { get; }
        bool Completed { get; }
        object Target { get; }
        float PlayHeadSeconds { get; }
        float Duration { get; }
        float Delay { get; }
        void Evaluate(float seconds);
        void Skip();
        void Kill();
        void Play();
        void Pause();
        void Seek(float seconds);
        ITween SetDelay(float value);
        ITween SetEasing(EasingFunction value);
        ITween SetTarget(object value);
        ITween Then(Action callback);
        void Advance(float deltaSeconds);
        void SetRunner(TweenRunner value);
        object GetCurrentValue();
    }
}