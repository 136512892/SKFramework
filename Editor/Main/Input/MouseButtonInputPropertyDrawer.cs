using UnityEditor;
using UnityEngine;

namespace SK.Framework
{
    [CustomPropertyDrawer(typeof(MouseButtonInput))]
    public class MouseButtonInputPropertyDrawer : PropertyDrawer
    {
        private readonly static string[] mouseButtonNames = new string[3] 
        { 
            "MouseButton Left", 
            "MouseButton Right",
            "MouseButton Middle" 
        };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(new Rect(position.x, position.y, position.width * .4f, position.height), label);
            var keyProperty = property.FindPropertyRelative("key");
            keyProperty.intValue = EditorGUI.Popup(
                new Rect(position.x + position.width * .4f, position.y, position.width * .6f, position.height),
                keyProperty.intValue, mouseButtonNames);
        }
    }
}