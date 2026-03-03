using UnityEditor;
using UnityEngine;
using AnkleBreaker.Utils.Types;

namespace AnkleBreaker.Utils.Types.Editor
{
    [CustomPropertyDrawer(typeof(UniversalString))]
    public class UniversalStringDrawer : PropertyDrawer
    {
        private const float ModeWidth = 120f;
        private const float Spacing = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var modeProp = property.FindPropertyRelative("mode");
            var mode = (UniversalString.StringMode)modeProp.enumValueIndex;

            float height = EditorGUIUtility.singleLineHeight;

            if (mode == UniversalString.StringMode.UnityLocalization)
                height += EditorGUIUtility.singleLineHeight + Spacing;

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var modeProp = property.FindPropertyRelative("mode");
            var plainTextProp = property.FindPropertyRelative("plainText");
            var i2TermProp = property.FindPropertyRelative("i2Term");
            var locTableProp = property.FindPropertyRelative("locTableName");
            var locEntryProp = property.FindPropertyRelative("locEntryKey");

            var mode = (UniversalString.StringMode)modeProp.enumValueIndex;

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
                case UniversalString.StringMode.PlainText:
                    EditorGUI.PropertyField(valueRect, plainTextProp, GUIContent.none);
                    break;

                case UniversalString.StringMode.I2Localize:
                    EditorGUI.PropertyField(valueRect, i2TermProp, new GUIContent("Term"));
                    break;

                case UniversalString.StringMode.UnityLocalization:
                    float halfWidth = valueWidth / 2f - Spacing / 2f;
                    Rect tableRect = new Rect(valueX, position.y, halfWidth, EditorGUIUtility.singleLineHeight);
                    Rect entryRect = new Rect(valueX + halfWidth + Spacing, position.y, halfWidth, EditorGUIUtility.singleLineHeight);

                    EditorGUI.PropertyField(tableRect, locTableProp, GUIContent.none);
                    EditorGUI.PropertyField(entryRect, locEntryProp, GUIContent.none);

                    // Hint labels below
                    float secondLineY = position.y + EditorGUIUtility.singleLineHeight + Spacing;
                    Rect tableHintRect = new Rect(valueX, secondLineY, halfWidth, EditorGUIUtility.singleLineHeight);
                    Rect entryHintRect = new Rect(valueX + halfWidth + Spacing, secondLineY, halfWidth, EditorGUIUtility.singleLineHeight);

                    var miniStyle = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.UpperLeft };
                    miniStyle.normal.textColor = new Color(0.5f, 0.5f, 0.5f);
                    EditorGUI.LabelField(tableHintRect, "Table Name", miniStyle);
                    EditorGUI.LabelField(entryHintRect, "Entry Key", miniStyle);
                    break;
            }

            EditorGUI.EndProperty();
        }
    }
}