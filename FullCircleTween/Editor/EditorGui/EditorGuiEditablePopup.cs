using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FullCircleTween.Extensions;
using UnityEditor;
using UnityEngine;

namespace FullCircleTween.EditorGui
{
    public static class EditorGuiEditablePopup
    {
        private static GenericMenu popupMenu;
        private static int currentPopupControlId;
        private static string clickedOption;
        private static PopupEditorWindow popupWindow;
        
        public static void Draw(Rect position, SerializedProperty property, List<string> values, string placeholder = "")
        {
            Draw(position, GUIContent.none, property, values);
        }

        public static void Draw(Rect position, string label, SerializedProperty property, List<string> values, string placeholder = "")
        {
            Draw(position, new GUIContent(label), property, values, placeholder);
        }

        public static void Draw(Rect position, GUIContent label, SerializedProperty property, List<string> values, string placeholder = "")
        {
            var controlName = "tf" + property.propertyPath + property.serializedObject.targetObject.GetInstanceID();
            GUI.SetNextControlName(controlName);

            if (popupWindow != null)
            {
                popupWindow.ProcessEvents(Event.current);
            }
            
            property.stringValue = EditorGUI.TextField(position, label, property.stringValue);
            var controlId = EditorGUIReflectionUtils.LastControlId;

            if (! string.IsNullOrEmpty(placeholder) && property.stringValue == "")
            {
                GUI.color = new Color(1, 1, 1, 0.3f);
                var rect = new Rect(position);
                rect.x += 2;
                EditorGUI.LabelField(rect, placeholder);
                GUI.color = Color.white;
            }
            
            if (GUI.GetNameOfFocusedControl() == controlName)
            {
                var options = values.Where(v => String.Equals(v, property.stringValue, StringComparison.CurrentCultureIgnoreCase))
                    .Concat(values.Where(v => v.ToLower().Contains(property.stringValue.ToLower()) && v != property.stringValue)).ToList();

                if (clickedOption != null)
                {
                    SetPropertyValue(property, clickedOption);
                    clickedOption = null;
                }

                var rect = new Rect(position);
                if (label != GUIContent.none && label.text != "")
                {
                    rect.x += EditorGUIUtility.labelWidth + EditorGUIUtility.standardVerticalSpacing;
                    rect.width -= EditorGUIUtility.labelWidth + EditorGUIUtility.standardVerticalSpacing * 2;
                }
                rect.y += rect.height;

                if (popupWindow == null)
                {
                    popupWindow = ScriptableObject.CreateInstance<PopupEditorWindow>();
                    currentPopupControlId = controlId;
                    popupWindow.Show();
                    popupWindow.property = property;
                    popupWindow.changed += value => { clickedOption = value; };
                    popupWindow.closed += () =>
                    {
                        popupWindow.Dispose();
                        popupWindow = null;
                    };
                }

                popupWindow.options = options;
                popupWindow.AdjustSize(GUIUtility.GUIToScreenRect(rect));
            } else
            {
                if (popupWindow != null && currentPopupControlId == controlId)
                {
                    popupWindow.Close();
                    popupWindow.Dispose();
                    popupWindow = null;
                }
            }
        }

        private static void SetPropertyValue(SerializedProperty property, string value)
        {
            property.stringValue = value;
            property.serializedObject.ApplyModifiedProperties();
            GUI.changed = true;
        }
    }
}