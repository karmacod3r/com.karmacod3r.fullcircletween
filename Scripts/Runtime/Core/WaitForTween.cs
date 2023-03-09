using FullCircleTween.Core.Interfaces;
using UnityEngine;

namespace FullCircleTween.Core
{
    public class WaitForTween : CustomYieldInstruction
    {
        private readonly ITween tween;

        public WaitForTween(ITween tween)
        {
            this.tween = tween;
        }

        public override bool keepWaiting => tween.IsPlaying;
    }
}