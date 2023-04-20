using System;
using System.Collections.Generic;
using System.Linq;
using FullCircleTween.Extensions;
using FullCircleTween.Utils;
using UnityEditor;
using UnityEngine;

namespace FullCircleTween.EditorGui
{
    public static class EditorGuiDropDown
    {
        private static GenericMenu popupMenu;
        private static int currentPopupControlId;
        private static string clickedOption;
        private static PopupEditorWindow popupWindow;
        private static string lastFocusedControl = "";
        private static readonly int hash = "s_dropdown".GetHashCode();
        private static GUIContent tempContent = new GUIContent();

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
            var controlName = "dd" + property.propertyPath + property.serializedObject.targetObject.GetInstanceID();
            GUI.SetNextControlName(controlName);

            if (popupWindow != null)
            {
                popupWindow.ProcessEvents(Event.current);
            }

            var controlId = GUIUtility.GetControlID(hash, FocusType.Keyboard, position);

            var current = Event.current;
            var focused = GUIUtility.keyboardControl == controlId || GUIUtility.hotControl == controlId;
            var reopenPopup = current.type == EventType.MouseDown
                              || (current.type == EventType.KeyDown
                                  && (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.DownArrow || current.keyCode == KeyCode.UpArrow));

            if (current.type == EventType.MouseDown && current.button == 0 && position.Contains(current.mousePosition))
            {
                GUIUtility.keyboardControl = controlId;
                EditorGUIUtility.editingTextField = false;
                HandleUtility.Repaint();
            }

            var propertyRect = EditorGuiUtils.GetPropertyRect(position, label);
            if (current.type == EventType.Repaint)
            {
                EditorStyles.popup.Draw(EditorGUI.PrefixLabel(position, controlId, label), TempContent(property.stringValue), false, false, false, focused);
            }

            if (!string.IsNullOrEmpty(placeholder) && property.stringValue == "")
            {
                GUI.color = new Color(1, 1, 1, 0.3f);
                var placeholderRect = position;
                placeholderRect.x += 2;
                EditorGUI.LabelField(placeholderRect, placeholder);
                GUI.color = Color.white;
            }

            if (clickedOption != null)
            {
                SetPropertyValue(property, clickedOption);
                clickedOption = null;
            }

            focused = focused || GUI.GetNameOfFocusedControl().StartsWith(controlName);
            if (focused && reopenPopup)
            {
                current.Use();
                ShowPopupWindow(controlId, property, values);
            }

            if (GUI.GetNameOfFocusedControl() != lastFocusedControl 
                && !focused 
                && !lastFocusedControl.StartsWith(controlName)
                && popupWindow != null 
                && currentPopupControlId == controlId)
            {
                popupWindow.Close();
                popupWindow.Dispose();
                popupWindow = null;
            }

            if (popupWindow != null)
            {
                popupWindow.AdjustSize(GUIUtility.GUIToScreenRect(propertyRect));
            }

            lastFocusedControl = GUI.GetNameOfFocusedControl();
        }

        internal static GUIContent TempContent(string t)
        {
            tempContent.text = t;
            return tempContent;
        }

        private static void ShowPopupWindow(int controlId, SerializedProperty property, List<string> values)
        {
            if (popupWindow == null)
            {
                popupWindow = ScriptableObject.CreateInstance<PopupEditorWindow>();
                currentPopupControlId = controlId;
                popupWindow.ShowPopup();
                popupWindow.property = property;
                popupWindow.changed += value => { clickedOption = value; };
                popupWindow.closed += () => { popupWindow = null; };
            }

            popupWindow.options = values;
            popupWindow.selectedIndex = values.IndexOf(property.stringValue);
        }


        private static void SetPropertyValue(SerializedProperty property, string value)
        {
            property.stringValue = value;
            property.serializedObject.ApplyModifiedProperties();
            GUI.changed = true;
        }
    }
}