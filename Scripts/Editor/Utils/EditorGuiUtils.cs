using UnityEditor;
using UnityEngine;

namespace FullCircleTween.Utils
{
    public static class EditorGuiUtils
    {
        public static Rect GetPropertyRect(Rect position, GUIContent label)
        {
            var rect = position;
            if (label != GUIContent.none && label.text != "")
            {
                rect.x += EditorGUIUtility.labelWidth + EditorGUIUtility.standardVerticalSpacing;
                rect.width -= EditorGUIUtility.labelWidth + EditorGUIUtility.standardVerticalSpacing * 2;
            }

            return rect;
        }
    }
}