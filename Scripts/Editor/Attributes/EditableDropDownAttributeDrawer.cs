using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FullCircleTween.EditorGui;
using UnityEditor;
using UnityEngine;

namespace FullCircleTween.Attributes
{
    [CustomPropertyDrawer(typeof(EditableDropDownAttribute))]
    public class EditableDropDownAttributeDrawer : PropertyDrawer
    {
        private IEnumerable<string> options;
        private SerializedProperty property;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.property = property;
            var editableDropdownAttribute = (EditableDropDownAttribute)attribute;
            
            var options = AttributeDrawerUtils.GetOptions(property.serializedObject.targetObject, editableDropdownAttribute.optionsMemberName);
            if (property.propertyType != SerializedPropertyType.String || options == null)
            {
                base.OnGUI(position, property, label);
                return;
            }
            var methodInfo = property.serializedObject.targetObject.GetType().GetMethod(editableDropdownAttribute.onChangeListenerName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();
            var optionsList = options.ToList();
            EditorGuiEditablePopup.Draw(position, label, property, optionsList, editableDropdownAttribute.placeholder);
            if (string.IsNullOrEmpty(property.stringValue) && !editableDropdownAttribute.allowUndefined && optionsList.Count > 0)
            {
                property.stringValue = optionsList[0];
            }
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
                
                if (methodInfo != null && methodInfo.GetParameters().Length == 0)
                {
                    methodInfo.Invoke(property.serializedObject.targetObject, null);
                }
            }
                
            EditorGUI.EndProperty();
        }
    }
}