using System;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FullCircleTween.Core
{
    [DefaultExecutionOrder(-200)]
    public class FullCircleConfig : ScriptableObject
    {
        private const string ConfigResourceFilePath = "Assets/Resources/";
        private const string ConfigResourceFile = "FullCircleConfig";
        private const string ConfigAssetFilePath = ConfigResourceFilePath + ConfigResourceFile + ".asset";
        
        public static FullCircleConfig Instance { get; private set; }
        
        [Header("Performance")] 
        [Tooltip("Trigger player loop update in edit mode to enable smooth script updates and coroutines.")] public bool triggerUpdateInEditMode = true;
        
        [Header("Tweening")] 
        public bool skipTweensInEditMode;
        [Min(0f)] public float globalTweenTimeScale = 1f;

        [RuntimeInitializeOnLoadMethod]
        private static void RuntimeInitialize()
        {
            LoadConfig();
        }
        
        private static void LoadConfig()
        {
            var config = Resources.Load<FullCircleConfig>(ConfigResourceFile);
            Instance = config ?? CreateInstance<FullCircleConfig>();
        }
        
        #if UNITY_EDITOR
        
        [InitializeOnLoadMethod]
        private static void EditorInitialize()
        {
            LoadOrCreateConfig();
        }

        private static void LoadOrCreateConfig()
        {
            LoadConfig();
            SaveConfig();
        }
        
        private static void SaveConfig()
        {
            if (!Directory.Exists(ConfigResourceFilePath))
            {
                Directory.CreateDirectory(ConfigResourceFilePath);
            }

            if (!File.Exists(ConfigAssetFilePath))
            {
                AssetDatabase.CreateAsset(Instance, ConfigAssetFilePath);
                AssetDatabase.Refresh();
            }
        }
        #endif
    }
}