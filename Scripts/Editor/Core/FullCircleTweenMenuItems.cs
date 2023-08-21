using FullCircleTween.Components;
using UnityEditor;
using UnityEngine;

namespace FullCircleTween.Core
{
    public static class FullCircleTweenMenuItems
    {
        [MenuItem("Window/Full Circle Tween/Preferences", false, 2400)]
        private static void CreateConfig()
        {
            Selection.activeObject = FullCircleConfig.Instance;  
        }
        
        [MenuItem("Window/Full Circle Tween/Recache Tween Methods...", false, 2410)]
        private static void RecacheTweenMethods()
        {
            TweenMethodCache.RecacheTweenMethods();
        }
        
        [MenuItem("Window/Full Circle Tween/Skip running tweens _>", false, 2420)]
        private static void SkipRunningTweens()
        {
            TweenManager.SkipAll();
        }
        
        [MenuItem("Window/Full Circle Tween/Kill all tweens", false, 2430)]
        private static void KillAllTweens()
        {
            TweenManager.KillAll();
        }
        
        [MenuItem("Window/Full Circle Tween/Toggle Record Mode ^#r", false, 2440)]
        private static void TweenSelectedProperty()
        {
            TweenStateMachineEditor.ToggleRecording();
        }
    }
}