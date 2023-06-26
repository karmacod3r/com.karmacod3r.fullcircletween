using System;
using System.Collections.Generic;
using System.Linq;
using FullCircleTween.Core.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace FullCircleTween.Core
{
    public class TweenRunner
    {
        private List<ITween> tweens = new List<ITween>();

        private bool inUpdate;
        private Queue<Action> cachedMethodCalls = new();

        public void Add(ITween tween)
        {
            if (inUpdate)
            {
                cachedMethodCalls.Enqueue(() => Add(tween));
                return;
            }

            tweens.Add(tween);
        }

        public void Remove(ITween tween)
        {
            if (inUpdate)
            {
                cachedMethodCalls.Enqueue(() => Remove(tween));
                return;
            }
            
            tweens.Remove(tween);
        }

        public void SkipAll()
        {
            if (inUpdate)
            {
                cachedMethodCalls.Enqueue(SkipAll);
                return;
            }
            
            foreach (var tween in tweens)
            {
                tween.Skip();
            }
        }

        public void KillTweensOf(object target)
        {
            if (inUpdate)
            {
                cachedMethodCalls.Enqueue(() => KillTweensOf(target));
                return;
            }
            
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

        public void KillSceneTweens(Scene scene)
        {
            var sceneTweens = tweens.Where(t => t.Target is Component component && component.gameObject.scene == scene
            || (t.Target is GameObject gameObject && gameObject.scene == scene)).ToList();
            
            sceneTweens.ForEach(t => t.Kill());
        }

        public void Seek(float seconds)
        {
            inUpdate = true;
            
            foreach (var tween in tweens)
            {
                tween.Seek(seconds);
            }

            inUpdate = false;

            CallCachedMethods();
        }

        public void Advance(float deltaSeconds)
        {
            inUpdate = true;
            
            foreach (var tween in tweens)
            {
                tween.Advance(deltaSeconds);
            }

            inUpdate = false;

            CallCachedMethods();
        }

        private void CallCachedMethods()
        {
            while (cachedMethodCalls.Count > 0)
            {
                cachedMethodCalls.Dequeue()();
            }
        }
    }
}