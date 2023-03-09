using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FullCircleTween.Extensions;
using UnityEditor;
using UnityEngine;

namespace FullCircleTween.Attributes
{
    [CustomPropertyDrawer(typeof(OnChangeAttribute))]
    public class OnChangeAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var onChangeAttribute = (OnChangeAttribute)attribute;
            var methodInfo = property.serializedObject.targetObject.GetType().GetMethod(onChangeAttribute.listenerName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label);
            if (EditorGUI.EndChangeCheck() && methodInfo != null && methodInfo.GetParameters().Length == 0)
            {
                methodInfo.Invoke(property.serializedObject.targetObject, null);
            }
            EditorGUI.EndProperty();
        }

        private IEnumerable<string> GetOptions(object target, string memberName)
        {
            var type = target.GetType();
            object value = null;

            var field = type.GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            if (field != null)
            {
                value = field.GetValue(target);
            } else
            {
                var property = type.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
                if (property != null)
                {
                    value = property.GetValue(target);
                } else
                {
                    var method = type.GetMethod(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
                    if (method != null && method.ReturnParameter != null && method.GetParameters().Length == 0)
                    {
                        value = method.Invoke(target, null);
                    }
                }
            }

            return value as IEnumerable<string>;
        }
    }
}