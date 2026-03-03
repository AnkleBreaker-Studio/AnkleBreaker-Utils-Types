using UnityEditor;
using UnityEngine;
using AnkleBreaker.Utils.UniversalTypes;

namespace AnkleBreaker.Utils.UniversalTypes.Editor
{
    [CustomPropertyDrawer(typeof(UniversalSound))]
    public class UniversalSoundDrawer : PropertyDrawer
    {
        private const float ModeWidth = 100f;
        private const float Spacing = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var modeProp = property.FindPropertyRelative("mode");
            var audioClipProp = property.FindPropertyRelative("audioClip");
            var wwiseEventProp = property.FindPropertyRelative("wwiseEventName");
            var fmodEventProp = property.FindPropertyRelative("fmodEventPath");

            var mode = (UniversalSound.SoundMode)modeProp.enumValueIndex;

            // Label
            Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelRect, label);

            float fieldX = position.x + EditorGUIUtility.labelWidth + Spacing;
            float fieldWidth = position.width - EditorGUIUtility.labelWidth - Spacing;

            // Mode dropdown
            Rect modeRect = new Rect(fieldX, position.y, ModeWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(modeRect, modeProp, GUIContent.none);

            // Value field
            float valueX = fieldX + ModeWidth + Spacing;
            float valueWidth = fieldWidth - ModeWidth - Spacing;
            Rect valueRect = new Rect(valueX, position.y, valueWidth, EditorGUIUtility.singleLineHeight);

            switch (mode)
            {
                case UniversalSound.SoundMode.AudioClip:
                    EditorGUI.PropertyField(valueRect, audioClipProp, GUIContent.none);
                    break;

                case UniversalSound.SoundMode.Wwise:
                    wwiseEventProp.stringValue = EditorGUI.TextField(valueRect, wwiseEventProp.stringValue);
                    if (string.IsNullOrEmpty(wwiseEventProp.stringValue))
                        DrawPlaceholder(valueRect, "Wwise Event Name (e.g. Play_UI_Click)");
                    break;

                case UniversalSound.SoundMode.FMOD:
                    fmodEventProp.stringValue = EditorGUI.TextField(valueRect, fmodEventProp.stringValue);
                    if (string.IsNullOrEmpty(fmodEventProp.stringValue))
                        DrawPlaceholder(valueRect, "FMOD Event Path (e.g. event:/UI/Click)");
                    break;
            }

            EditorGUI.EndProperty();
        }

        private static void DrawPlaceholder(Rect rect, string placeholder)
        {
            var style = new GUIStyle(EditorStyles.label);
            style.normal.textColor = new Color(0.5f, 0.5f, 0.5f, 0.6f);
            style.fontStyle = FontStyle.Italic;
            EditorGUI.LabelField(rect, placeholder, style);
        }
    }
}