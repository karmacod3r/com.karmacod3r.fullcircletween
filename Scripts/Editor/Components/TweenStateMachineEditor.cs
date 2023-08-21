using System;
using System.Linq;
using System.Threading.Tasks;
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
        private static TweenStateMachineEditor firstInstance;
        private static TweenStateMachine currentTarget;
        private static TweenStateMachine recordingTweenStateMachine;

        internal static void ToggleRecording()
        {
            if (recordingTweenStateMachine == null)
            {
                recordingTweenStateMachine = firstInstance?.target as TweenStateMachine;
            }
            else
            {
                recordingTweenStateMachine = null;
            }

            if (firstInstance != null)
            {
                firstInstance.Repaint();
            }
        }

        private class TweenStateMachineEditorState
        {
            public EditorGuiEditableList<TweenState> editableList;
            public string propertyPath;
        }

        private GUIStyle richTextStyle;

        private const string MatchOperatorColor = "#aa0000";
        private const string DefaultStateName = "New State";
        private const float DefaultTweenDuration = 0.2f;


        private TweenStateMachineEditorState state;


        private void OnEnable()
        {
            Undo.postprocessModifications -= PostProcessModifications;
            Undo.postprocessModifications += PostProcessModifications;

            currentTarget = target as TweenStateMachine;
            if (firstInstance == null)
            {
                firstInstance = this;
            }
        }

        private void OnDisable()
        {
            if (recordingTweenStateMachine == target)
            {
                recordingTweenStateMachine = null;
            }

            if (currentTarget == target as TweenStateMachine)
            {
                currentTarget = null;
            }

            Undo.postprocessModifications -= PostProcessModifications;
            firstInstance = null;
        }

        private UndoPropertyModification[] PostProcessModifications(UndoPropertyModification[] modifications)
        {
            if (!IsRecording) return modifications;

            var visibleTweenState = GetVisibleTweenState(serializedObject.FindProperty("tweenStates"));
            var tweenState = visibleTweenState.GetTarget() as TweenState;
            modifications.ToList().ForEach(mod => AddOrUpdateTweenClip(mod, visibleTweenState, tweenState));

            // this delay is needed for the undo to work properly
            ApplyModifiedPropertiesDelayed();

            return modifications;
        }

        private async Task ApplyModifiedPropertiesDelayed()
        {
            await Task.Delay(1);
            serializedObject.ApplyModifiedProperties();
        }

        public bool IsRecording
        {
            get { return recordingTweenStateMachine == target; }
            set
            {
                if (value)
                {
                    recordingTweenStateMachine = target as TweenStateMachine;
                }
                else if (recordingTweenStateMachine == target)
                {
                    recordingTweenStateMachine = null;
                }
            }
        }

        private void AddOrUpdateTweenClip(UndoPropertyModification mod, SerializedProperty visibleTweenState,
            TweenState tweenState)
        {
            var propertyPath = TweenMethodCache.GetFirstLevelPropertyPath(mod.currentValue.propertyPath);
            Debug.Log(mod.currentValue.target.GetType().Name + " : " + propertyPath);

            var context = mod.currentValue.target as Component;
            var info = TweenMethodCache.GetTweenMethodInfoForPropertyPath(mod.currentValue.target.GetType(),
                propertyPath);
            if (info != null)
            {
                var methodName = TweenMethodCache.GetMethodName(info);
                var visibleClips = visibleTweenState.FindPropertyRelative(nameof(TweenState.tweenGroup))
                    .FindPropertyRelative(nameof(TweenGroupClip.tweenClips)).FindPropertyRelative("list");

                // find existing clip with same target and method name
                var clipIndex = FindSerializedClip(context, visibleClips, methodName);

                var newClipDuration = tweenState.tweenGroup.tweenClips.Count > 0
                    ? tweenState.tweenGroup.tweenClips.Last().duration
                    : DefaultTweenDuration;

                var newClip = false;
                if (clipIndex < 0)
                {
                    visibleClips.arraySize++;

                    clipIndex = visibleClips.arraySize - 1;
                    newClip = true;
                }

                var clipProperty = visibleClips.GetArrayElementAtIndex(clipIndex);

                if (newClip)
                {
                    clipProperty.FindPropertyRelative(nameof(TweenClip.tweenTarget))
                            .FindPropertyRelative(nameof(TweenTarget.typeName)).stringValue =
                        TweenTarget.Serialize(mod.currentValue.target as Component);

                    clipProperty.FindPropertyRelative(nameof(TweenClip.tweenMethodName)).stringValue = methodName;
                    clipProperty.FindPropertyRelative(nameof(TweenClip.duration)).floatValue = newClipDuration;
                }

                // set tween toValue
                clipProperty.FindPropertyRelative(nameof(TweenClip.toValue))
                        .FindPropertyRelative(nameof(TweenClipValue.serializedValue)).stringValue =
                    TweenClipValue.Serialize(GetCurrentTweenValue(context, clipProperty));
            }
        }

        private object GetCurrentTweenValue(Component context, SerializedProperty clip)
        {
            var tweenTarget = TweenTarget.Deserialize(context, clip.FindPropertyRelative(nameof(TweenClip.tweenTarget))
                .FindPropertyRelative(nameof(TweenTarget.typeName)).stringValue);
            var tweenMethod = clip.FindPropertyRelative(nameof(TweenClip.tweenMethodName)).stringValue;
            var tweenToValueType =
                TweenMethodCache.GetMethodTweenedType(
                    TweenMethodCache.GetTweenMethodInfo(tweenTarget.GetType(), tweenMethod));
            var tweenToValue = TweenClipValue.Deserialize(tweenToValueType,
                clip.FindPropertyRelative(nameof(TweenClip.tweenMethodName)).stringValue);

            var tween = TweenMethodCache.CreateTween(tweenTarget, tweenMethod, tweenToValue, 1);
            var value = tween?.GetCurrentValue();
            tween.Kill();

            return value;
        }

        private static int FindSerializedClip(Component context, SerializedProperty visibleClips, string methodName)
        {
            var modTargetTypeName = TweenTarget.Serialize(context);
            for (var i = 0; i < visibleClips.arraySize; i++)
            {
                var element = visibleClips.GetArrayElementAtIndex(i);
                var tweenTargetTypeName = element.FindPropertyRelative(nameof(TweenClip.tweenTarget))
                    .FindPropertyRelative(nameof(TweenTarget.typeName));
                var tweenMethodName = element.FindPropertyRelative(nameof(TweenClip.tweenMethodName));
                if (tweenTargetTypeName.stringValue == modTargetTypeName
                    && tweenMethodName.stringValue == methodName)
                {
                    return i;
                }
            }

            return -1;
        }

        public override void OnInspectorGUI()
        {
            InitStyles();

            var tweenStateMachine = target as TweenStateMachine;

            var controlID = GUIUtility.GetControlID(FocusType.Passive);
            state =
                GUIUtility.GetStateObject(typeof(TweenStateMachineEditorState), controlID) as
                    TweenStateMachineEditorState;

            var currentState = serializedObject.FindProperty("currentState");
            var controlledByParent = serializedObject.FindProperty("controlledByParent");
            var tweenStates = serializedObject.FindProperty("tweenStates");
            var index = tweenStateMachine.StateNames?.ToList().IndexOf(currentState.stringValue) ?? -1;

            DrawRecordingButton();
            EditorGUILayout.PropertyField(controlledByParent);
            EditorGUILayout.Space();
            DrawStateList(tweenStates, currentState, index);

            var visibleTweenState = GetVisibleTweenState(tweenStates);

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

        private SerializedProperty GetVisibleTweenState(SerializedProperty tweenStates)
        {
            var editIndex = state.editableList.SelectedIndex;
            var visibleTweenState = editIndex > -1 ? tweenStates.GetArrayElementAtIndex(editIndex) : null;
            return visibleTweenState;
        }

        private void DrawRecordingButton()
        {
            IsRecording = GUILayout.Toggle(IsRecording, "Record", EditorStyles.miniButton);
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
            if (state.editableList == null || !state.editableList.IsValid ||
                property.propertyPath != state.propertyPath)
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

        private void TweenStateListDrawElementCallback(Rect rect, SerializedProperty listProperty,
            SerializedProperty element, int index, bool isActive, bool isFocused)
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
                    return $"<color=\"{MatchOperatorColor}\">!= /{tweenState.stateRegex}/</color> " +
                           tweenState.stateName;
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