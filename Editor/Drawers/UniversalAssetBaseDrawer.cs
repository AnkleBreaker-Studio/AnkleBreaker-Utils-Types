using UnityEditor;
using UnityEngine;
using AnkleBreaker.Utils.Types;

namespace AnkleBreaker.Utils.Types.Editor
{
    [CustomPropertyDrawer(typeof(UniversalAssetBase), true)]
    public class UniversalAssetBaseDrawer : PropertyDrawer
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
            var directRefProp = property.FindPropertyRelative("directReference");
            var addressableKeyProp = property.FindPropertyRelative("addressableKey");

            var mode = (UniversalAssetBase.AssetMode)modeProp.enumValueIndex;

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
                case UniversalAssetBase.AssetMode.Direct:
                    if (directRefProp != null)
                        EditorGUI.PropertyField(valueRect, directRefProp, GUIContent.none);
                    else
                        EditorGUI.LabelField(valueRect, "(no direct reference field)");
                    break;

                case UniversalAssetBase.AssetMode.Addressable:
                    addressableKeyProp.stringValue = EditorGUI.TextField(valueRect, addressableKeyProp.stringValue);
                    break;
            }

            EditorGUI.EndProperty();
        }
    }
}