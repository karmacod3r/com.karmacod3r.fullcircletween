using FullCircleTween.Core.Interfaces;
using FullCircleTween.Test;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FullCircleTween.Components
{
    // [CustomEditor(typeof(TweenPlayer))]
    public class TweenPlayerEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();
 
            var iterator = serializedObject.GetIterator();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                    container.Add(new PropertyField(iterator));
            }
            
            return container;
        }
 
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}