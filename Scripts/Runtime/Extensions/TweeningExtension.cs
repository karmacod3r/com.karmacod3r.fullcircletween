using System;
using FullCircleTween.Core;
using FullCircleTween.Core.Interfaces;
using UnityEngine;

namespace FullCircleTween.Extensions
{
    public static class TweeningExtension
    {
        public static void KillAllTweens(this Component target)
        {
            TweenManager.KillTweensOf(target);
        }
        
        public static void KillAllTweens(this object target)
        {
            TweenManager.KillTweensOf(target);
        }

        public static ITween DelayedCall(this Component target, float delay, Action callback)
        {
            return Tween.DelayedCall(target, delay, callback);    
        }
    }
}