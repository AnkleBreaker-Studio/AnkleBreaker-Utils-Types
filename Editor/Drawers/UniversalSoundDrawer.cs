using UnityEditor;
using UnityEngine;
using AnkleBreaker.Utils.UniversalTypes;

namespace AnkleBreaker.Utils.UniversalTypes.Editor
{
    [CustomPropertyDrawer(typeof(UniversalSound))]
    public class UniversalSoundDrawer : PropertyDrawer
    {
        private const float Spacing = 2f;

        // Show mode dropdown only if there are multiple modes available
        private static bool HasMultipleModes
        {
            get
            {
#if AB_WWISE || AB_FMOD
                return true;
#else
                return false;
#endif
            }
        }

        private const float ModeWidth = 85f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var modeProp = property.FindPropertyRelative("mode");
            var mode = (UniversalSound.SoundMode)modeProp.enumValueIndex;

            if (!HasMultipleModes)
            {
                // Only AudioClip available — draw as simple AudioClip field
                var audioClipProp = property.FindPropertyRelative("audioClip");
                EditorGUI.PropertyField(position, audioClipProp, label);
                EditorGUI.EndProperty();
                return;
            }

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
                    var audioClipProp = property.FindPropertyRelative("audioClip");
                    EditorGUI.PropertyField(valueRect, audioClipProp, GUIContent.none);
                    break;

#if AB_WWISE
                case UniversalSound.SoundMode.Wwise:
                    var wwiseEventProp = property.FindPropertyRelative("wwiseEvent");
                    EditorGUI.PropertyField(valueRect, wwiseEventProp, GUIContent.none);
                    break;
#endif

#if AB_FMOD
                case UniversalSound.SoundMode.FMOD:
                    var fmodEventProp = property.FindPropertyRelative("fmodEvent");
                    EditorGUI.PropertyField(valueRect, fmodEventProp, GUIContent.none);
                    break;
#endif
            }

            EditorGUI.EndProperty();
        }
    }
}