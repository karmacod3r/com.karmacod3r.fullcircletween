using UnityEditor;
using UnityEngine;

namespace FullCircleTween.EditorGui
{
    public interface IEditorGuiEditableList
    {
        void DoLayoutList();
        void DoList(Rect rect);
        SerializedProperty ListProperty { get; }
        public bool IsValid { get; }
        string DisplayName { get; set; }
        int SelectedIndex { get; set; }
    }
}