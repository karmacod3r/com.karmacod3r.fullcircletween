using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FullCircleTween.Extensions;
using UnityEditor;
using UnityEngine;

namespace FullCircleTween.Attributes
{
    [CustomPropertyDrawer(typeof(DynamicDropDownAttribute))]
    public class DynamicDropDownAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var dynamicDropdownAttribute = (DynamicDropDownAttribute)attribute;

            var options = AttributeDrawerUtils.GetOptions(property.serializedObject.targetObject, dynamicDropdownAttribute.optionsMemberName);
            if (property.propertyType != SerializedPropertyType.String || options == null)
            {
                base.OnGUI(position, property, label);
                return;
            }
            var methodInfo = property.serializedObject.targetObject.GetType().GetMethod(dynamicDropdownAttribute.onChangeListenerName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();
            var optionsList = options.ToList();
            var index = EditorGUI.Popup(position, label.text, optionsList.IndexOf(property.stringValue), options.ToArray());
            if (index < 0 && !dynamicDropdownAttribute.allowUndefined && optionsList.Count > 0)
            {
                property.stringValue = optionsList[0];
            }
            if (EditorGUI.EndChangeCheck())
            {
                property.stringValue = index >= 0 && index < optionsList.Count ? optionsList[index] 
                    : dynamicDropdownAttribute.allowUndefined || optionsList.Count == 0 ? "" : optionsList[0];
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