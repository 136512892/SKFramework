#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace SK.Framework.Audios
{
    [CustomPropertyDrawer(typeof(Sound))]
    public class SoundPropertyDrawer : PropertyDrawer
    {
        private AudioDatabase[] databases;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect labelRect = new Rect(position.x, position.y, position.width, position.height * .5f);
            EditorGUI.LabelField(labelRect, label);

            EditorGUI.indentLevel++;

            Rect sourceLabelRect = new Rect(position.x, position.y + position.height * .5f, position.width * .15f, position.height * .5f);
            EditorGUI.LabelField(sourceLabelRect, "From");

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
                    if (databases == null)
                    {
                        string[] guids = AssetDatabase.FindAssets("t:AudioDatabase");
                        databases = new AudioDatabase[guids.Length];
                        for (int i = 0; i < guids.Length; i++)
                        {
                            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                            databases[i] = AssetDatabase.LoadAssetAtPath<AudioDatabase>(path);
                        }
                    }
                    var databaseName = property.FindPropertyRelative("databaseName");
                    var audioDataName = property.FindPropertyRelative("audioDataName");
                    string[] databaseNames = databases.Select(m => m.databaseName).ToArray();
                    int index = Array.FindIndex(databaseNames, m => m == databaseName.stringValue);
                    Rect databaseNameRect = new Rect(position.x + position.width * .45f, position.y + position.height * .5f + 3f, position.width * .3f, position.height * .5f - 4f);
                    int newIndex = EditorGUI.Popup(databaseNameRect, index, databaseNames);
                    if (index != newIndex)
                    {
                        databaseName.stringValue = databaseNames[newIndex];
                    }

                    Rect dataRect = new Rect(position.x + position.width * .75f, position.y + position.height * .5f + 3f, position.width * .25f, position.height * .5f - 4f);
                    if (index != -1)
                    {
                        AudioData[] audioDatas = databases[newIndex].datasets.ToArray();
                        string[] audioDataNames = audioDatas.Select(m => m.name).ToArray();
                        int dataIndex = Array.FindIndex(audioDataNames, m => m == audioDataName.stringValue);
                        int newDataIndex = EditorGUI.Popup(dataRect, dataIndex, audioDataNames);
                        if (newDataIndex != dataIndex)
                        {
                            audioDataName.stringValue = audioDataNames[newDataIndex];
                        }
                    }
                    else
                    {
                        string[] audioDataNames = new string[0];
                        EditorGUI.Popup(dataRect, -1, audioDataNames);
                    }
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