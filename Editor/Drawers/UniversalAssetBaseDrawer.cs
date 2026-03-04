using UnityEditor;
using UnityEngine;
using AnkleBreaker.Utils.UniversalTypes;

namespace AnkleBreaker.Utils.UniversalTypes.Editor
{
    [CustomPropertyDrawer(typeof(UniversalAssetBase), true)]
    public class UniversalAssetBaseDrawer : PropertyDrawer
    {
#if AB_ADDRESSABLES
        private const float ModeWidth = 90f;
#endif
        private const float Spacing = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var directRefProp = property.FindPropertyRelative("directReference");

#if AB_ADDRESSABLES
            var modeProp = property.FindPropertyRelative("mode");
            var addressableRefProp = property.FindPropertyRelative("addressableRef");
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
                    break;

                case UniversalAssetBase.AssetMode.Addressable:
                    if (addressableRefProp != null)
                        EditorGUI.PropertyField(valueRect, addressableRefProp, GUIContent.none);
                    break;
            }
#else
            // Without Addressables, just show the direct reference field normally
            if (directRefProp != null)
                EditorGUI.PropertyField(position, directRefProp, label);
            else
                EditorGUI.LabelField(position, label, new GUIContent("(no direct reference field)"));
#endif

            EditorGUI.EndProperty();
        }
    }
}