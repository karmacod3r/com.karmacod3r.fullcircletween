using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DataBinding.Lib.Editor;
using FullCircleTween.Extensions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FullCircleTween.EditorGui
{
    public class EditorGuiEditableList<T> : ReorderableList, IEditorGuiEditableList
    {
        public delegate void LayoutElementCallbackDelegate(
            Rect rect,
            SerializedProperty listProperty,
            SerializedProperty element,
            int index,
            bool isActive,
            bool isFocused
        );

        public LayoutElementCallbackDelegate layoutElementCallback;
        public event Action beforeAdd;
        public event Action<int> beforeDelete;
        private int controlId;

        [Serializable]
        public class PropertyList
        {
            public List<T> items = new List<T>();
        }

        public EditorGuiEditableList(string displayName, IList elements) : base(elements, typeof(T), true, true, true, true)
        {
            DisplayName = displayName;
            Init();
        }

        public EditorGuiEditableList(string displayName, IList elements, bool draggable, bool displayHeader, bool displayAddButton, bool displayRemoveButton) : base(elements, typeof(T), draggable, displayHeader, displayAddButton,
            displayRemoveButton)
        {
            DisplayName = displayName;
            Init();
        }

        public EditorGuiEditableList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements, true, true, true, true)
        {
            DisplayName = serializedProperty.displayName;
            Init();
        }

        public EditorGuiEditableList(SerializedObject serializedObject, SerializedProperty elements, bool draggable, bool displayHeader, bool displayAddButton, bool displayRemoveButton) : base(serializedObject, elements, draggable,
            displayHeader, displayAddButton, displayRemoveButton)
        {
            DisplayName = serializedProperty.displayName;
            Init();
        }

        private void Init()
        {
            drawHeaderCallback = DrawHeaderCallback;
            drawElementCallback = DrawElementCallback;
            elementHeightCallback = ElementHeightCallback;
            onAddCallback = OnAddCallback;
            onRemoveCallback = OnRemoveCallback;

            controlId = GetControlId();
        }

        private int GetControlId()
        {
            return (int)typeof(ReorderableList).GetField("id", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);
        }

        private void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, DisplayName, EditorStyles.label);
        }

        private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 1.0f;

            // set index to clicked row
            // TODO: Detect tab focus change
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                this.index = index;
            }

            EditorGUI.BeginChangeCheck();
            rect.height = elementHeightCallback(index);
            if (layoutElementCallback != null)
            {
                layoutElementCallback(rect, ListProperty, element, index, isActive, isFocused);
            }
            else
            {
                EditorGUI.PropertyField(rect, element, GUIContent.none);
            }

            if (EditorGUI.EndChangeCheck())
            {
                this.index = index;
            }
        }

        private float ElementHeightCallback(int index)
        {
            return EditorGUI.GetPropertyHeight(serializedProperty.GetArrayElementAtIndex(index));
        }

        private void DisposeAll()
        {
            if (!typeof(IDisposable).IsAssignableFrom(typeof(T))) return;

            for (var i = 0; i < serializedProperty.arraySize; i++)
            {
                var property = serializedProperty.GetArrayElementAtIndex(i);
                IDisposable item = (IDisposable)property.GetTarget();
                item.Dispose();
            }
        }

        private void OnAddCallback(ReorderableList list)
        {
            beforeAdd?.Invoke();

            DisposeAll();

            if (list.index >= 0 && serializedProperty.arraySize > 0)
            {
                DuplicateAt(list.index);

                return;
            }

            serializedProperty.arraySize++;
        }

        private void OnRemoveCallback(ReorderableList list)
        {
            if (list.index < 0) return;

            RemoveAt(index);
        }

        private void RemoveAt(int itemIndex)
        {
            beforeDelete?.Invoke(itemIndex);
            DisposeAll();

            if (serializedProperty != null)
            {
                serializedProperty.DeleteArrayElementAtIndex(itemIndex);
                if (index < serializedProperty.arraySize - 1)
                    return;
                index = serializedProperty.arraySize - 1;
            }
            else
            {
                list.RemoveAt(itemIndex);
                if (index >= list.Count - 1)
                    index = list.Count - 1;
            }
        }

        private void RemoveAll()
        {
            var listCount = serializedProperty?.arraySize ?? list.Count;
            for (var i = listCount - 1; i >= 0; i--)
            {
                RemoveAt(i);
            }
        }

        public T GetItemAt(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= serializedProperty.arraySize) return default;

            return serializedProperty.GetArrayElementAtIndex(itemIndex).CastTo<T>();
        }

        public void SetItemAt(int itemIndex, T item)
        {
            if (itemIndex < 0 || itemIndex >= serializedProperty.arraySize) return;

            serializedProperty.CastTo<List<T>>()[itemIndex] = item;
        }

        public bool DuplicateAt(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= serializedProperty.arraySize) return false;

            DisposeAll();

            var item = serializedProperty.GetArrayElementAtIndex(itemIndex);
            item.DuplicateCommand();

            return true;
        }

        private bool CopyAt(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= serializedProperty.arraySize) return false;

            var item = GetItemAt(itemIndex);
            ClipboardUtils.Push(new PropertyList { items = new List<T> { item } });

            return true;
        }

        private bool CopyAll()
        {
            if (serializedProperty.arraySize == 0) return false;

            var propertyList = new PropertyList();
            for (var i = 0; i < serializedProperty.arraySize; i++)
            {
                propertyList.items.Add(GetItemAt(i));
            }

            ClipboardUtils.Push(propertyList);

            return true;
        }

        private bool PasteAt(int itemIndex)
        {
            var clipboardItems = ClipboardUtils.Get<PropertyList>();
            if (clipboardItems?.items == null || clipboardItems.items.Count == 0) return false;

            var startIndex = Mathf.Clamp(itemIndex, 0, serializedProperty.arraySize);
            var newIndex = startIndex;
            clipboardItems.items.ForEach(item =>
            {
                serializedProperty.InsertArrayElementAtIndex(newIndex);
                newIndex++;
            });

            serializedProperty.serializedObject.ApplyModifiedProperties();

            newIndex = startIndex;
            clipboardItems.items.ForEach(item =>
            {
                SetItemAt(newIndex, item);
                newIndex++;
            });
            serializedProperty.serializedObject.ApplyModifiedProperties();

            return true;
        }

        private bool ReplaceWith()
        {
            RemoveAll();
            return PasteAt(0);
        }

        public void HandleEvents()
        {
            var type = Event.current.GetTypeForControl(controlId);
            if (GUIUtility.keyboardControl != controlId || type != EventType.KeyDown || index < 0) return;

            if (index >= 0 &&
                Event.current.keyCode == KeyCode.Delete
                || Event.current.keyCode == KeyCode.Backspace
               )
            {
                Event.current.Use();

                OnRemoveCallback(this);
                return;
            }

            if (Event.current.keyCode == KeyCode.Plus
                || (Event.current.command && Event.current.keyCode == KeyCode.Return)
               )
            {
                Event.current.Use();

                OnAddCallback(this);
                return;
            }

#if UNITY_EDITOR_OSX
            if (!Event.current.command) return;
#else
            if (!Event.current.control) return;
#endif

            switch (Event.current.keyCode)
            {
                case KeyCode.C:
                {
                    if (Event.current.shift && CopyAll())
                    {
                        Event.current.Use();
                        break;
                    }

                    if (CopyAt(index))
                    {
                        Event.current.Use();
                    }

                    break;
                }
                case KeyCode.V:
                {
                    if (Event.current.shift && ReplaceWith())
                    {
                        Event.current.Use();
                        break;
                    }

                    if (PasteAt(index + 1))
                    {
                        Event.current.Use();
                    }

                    break;
                }
                case KeyCode.D:
                {
                    if (DuplicateAt(index))
                    {
                        Event.current.Use();
                    }

                    break;
                }
            }
        }

        public new void DoLayoutList()
        {
            base.DoLayoutList();
            HandleEvents();
        }

        public new void DoList(Rect rect)
        {
            base.DoList(rect);
            HandleEvents();
        }

        public SerializedProperty ListProperty => serializedProperty;

        public bool IsValid
        {
            get
            {
                try
                {
                    return ListProperty?.isArray ?? false;
                }
                catch (NullReferenceException)
                {
                    return false;
                }
                catch (ObjectDisposedException)
                {
                    return false;
                }
            }
        }

        public string DisplayName { get; set; }

        public int SelectedIndex
        {
            get => index;
            set => index = value;
        }
    }
}