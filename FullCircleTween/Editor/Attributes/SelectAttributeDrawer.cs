using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FullCircleTween.Extensions;
using UnityEditor;
using UnityEngine;

namespace FullCircleTween.Attributes
{
    [CustomPropertyDrawer(typeof(SelectAttribute))]
    public class SelectAttributeDrawer : PropertyDrawer
    {
        public class SelectState
        {
            public IEnumerable<string> options;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var selectAttribute = (SelectAttribute) attribute;
            var options = GetOptions(property);

            if (property.propertyType != SerializedPropertyType.String || options == null)
            {
                base.OnGUI(position, property, label);
                return;
            }

            var methodInfo = property.serializedObject.targetObject.GetType().GetMethod(
                selectAttribute.onChangeListenerName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.PrefixLabel(position, label);
            
            var buttonRect = position;
            buttonRect.x += EditorGUIUtility.labelWidth;
            buttonRect.width = position.width - buttonRect.x;
            buttonRect.height = EditorGUIUtility.singleLineHeight;
            for (var i = 0; i < options.Count(); i++)
            {
                var optionValue = options.ElementAt(i);
                var selected = property.stringValue == optionValue;
                var newValue = GUI.Toggle(buttonRect, selected, " " + options.ElementAt(i), EditorStyles.radioButton);
                if (newValue && !selected)
                {
                    property.stringValue = optionValue;
                    property.serializedObject.ApplyModifiedProperties();

                    if (methodInfo != null && methodInfo.GetParameters().Length == 0)
                    {
                        methodInfo.Invoke(property.serializedObject.targetObject, null);
                    }
                }

                buttonRect.y += EditorGUIUtility.singleLineHeight;
            }

            EditorGUI.EndProperty();
        }

        private IEnumerable<string> GetOptions(SerializedProperty property)
        {
            var controlId = GUIUtility.GetControlID(FocusType.Passive);
            var state = EditorGUIUtility.GetStateObject(typeof(SelectState), controlId) as SelectState;
            state.options = AttributeDrawerUtils.GetOptions(property.serializedObject.targetObject, (attribute as SelectAttribute).optionsMemberName);

            return state.options;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var options = GetOptions(property);
            return EditorGUIUtility.singleLineHeight * options.Count();
        }
    }
}