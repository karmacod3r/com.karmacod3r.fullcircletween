using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace FullCircleTween.Extensions
{
    public static class EditorGUIReflectionUtils
    {
        public static TextEditor RecycledEditor
        {
            get
            {
                var editorFieldInfo = typeof(EditorGUI).GetField("s_RecycledEditor", BindingFlags.Static | BindingFlags.NonPublic);
                return editorFieldInfo.GetValue(null) as TextEditor;
            }
        }

        public static int LastControlId
        {
            get
            {
                var controlIdFieldInfo = typeof(EditorGUIUtility).GetField("s_LastControlID", BindingFlags.Static | BindingFlags.NonPublic);
                var controlId = (int) controlIdFieldInfo.GetValue(null);
                return controlId;
            }
        }

        public static void AddGlobalEventHandler(EditorApplication.CallbackFunction callback)
        {
            var globalEventHandlerInfo = typeof(EditorApplication).GetField("globalEventHandler", BindingFlags.Static | BindingFlags.NonPublic);
            var globalEventHandler = (EditorApplication.CallbackFunction) globalEventHandlerInfo.GetValue(null);
            globalEventHandler += callback;
            globalEventHandlerInfo.SetValue(null, globalEventHandler);
        }

        public static void RemoveGlobalEventHandler(EditorApplication.CallbackFunction callback)
        {
            var globalEventHandlerInfo = typeof(EditorApplication).GetField("globalEventHandler", BindingFlags.Static | BindingFlags.NonPublic);
            var globalEventHandler = (EditorApplication.CallbackFunction) globalEventHandlerInfo.GetValue(null);
            globalEventHandler -= callback;
            globalEventHandlerInfo.SetValue(null, globalEventHandler);
        }
    }
}