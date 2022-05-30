using UnityEditor;
using UnityEngine;

namespace SK.Framework
{
    [CustomPropertyDrawer(typeof(KeyInput))]
    public class KeyInputPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(new Rect(position.x, position.y, position.width * .4f, position.height), label);
            var keyProperty = property.FindPropertyRelative("key");
            keyProperty.enumValueIndex = EditorGUI.Popup(
                new Rect(position.x + position.width * .4f, position.y, position.width * .6f, position.height),
                keyProperty.enumValueIndex, keyProperty.enumDisplayNames);
        }
    }
}