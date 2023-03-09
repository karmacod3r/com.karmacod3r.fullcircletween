using System;
using UnityEngine;

namespace FullCircleTween.Core
{
    public class FullCircleConfig : ScriptableObject
    {
        public const string ResourceFilePath = "Assets/Resources/";
        public const string ResourceFile = ResourceFilePath + "FullCircleConfig.asset";
        
        public static FullCircleConfig Instance { get; private set; }
        
        [Header("Tweening")] public bool skipTweensInEditMode;
        [Min(0f)] public float globalTweenTimeScale = 1f;

        public static FullCircleConfig LoadConfig()
        {
            var config = Resources.Load<FullCircleConfig>(ResourceFile);
            return config != null ? config : CreateInstance<FullCircleConfig>();
        }

        private void OnEnable()
        {
            Instance = this;
        }
    }
}