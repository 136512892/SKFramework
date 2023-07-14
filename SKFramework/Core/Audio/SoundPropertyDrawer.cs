#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SK.Framework.Audio
{
    [CustomPropertyDrawer(typeof(Sound))]
    public class SoundPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect labelRect = new Rect(position.x, position.y, position.width, position.height * .5f);
            EditorGUI.LabelField(labelRect, label);

            EditorGUI.indentLevel++;

            Rect sourceLabelRect = new Rect(position.x, position.y + position.height * .5f, position.width * .15f, position.height * .5f);
            EditorGUI.LabelField(sourceLabelRect, "Source");

            var source = property.FindPropertyRelative("source");
            Rect sourceValueRect = new Rect(position.x + position.width * .15f, position.y + position.height * .5f + 3f, position.width * .3f, position.height * .5f);
            int newSourceValue = EditorGUI.Popup(sourceValueRect, source.enumValueIndex, source.enumNames);
            if (newSourceValue != source.enumValueIndex)
            {
                source.enumValueIndex = newSourceValue;
            }

            switch (source.enumValueIndex)
            {
                case 0:
                    var audioClip = property.FindPropertyRelative("audioClip");
                    Rect audioClipRect = new Rect(position.x + position.width * .45f, position.y + position.height * .5f + 3f, position.width * .55f, position.height * .5f - 4f);
                    EditorGUI.PropertyField(audioClipRect, audioClip, GUIContent.none);
                    break;
                case 1:
                    var databaseName = property.FindPropertyRelative("databaseName");
                    var clipName = property.FindPropertyRelative("clipName");
                    Rect databaseNameRect = new Rect(position.x + position.width * .45f, position.y + position.height * .5f + 3f, position.width * .3f, position.height * .5f - 4f);
                    databaseName.stringValue = EditorGUI.TextField(databaseNameRect, databaseName.stringValue);
                    Rect dataRect = new Rect(position.x + position.width * .75f, position.y + position.height * .5f + 3f, position.width * .25f, position.height * .5f - 4f);
                    clipName.stringValue = EditorGUI.TextField(dataRect, clipName.stringValue);
                    break;
                default: break;
            }

            EditorGUI.indentLevel--;

            if (GUI.changed)
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 2f + 10f;
        }
    }
}
#endif