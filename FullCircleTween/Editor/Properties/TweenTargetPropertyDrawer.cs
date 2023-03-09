using System;
using System.Collections.Generic;
using System.Linq;
using FullCircleTween.Core;
using UnityEditor;
using UnityEngine;

namespace FullCircleTween.Properties
{
    [CustomPropertyDrawer(typeof(TweenTarget))]
    public class TweenTargetPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var target = property.serializedObject.targetObject;
            var typeName = property.FindPropertyRelative("typeName");
            
            typeName.stringValue = ComponentPopup(position, (target as Component).transform, typeName.stringValue, label.text);
            
            EditorGUI.EndProperty();
        }

        private static string[] GetBehaviourNames(MonoBehaviour target)
        {
            return target.GetComponents<Behaviour>().ToList()
                .Select(b => b.GetType().Name)
                .ToArray();
        }
        
        private static string ComponentPopup(Rect position, Transform root, string value, string label)
        {
            List<string> componentTypeAssemblyNames = null;
            
            if (root != null)
            {
                var targetComponents = root.GetComponents<Component>();

                componentTypeAssemblyNames = targetComponents.ToList()
                    .Where(component => component != null)
                    .Where(component =>
                    {
                        var baseTypes = GetAllBaseTypesAssemblyQualifiedNames(component.GetType().AssemblyQualifiedName);
                        return TweenMethodCache.allComponentTypeNames.Any(baseTypes.Contains);
                    })
                    .Select(component => TweenTarget.Serialize(component))
                    .ToList();
            } else
            {
                componentTypeAssemblyNames = new List<string>(TweenMethodCache.allComponentTypeNames); 
            }

            var valueAdded = false;
            if (!string.IsNullOrEmpty(value) && !componentTypeAssemblyNames.Contains(value))
            {
                valueAdded = true;
                componentTypeAssemblyNames.Add(value);
            }
            
            // componentTypeAssemblyNames.Sort();
            var selectedIndex = Mathf.Max(componentTypeAssemblyNames.IndexOf(value), 0);

            var typeDisplayNames = componentTypeAssemblyNames.Select(assemblyName =>
            {
                var parts = assemblyName.Split('^');
                var ret = Type.GetType(parts[0]).FullName.Replace("UnityEngine.", "")
                    + (parts.Length > 1 ? $"[{parts[1]}]" : "");
                if (valueAdded && assemblyName == value)
                {
                    ret = "-- " + ret;
                }
                return ret;
            }).ToArray();

            selectedIndex = EditorGUI.Popup(position, label, selectedIndex, typeDisplayNames);

            return selectedIndex == -1 ? "" : componentTypeAssemblyNames[selectedIndex];
        }
        
        private static List<string> GetAllBaseTypesAssemblyQualifiedNames(string typeName)
        {
            var types = new List<string>();
            var t = Type.GetType(typeName);
            while (t != null)
            {
                types.Add(t.AssemblyQualifiedName);
                t = t.BaseType;
            }

            return types;
        }
    }
}