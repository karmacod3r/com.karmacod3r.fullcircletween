using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FullCircleTween.Components
{
    [CustomEditor(typeof(TweenPlayer))]
    public class TweenPlayerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var tweenPlayer = target as TweenPlayer;

            if (GUILayout.Button("Play"))
            {
                tweenPlayer.Play();
            }

            if (GUILayout.Button("Pause"))
            {
                tweenPlayer.Pause();
            }
        }
    }
}