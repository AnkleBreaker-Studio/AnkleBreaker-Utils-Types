using UnityEditor;
using UnityEngine;
using AnkleBreaker.Utils.UniversalTypes;

namespace AnkleBreaker.Utils.UniversalTypes.Editor
{
    [CustomPropertyDrawer(typeof(UniversalString))]
    public class UniversalStringDrawer : PropertyDrawer
    {
        private const float ModeWidth = 130f;
        private const float Spacing = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
#if AB_UNITY_LOCALIZATION
            var modeProp = property.FindPropertyRelative("mode");
            if (modeProp.enumValueIndex == (int)UniversalString.StringMode.UnityLocalization)
                return EditorGUIUtility.singleLineHeight * 2f + Spacing;
#endif
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var modeProp = property.FindPropertyRelative("mode");

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

            var mode = (UniversalString.StringMode)modeProp.enumValueIndex;

            switch (mode)
            {
                case UniversalString.StringMode.PlainText:
                    var plainTextProp = property.FindPropertyRelative("plainText");
                    EditorGUI.PropertyField(valueRect, plainTextProp, GUIContent.none);
                    break;

#if AB_I2_LOCALIZE
                case UniversalString.StringMode.I2Localize:
                    var i2TermProp = property.FindPropertyRelative("i2Term");
                    i2TermProp.stringValue = EditorGUI.TextField(valueRect, i2TermProp.stringValue);
                    if (string.IsNullOrEmpty(i2TermProp.stringValue))
                        DrawPlaceholder(valueRect, "i2 Term (e.g. UI/Title)");
                    break;
#endif

#if AB_UNITY_LOCALIZATION
                case UniversalString.StringMode.UnityLocalization:
                    var locTableProp = property.FindPropertyRelative("locTableName");
                    var locEntryProp = property.FindPropertyRelative("locEntryKey");

                    float halfWidth = valueWidth / 2f - Spacing / 2f;
                    Rect tableRect = new Rect(valueX, position.y, halfWidth, EditorGUIUtility.singleLineHeight);
                    Rect entryRect = new Rect(valueX + halfWidth + Spacing, position.y, halfWidth, EditorGUIUtility.singleLineHeight);

                    locTableProp.stringValue = EditorGUI.TextField(tableRect, locTableProp.stringValue);
                    locEntryProp.stringValue = EditorGUI.TextField(entryRect, locEntryProp.stringValue);

                    if (string.IsNullOrEmpty(locTableProp.stringValue))
                        DrawPlaceholder(tableRect, "Table Name");
                    if (string.IsNullOrEmpty(locEntryProp.stringValue))
                        DrawPlaceholder(entryRect, "Entry Key");

                    // Hint labels below
                    float secondLineY = position.y + EditorGUIUtility.singleLineHeight + Spacing;
                    var miniStyle = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.UpperLeft };
                    miniStyle.normal.textColor = new Color(0.5f, 0.5f, 0.5f);
                    EditorGUI.LabelField(new Rect(valueX, secondLineY, halfWidth, EditorGUIUtility.singleLineHeight), "Table Name", miniStyle);
                    EditorGUI.LabelField(new Rect(valueX + halfWidth + Spacing, secondLineY, halfWidth, EditorGUIUtility.singleLineHeight), "Entry Key", miniStyle);
                    break;
#endif
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