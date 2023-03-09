using System.Collections.Generic;
using FullCircleTween.EditorGui;
using FullCircleTween.Extensions;
using UnityEditor;
using UnityEngine;

namespace FullCircleTween.Properties
{
    [CustomPropertyDrawer(typeof(EditableList<>))]
    public class EditableListDrawer : PropertyDrawer
    {
        private class EditableListAttributeDrawerState
        {
            public IEditorGuiEditableList editableList;
            public string propertyPath;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var controlID = GUIUtility.GetControlID(FocusType.Passive);
            var state = GUIUtility.GetStateObject(typeof(EditableListAttributeDrawerState), controlID) as EditableListAttributeDrawerState;
            var listProperty = property.FindPropertyRelative("list");
            if (state.editableList == null || !state.editableList.IsValid || property.propertyPath != state.propertyPath)
            {
                var target = property.GetTarget();
                var editableListType = typeof(EditorGuiEditableList<>).MakeGenericType(target.GetType().GetGenericArguments());
                var constructor = editableListType.GetConstructor(new[] { typeof(SerializedObject), typeof(SerializedProperty) });
                state.editableList = (IEditorGuiEditableList)constructor.Invoke(new object[] { listProperty.serializedObject, listProperty });
                state.editableList.DisplayName = property.displayName;
                state.propertyPath = property.propertyPath;
            }

            EditorGUI.BeginProperty(position, label, property);
            state.editableList.DoList(position);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var listProperty = property.FindPropertyRelative("list");
            var height = 3 * EditorGUIUtility.singleLineHeight;
            
            for (var i = 0; i < listProperty.arraySize; i++)
            {
                var element = listProperty.GetArrayElementAtIndex(i);
                height += EditorGUI.GetPropertyHeight(element);
            }

            if (listProperty.arraySize == 0)
            {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            height += listProperty.arraySize * EditorGUIUtility.standardVerticalSpacing;
            
            return height;
        }
    }
}