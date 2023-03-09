using FullCircleTween.Core;
using UnityEngine;

namespace FullCircleTween.Extensions
{
    public static class KillExtension
    {
        public static void KillTweens(this Transform target)
        {
            TweenManager.KillTweensOf(target);
        }
        
        public static void KillTweens(this Behaviour target)
        {
            TweenManager.KillTweensOf(target);
        }
        
        public static void KillTweens(this GameObject target)
        {
            TweenManager.KillTweensOf(target);
        }
    }
}