using FullCircleTween.Components;
using UnityEditor;
using UnityEngine;

namespace FullCircleTween.Properties
{
    [CustomPropertyDrawer(typeof(TweenState))]
    public class TweenStateEditor : NoLabelPropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var matchType = property.FindPropertyRelative(nameof(TweenState.matchType));
            var stateName = property.FindPropertyRelative(nameof(TweenState.stateName));
            var stateRegex = property.FindPropertyRelative("stateRegex");
            var tweenGroup = property.FindPropertyRelative(nameof(TweenState.tweenGroup));
            
            EditorGUILayout.PropertyField(matchType);
            EditorGUILayout.PropertyField(stateName);
            var isRegex = matchType.enumValueIndex == (int) TweenState.TweenStateMatchType.RegexMatch
                 || matchType.enumValueIndex == (int) TweenState.TweenStateMatchType.RegexNoMatch;
            if (isRegex)
            {
                EditorGUILayout.PropertyField(stateRegex);
            }
            EditorGUILayout.PropertyField(tweenGroup);
            
            EditorGUI.EndProperty();
        }
    }
}