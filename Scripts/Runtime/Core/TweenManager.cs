#define FULL_CIRCLE_TWEEN

using System;
using System.Collections.Generic;
using System.Diagnostics;
using FullCircleTween.Core.Interfaces;
using UnityEngine;
using UnityEngine.LowLevel;
using Debug = UnityEngine.Debug;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace FullCircleTween.Core
{
    public static class TweenManager
    {
        private static bool initialized;
        private static List<ITween> tweens;
        private static bool inUpdate;
        private static List<Action> cachedMethodCalls = new();

        internal static void AddTween(ITween tween)
        {
            if (inUpdate)
            {
                cachedMethodCalls.Add(() => AddTween(tween));
                return;
            }

            tweens.Add(tween);
        }

        internal static void RemoveTween(ITween tween)
        {
            if (inUpdate)
            {
                cachedMethodCalls.Add(() => RemoveTween(tween));
                return;
            }

            tweens.Remove(tween);
        }

        internal static void KillTweensOf(object target)
        {
            for (var i = tweens.Count - 1; i >= 0; i--)
            {
                var tween = tweens[i];
                if (tween.Target != target) return;
                tween.Kill();
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            if (initialized || !Application.isPlaying) return;
            initialized = true;

            tweens = new List<ITween>();
            
            var go = new GameObject("FullCircleTween");
            go.AddComponent<UpdateDispatcher>();
            go.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.HideAndDontSave;
            GameObject.DontDestroyOnLoad(go);
            
            LoadConfig();
        }

        internal static void UpdateTweens(float deltaSeconds)
        {
            if (!Application.isPlaying && FullCircleConfig.Instance.skipTweensInEditMode)
            {
                SkipRunningTweens();
                return;
            }
            
            AdvanceTweens(deltaSeconds * FullCircleConfig.Instance.globalTweenTimeScale);
        }
        
        private static void AdvanceTweens(float deltaSeconds)
        {
            inUpdate = true;
            for (var i = 0; i < tweens.Count; i++)
            {
                tweens[i].Advance(deltaSeconds);
            }
            inUpdate = false;

            CallCachedMethods();
        }

        public static void SkipRunningTweens()
        {
            inUpdate = true;
            for (var i = 0; i < tweens.Count; i++)
            {
                tweens[i].Skip();
            }
            inUpdate = false;

            CallCachedMethods();
        }

        private static void CallCachedMethods()
        {
            cachedMethodCalls.ForEach(method => method());
            cachedMethodCalls.Clear();
        }

        public static void LoadConfig()
        {
            FullCircleConfig.LoadConfig();
        }

#if UNITY_EDITOR
        private static Stopwatch stopWatch;

        [InitializeOnLoadMethod]
        private static void EditorInitialize()
        {
            tweens = new List<ITween>();

            stopWatch = new Stopwatch();
            EditorApplication.update -= OnEditorUpdate;
            EditorApplication.update += OnEditorUpdate;

            LoadOrCreateConfig();
        }

        private static void LoadOrCreateConfig()
        {
            var config = FullCircleConfig.LoadConfig();
            if (!File.Exists(FullCircleConfig.ResourceFile))
            {
                SaveConfig(config);
            }
        }

        private static void SaveConfig(FullCircleConfig config)
        {
            if (!Directory.Exists("Assets/Resources"))
            {
                Directory.CreateDirectory("Assets/Resources");
            }
            AssetDatabase.CreateAsset(config, FullCircleConfig.ResourceFile);
            AssetDatabase.Refresh();
        }
        
        private static void OnEditorUpdate()
        {
            if (Application.isPlaying) return;

            UpdateTweens(stopWatch.ElapsedMilliseconds / 1000f);
            stopWatch.Restart();
            
            // Trigger player loop to enable updates and coroutines in edit mode
            EditorApplication.QueuePlayerLoopUpdate();
        }
#endif
    }
}