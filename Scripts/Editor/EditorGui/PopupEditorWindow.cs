using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace FullCircleTween.EditorGui
{
    [DefaultExecutionOrder(-10000)]
    internal class PopupEditorWindow : EditorWindow
    {
        public SerializedProperty property;
        public List<string> options;
        public int selectedIndex;

        public event Action<string> changed;
        public event Action closed;

        private int lastSelectedIndex;
        private GUIStyle buttonStyle;
        private GUIStyle selectedButtonStyle;
        private Vector2 scrollPosition = Vector2.zero;
        private float lineHeight;

        private static PopupEditorWindow instance;

        private void Awake()
        {
            instance = this;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void ProcessEvents(Event ev)
        {
            switch (ev.type)
            {
                case EventType.KeyDown:
                    switch (ev.keyCode)
                    {
                        case KeyCode.UpArrow:
                            ev.Use();
                            selectedIndex = Mathf.Max(selectedIndex - 1, 0);
                            Repaint();
                            break;
                        case KeyCode.DownArrow:
                            ev.Use();
                            selectedIndex = Mathf.Min(selectedIndex + 1, options.Count() - 1);
                            Repaint();
                            break;
                        case KeyCode.Return:
                            ev.Use();
                            Repaint();
                            if (selectedIndex < options.Count())
                            {
                                changed?.Invoke(options[selectedIndex]);
                                DoClose();
                            }

                            break;
                        case KeyCode.Escape:
                            ev.Use();
                            DoClose();
                            break;
                    }

                    break;

                case EventType.MouseDown:
                    if (!position.Contains(ev.mousePosition))
                    {
                        Close();
                    }

                    break;
            }
        }


        private void OnGUI()
        {
            if (instance == null)
            {
                Close();
                Dispose();
                DestroyImmediate(this);
                return;
            }

            try
            {
                if (property.serializedObject == null || property.propertyPath == "")
                {
                    DoClose();
                }
            }
            catch (Exception)
            {
                Close();
                Dispose();
                DestroyImmediate(this);
                return;
            }

            InitStyles();

            if (selectedIndex != lastSelectedIndex)
            {
                lastSelectedIndex = selectedIndex;

                // scroll selected into view
                if (selectedIndex * lineHeight < scrollPosition.y)
                {
                    scrollPosition.y = selectedIndex * lineHeight;
                }

                if ((selectedIndex + 1) * lineHeight > scrollPosition.y + position.height)
                {
                    scrollPosition.y = (selectedIndex + 1) * lineHeight - position.height;
                }
            }

            var controlName = GUI.GetNameOfFocusedControl();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar);
            for (var i = 0; i < options.Count; i++)
            {
                var o = options[i];

                GUI.SetNextControlName(controlName + "Option" + i);
                if (GUILayout.Button(o, (i == selectedIndex) ? selectedButtonStyle : buttonStyle))
                {
                    selectedIndex = i;
                    changed?.Invoke(o);
                    DoClose();
                }
            }

            GUILayout.EndScrollView();
        }

        private void InitStyles()
        {
            if (buttonStyle != null) return;

            buttonStyle = new GUIStyle(EditorStyles.label)
            {
                padding = new RectOffset(8, 8, 2, 2),
                margin = new RectOffset(0, 0, 0, 0),
                fontSize = EditorStyles.label.fontSize - 1
            };

            var bgColor = new Color(0.22f, 0.52f, 0.96f);
            var bgTexture = new Texture2D(2, 2);
            bgTexture.SetPixels(new[]
            {
                bgColor, bgColor, bgColor, bgColor
            });
            bgTexture.Apply();

            selectedButtonStyle = new GUIStyle(buttonStyle)
            {
                normal =
                {
                    background = bgTexture,
                    textColor = Color.white
                }
            };

            lineHeight = buttonStyle.CalcHeight(new GUIContent("A"), position.width);
        }

        public void AdjustSize(Rect rect)
        {
            if (Event.current.type == EventType.Layout) return;
            InitStyles();

            var longestOption = "";
            options.ForEach(o =>
            {
                if (o.Length > longestOption.Length)
                {
                    longestOption = o;
                }
            });
            var optionWidth = buttonStyle.CalcSize(new GUIContent(longestOption)).x;

            rect.width = Mathf.Max(optionWidth + 8, rect.width);
            rect.height = Mathf.Min(options.Count(), 4) * lineHeight;
            position = rect;
        }

        private void DoClose()
        {
            Close();
            closed?.Invoke();
        }

        public void Dispose()
        {
            property = null;
            changed = null;
            closed = null;
        }

        public void ShowPopup()
        {
            var showAsDropDown = typeof(EditorWindow)
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).First(info => info.GetParameters().Length == 5);
            showAsDropDown.Invoke(this, new object[] { position, new Vector2(position.width, position.height), null, 1, false });
        }
    }
}