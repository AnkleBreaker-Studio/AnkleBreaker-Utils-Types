using UnityEditor;
using UnityEngine;

namespace AnkleBreaker.Utils.UniversalTypes.Editor
{
    [CustomPropertyDrawer(typeof(UniversalString))]
    public class UniversalStringDrawer : PropertyDrawer
    {
        private const float ToggleWidth = 18f;
        private const float LabelLocWidth = 58f;
        private const float Spacing = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var plainTextProp = property.FindPropertyRelative("plainText");
#if AB_I2_LOCALIZE || AB_UNITY_LOCALIZATION
            var isLocalizedProp = property.FindPropertyRelative("isLocalized");
            bool isLocalized = isLocalizedProp.boolValue;

            // Label
            Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelRect, label);

            float fieldX = position.x + EditorGUIUtility.labelWidth + Spacing;
            float fieldWidth = position.width - EditorGUIUtility.labelWidth - Spacing;

            // Toggle + "Loc" label
            Rect toggleRect = new Rect(fieldX, position.y, ToggleWidth, EditorGUIUtility.singleLineHeight);
            isLocalizedProp.boolValue = EditorGUI.Toggle(toggleRect, isLocalized);

            Rect locLabelRect = new Rect(fieldX + ToggleWidth, position.y, LabelLocWidth, EditorGUIUtility.singleLineHeight);
            var miniStyle = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleLeft };
            EditorGUI.LabelField(locLabelRect, "Localized", miniStyle);

            // Value field
            float valueX = fieldX + ToggleWidth + LabelLocWidth + Spacing;
            float valueWidth = fieldWidth - ToggleWidth - LabelLocWidth - Spacing;
            Rect valueRect = new Rect(valueX, position.y, valueWidth, EditorGUIUtility.singleLineHeight);

            if (!isLocalized)
            {
                EditorGUI.PropertyField(valueRect, plainTextProp, GUIContent.none);
            }
            else
            {
#if AB_I2_LOCALIZE
                // Draw using I2L's native LocalizedStringDrawer (popup with term selection)
                var i2Prop = property.FindPropertyRelative("i2LocalizedString");
                EditorGUI.PropertyField(valueRect, i2Prop, GUIContent.none);
#elif AB_UNITY_LOCALIZATION
                var locStringProp = property.FindPropertyRelative("unityLocalizedString");
                EditorGUI.PropertyField(valueRect, locStringProp, GUIContent.none);
#endif
            }
#else
            // No localization package: just a plain string field
            EditorGUI.PropertyField(position, plainTextProp, label);
#endif

            EditorGUI.EndProperty();
        }
    }
}
