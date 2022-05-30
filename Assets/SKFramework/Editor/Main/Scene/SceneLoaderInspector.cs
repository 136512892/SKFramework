using System;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using UnityEngine.SceneManagement;

namespace SK.Framework
{
    [CustomEditor(typeof(SceneLoader))]
    [CSDNUrl("https://coderz.blog.csdn.net/article/details/124877549?spm=1001.2014.3001.5502")]
    public class SceneLoaderInspector : AbstractEditor<SceneLoader>
    {
        protected override bool IsEnableBaseInspectorGUI
        {
            get
            {
                return true;
            }
        }

        private GetSceneMode getSceneMode;
        private LoadSceneMode loadSceneMode;
        private string sceneName;
        private int sceneBuildIndex;
        private float sceneActivationDelay;

        protected override void OnRuntimeEnable()
        {
            Type type = typeof(SceneLoader);
            getSceneMode = (GetSceneMode)type.GetField("getSceneMode", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Target);
            loadSceneMode = (LoadSceneMode)type.GetField("loadSceneMode", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Target);
            sceneName = type.GetField("sceneName", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Target) as string;
            sceneBuildIndex = (int)type.GetField("sceneBuildIndex", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Target);
            sceneActivationDelay = (float)type.GetField("sceneActivationDelay", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Target);

            EditorApplication.update += Repaint;
        }
        private void OnDisable()
        {
            EditorApplication.update -= Repaint;
        }

        protected override void OnRuntimeInspectorGUI()
        {
            GUILayout.Label(string.Format("Get Scene Mode - {0}", getSceneMode));
            switch (getSceneMode)
            {
                case GetSceneMode.Name: GUILayout.Label(string.Format("Scene Name - {0}", sceneName)); break;
                case GetSceneMode.BuildIndex: GUILayout.Label(string.Format("Scene Build Index - {0}", sceneBuildIndex)); break;
            }
            GUILayout.Label(string.Format("Scene Activation Delay - {0}", sceneActivationDelay));
            GUILayout.Label(string.Format("Load Scene Mode - {0}", loadSceneMode));
            GUILayout.Label(string.Format("Load Progress - {0}", Target.Progress));
        }
    }
}