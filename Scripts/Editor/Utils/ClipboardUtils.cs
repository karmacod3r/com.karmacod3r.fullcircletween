using System;
using FullCircleTween.Core;
using UnityEditor;

namespace DataBinding.Lib.Editor
{
    [Serializable]
    public class ClipboardEntry
    {
        public string type;
        public string value;
    }

    public static class ClipboardUtils
    {
        private static void Push(string text)
        {
            EditorGUIUtility.systemCopyBuffer = text;
        }

        public static void Push<T>(T serializable)
        {
            Push(Serializer.SerializeObject(serializable));
        }

        private static string Get()
        {
            return EditorGUIUtility.systemCopyBuffer;
        }

        public static T Get<T>() where T : class, new()
        {
            var buffer = Get();
            if (string.IsNullOrEmpty(buffer)) return default;

            return Serializer.DeserializeObject<T>(buffer);
        }
    }
}