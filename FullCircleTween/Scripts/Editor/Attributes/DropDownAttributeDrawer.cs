using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FullCircleTween.EditorGui;
using FullCircleTween.Extensions;
using UnityEditor;
using UnityEngine;

namespace FullCircleTween.Attributes
{
    [CustomPropertyDrawer(typeof(DropDownAttribute))]
    public class DropDownAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var dropdownAttribute = (DropDownAttribute)attribute;

            var options = AttributeDrawerUtils.GetOptions(property.serializedObject.targetObject, dropdownAttribute.optionsMemberName);
            if (property.propertyType != SerializedPropertyType.String || options == null)
            {
                base.OnGUI(position, property, label);
                return;
            }
            var methodInfo = property.serializedObject.targetObject.GetType().GetMethod(dropdownAttribute.onChangeListenerName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();
            var optionsList = options.ToList();
            EditorGuiDropDown.Draw(position, label, property, optionsList, dropdownAttribute.placeholder);
            if (string.IsNullOrEmpty(property.stringValue) && !dropdownAttribute.allowUndefined && optionsList.Count > 0)
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