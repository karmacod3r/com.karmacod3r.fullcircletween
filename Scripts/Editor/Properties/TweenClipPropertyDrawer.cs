using System;
using System.Globalization;
using FullCircleTween.Core;
using UnityEditor;
using UnityEngine;

namespace FullCircleTween.Properties
{
    [CustomPropertyDrawer(typeof(TweenClip))]
    public class TweenClipPropertyDrawer : PropertyDrawer
    {
        private static readonly string[] controlSizing = {"24", "15%", "25%", "*", "24"};
        private static Rect[] controlRects = new Rect[5];

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var delayProperty = property.FindPropertyRelative("delay");
            var tweenTargetProperty = property.FindPropertyRelative("tweenTarget");
            var tweenMethodNameProperty = property.FindPropertyRelative("tweenMethodName");
            var toValueProperty = property.FindPropertyRelative("toValue");
            var toValueSerializedValueProperty = toValueProperty.FindPropertyRelative("serializedValue");
            var durationProperty = property.FindPropertyRelative("duration");

            CalculateHorizontalLayout(position, controlSizing, ref controlRects);
            
            EditorGUI.PropertyField(controlRects[0], delayProperty, GUIContent.none);
            EditorGUI.PropertyField(controlRects[1], tweenTargetProperty, GUIContent.none);
            var targetComponent = TweenTarget.Deserialize(property.serializedObject.targetObject as Component, tweenTargetProperty.FindPropertyRelative("typeName").stringValue);
            var targetType = targetComponent?.GetType();
            tweenMethodNameProperty.stringValue = TweenMethodPopup(controlRects[2], targetType, tweenMethodNameProperty.stringValue, "");
            toValueSerializedValueProperty.stringValue = ValueField(controlRects[3], TweenMethodCache.GetMethodTweenedType(TweenMethodCache.GetTweenMethodInfo(targetType, tweenMethodNameProperty.stringValue)), toValueSerializedValueProperty.stringValue);
            EditorGUI.PropertyField(controlRects[4], durationProperty, GUIContent.none);

            EditorGUI.EndProperty();
        }

        private static void CalculateHorizontalLayout(Rect position, string[] sizing, ref Rect[] rects, float minSize = 16f, float margin = 0f, float spacing = 2f)
        {
            if (rects == null || rects.Length != sizing.Length)
            {
                rects = new Rect[sizing.Length];
            }

            var expandCount = 0;
            var restSize = position.width - margin * 2;
            foreach (var size in sizing)
            {
                if (size == "*")
                {
                    expandCount++;
                } else 
                {
                    restSize -= GetSize(size, position.width);
                }
            }

            var expandedSize = (restSize - (sizing.Length - 1) * spacing) / expandCount;
            var rect = position;
            rect.x += margin;

            for (var i = 0; i < sizing.Length; i++)
            {
                var w = sizing[i] == "*" ? expandedSize : GetSize(sizing[i], position.width);
                rect.width = Mathf.Max(w, minSize);
                rects[i] = rect;
                rect.x += w + spacing;
            }
        }

        private static float GetSize(string size, float fullSize)
        {
            if (size.Contains("%"))
            {
                var p = float.Parse(size.Replace("%", ""), CultureInfo.InvariantCulture);
                return fullSize * p / 100f;
            }
            
            return float.Parse(size, CultureInfo.InvariantCulture);
        }

        private static string TweenMethodPopup(Rect position, Type targetType, string value, string label)
        {
            var methodNames = TweenMethodCache.GetPopupMethodNames(targetType);
            var index = EditorGUI.Popup(position, label, methodNames.IndexOf(value), methodNames.ToArray());
            return index > -1 && index < methodNames.Count ? methodNames[index] : value;
        }

        public static string ValueField(Rect position, Type propertyType, string value)
        {
            var deserializedValue = TweenClipValue.Deserialize(propertyType, value);

            // TODO: Enable custom type property drawers
            if (propertyType == typeof(bool))
            {
                return TweenClipValue.Serialize(EditorGUI.Toggle(position, (bool) deserializedValue));
            }

            if (propertyType == typeof(float) || propertyType == typeof(double))
            {
                return TweenClipValue.Serialize(EditorGUI.FloatField(position, (float) deserializedValue));
            }

            if (propertyType == typeof(int))
            {
                return TweenClipValue.Serialize(EditorGUI.IntField(position, (int) deserializedValue));
            }

            if (propertyType == typeof(Vector2))
            {
                return TweenClipValue.Serialize(EditorGUI.Vector2Field(position, "", (Vector2) deserializedValue));
            }

            if (propertyType == typeof(Vector3))
            {
                return TweenClipValue.Serialize(EditorGUI.Vector3Field(position, "", (Vector3) deserializedValue));
            }

            if (propertyType == typeof(Vector4))
            {
                return TweenClipValue.Serialize(EditorGUI.Vector4Field(position, "", (Vector4) deserializedValue));
            }

            if (propertyType == typeof(Quaternion))
            {
                return TweenClipValue.Serialize(Quaternion.Euler(EditorGUI.Vector3Field(position, "", ((Quaternion) deserializedValue).eulerAngles)));
            }

            if (propertyType == typeof(Color))
            {
                return TweenClipValue.Serialize(EditorGUI.ColorField(position, "", (Color) deserializedValue));
            }

            if (propertyType == typeof(Color32))
            {
                return TweenClipValue.Serialize(EditorGUI.ColorField(position, "", (Color32) deserializedValue));
            }

            if (propertyType == typeof(string))
            {
                return EditorGUI.TextField(position, value);
            }

            return value;
        }
    }
}