#define FULL_CIRCLE_TWEEN

using System;
using System.Diagnostics;
using FullCircleTween.Core.Interfaces;
using FullCircleTween.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FullCircleTween.Core
{
    public static class TweenManager
    {
        private static bool initialized;
        public static TweenRunner Runner { get; } = new();

        internal static void KillTweensOf(object target)
        {
            Runner.KillTweensOf(target);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            KillAll();
            
            if (initialized || !Application.isPlaying) return;
            initialized = true;
            
            var go = new GameObject("FullCircleTween");
            go.AddComponent<UpdateDispatcher>();
            go.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.HideAndDontSave;
            Object.DontDestroyOnLoad(go);
        }

        internal static void UpdateTweens(float deltaSeconds)
        {
            if (!Application.isPlaying && FullCircleConfig.Instance.skipTweensInEditMode)
            {
                SkipAll();
                return;
            }

            Runner.Advance(deltaSeconds * FullCircleConfig.Instance.globalTweenTimeScale);
        }

        public static void SkipAll()
        {
            Runner.SkipAll();
        }

        public static void KillAll()
        {
            Runner.KillAllTweens();
        }
        
#if UNITY_EDITOR
        private static Stopwatch stopWatch;

        [InitializeOnLoadMethod]
        private static void EditorInitialize()
        {
            KillAll();
            
            stopWatch = new Stopwatch();
            EditorApplication.playModeStateChanged -= OnPlayModeState;
            EditorApplication.playModeStateChanged += OnPlayModeState;
            EditorApplication.update -= OnEditorUpdate;
            EditorApplication.update += OnEditorUpdate;
        }
        
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded() {
            KillAll();
        }

        private static void OnPlayModeState(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                case PlayModeStateChange.ExitingPlayMode:
                    KillAll();
                    break;
            }
        }

        private static void OnEditorUpdate()
        {
            if (Application.isPlaying) return;

            UpdateTweens(stopWatch.ElapsedMilliseconds / 1000f);
            stopWatch.Restart();

            if (FullCircleConfig.Instance.triggerUpdateInEditMode)
            {
                // Trigger player loop to enable updates and coroutines in edit mode
                EditorApplication.QueuePlayerLoopUpdate();
            }
        }
#endif
    }
}