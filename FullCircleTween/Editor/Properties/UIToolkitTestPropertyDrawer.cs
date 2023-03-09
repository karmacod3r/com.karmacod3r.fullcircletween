using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FullCircleTween.Properties
{
    [CustomPropertyDrawer(typeof(UIToolkitTest))]
    public class UIToolkitTestPropertyDrawer : PropertyDrawer
    {
        private static string uxmlPath = "Assets/Scripts/FullCircleTween/Editor/UI/Test.uxml";
        private static Button button;
        private static Label label;
        private static TextField textField;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
            VisualElement ui = uiAsset.CloneTree(property.propertyPath);
 
            // get ui elements by name
            button = ui.Q<Button>("SampleButton");
 
            // add event handler
            button.clickable.clicked += Button_clicked;

            return ui;
        }

        private static void Button_clicked()
        {
            Debug.Log("Button clicked");
        }
    }
}