using System;
using System.Linq;
using FullCircleTween.Core;
using FullCircleTween.EditorGui;
using FullCircleTween.Extensions;
using FullCircleTween.Properties;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FullCircleTween.Components
{
    [CustomEditor(typeof(TweenStateMachine))]
    public class TweenStateMachineEditor : Editor
    {
        private class TweenStateMachineEditorState
        {
            public EditorGuiEditableList<TweenState> editableList;
            public string propertyPath;
        }

        private GUIStyle richTextStyle;
        
        private const string MatchOperatorColor = "#aa0000";
        private const string StateDefaultName = "New State";
        
        private TweenStateMachineEditorState state;

        public override void OnInspectorGUI()
        {
            InitStyles();
            
            var tweenStateMachine = target as TweenStateMachine;
            
            var controlID = GUIUtility.GetControlID(FocusType.Passive);
            state = GUIUtility.GetStateObject(typeof(TweenStateMachineEditorState), controlID) as TweenStateMachineEditorState;

            var currentState = serializedObject.FindProperty("currentState");
            var controlledByParent = serializedObject.FindProperty("controlledByParent");
            var tweenStates = serializedObject.FindProperty("tweenStates");
            var index = tweenStateMachine.StateNames?.ToList().IndexOf(currentState.stringValue) ?? -1;
            
            EditorGUILayout.PropertyField(controlledByParent);
            EditorGUILayout.Space();
            var editIndex = DrawStateList(tweenStates, currentState, index);
            
            var visibleTweenState = editIndex > -1 ? tweenStates.GetArrayElementAtIndex(editIndex) : null;

            var currentStateValue = currentState.stringValue;
            var visibleTweenStateName = GetTweenStateName(visibleTweenState);
            
            if (visibleTweenState != null)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Edit State", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(visibleTweenState);

                if (EditorGUI.EndChangeCheck())
                {
                    var newStateName = GetTweenStateName(visibleTweenState);
                    
                    // did the state name change?
                    if (visibleTweenStateName != newStateName)
                    {
                        // apply name change to current state value
                        if (currentStateValue == visibleTweenStateName)
                        {
                            currentState.stringValue = newStateName;
                        }
                    }
                    
                    serializedObject.ApplyModifiedProperties();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void InitStyles()
        {
            if (richTextStyle == null)
            {
                richTextStyle = new(GUI.skin.label)
                {
                    richText = true
                };
            }
        }

        private int DrawStateList(SerializedProperty listProperty, SerializedProperty property, int index)
        {
            if (state.editableList == null || !state.editableList.IsValid || property.propertyPath != state.propertyPath)
            {
                state.editableList = new EditorGuiEditableList<TweenState>(listProperty.serializedObject, listProperty);
                state.editableList.DisplayName = listProperty.displayName;
                state.editableList.drawElementBackgroundCallback = TweenStateListDrawElementBackgroundCallback;
                state.editableList.layoutElementCallback = TweenStateListDrawElementCallback;
                state.editableList.elementHeightCallback = TweenStateListElementHeightCallback;
                state.propertyPath = property.propertyPath;
            }

            if (state.editableList.index < 0)
            {
                state.editableList.index = index;
            }
            state.editableList.DoLayoutList();

            return state.editableList.SelectedIndex;
        }

        private void TweenStateListDrawElementBackgroundCallback(Rect rect, int index, bool isactive, bool isfocused)
        {
            if (Event.current.type != EventType.Repaint) return;
            
            var selected = index == state.editableList.index;
            ReorderableList.defaultBehaviours.DrawElementBackground(rect, index, selected, selected, selected);
        }

        private float TweenStateListElementHeightCallback(int index)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        private void TweenStateListDrawElementCallback(Rect rect, SerializedProperty listProperty, SerializedProperty element, int index, bool isActive, bool isFocused)
        {
            var tweenState = element.CastTo<TweenState>();
            var stateName = element.FindPropertyRelative("stateName");
            var currentState = serializedObject.FindProperty("currentState");
            
            const float buttonWidth = 32f;

            var labelRect = rect;
            labelRect.width -= buttonWidth;
            EditorGUI.LabelField(labelRect, GetTweenStateLabel(tweenState), richTextStyle);

            var buttonRect = labelRect;
            buttonRect.x = labelRect.xMax;
            buttonRect.width = buttonWidth;

            var stateMachine = target as TweenStateMachine;
            var selected = stateMachine.GetState(currentState.stringValue) == tweenState;
            var newSelected = GUI.Toggle(buttonRect, selected, "", EditorStyles.radioButton);
            if (newSelected && !selected)
            {
                SetCurrentTweenState(stateName.stringValue);
            }
        }

        private string GetTweenStateLabel(TweenState tweenState)
        {
            if (tweenState == null) return "";

            switch (tweenState.matchType)
            {
                case TweenState.TweenStateMatchType.Equal:
                    return tweenState.stateName;
                case TweenState.TweenStateMatchType.NotEqual:
                    return $"<color=\"{MatchOperatorColor}\">!=</color> " + tweenState.stateName;
                case TweenState.TweenStateMatchType.RegexMatch:
                    return $"<color=\"{MatchOperatorColor}\">/{tweenState.stateRegex}/</color> " + tweenState.stateName;
                case TweenState.TweenStateMatchType.RegexNoMatch:
                    return $"<color=\"{MatchOperatorColor}\">!= /{tweenState.stateRegex}/</color> " + tweenState.stateName;
                case TweenState.TweenStateMatchType.CatchAll:
                    return $"<color=\"{MatchOperatorColor}\">*</color> " + tweenState.stateName;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return "";
        }

        private void SetCurrentTweenState(string stateName)
        {
            var tweenStateMachine = target as TweenStateMachine;
            tweenStateMachine.CurrentState = stateName;
            serializedObject.ApplyModifiedProperties();
        }

        private static string GetTweenStateName(SerializedProperty tweenStateProperty)
        {
            return tweenStateProperty?.FindPropertyRelative(nameof(TweenState.stateName))?.stringValue;
        }
    }
}