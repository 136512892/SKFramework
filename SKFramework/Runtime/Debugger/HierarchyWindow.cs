/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace SK.Framework.Debugger
{
    [WindowTitle("Hierarchy")]
    public class HierarchyWindow : DebuggerWindow
    {
        private readonly Dictionary<string, List<GameObject>> m_SceneRootsCache = new Dictionary<string, List<GameObject>>();
        private readonly Dictionary<GameObject, bool> m_FoldoutDic = new Dictionary<GameObject, bool>();
        private GameObject m_SelectedObject;
        private Vector2 m_ScrollPosition;
        private bool m_IsListeningSceneEvents;
        private bool m_IsInit;
        private GUIStyle m_StyleNormal;
        private GUIStyle m_StyleSelect;

        public GameObject SelectedObject => m_SelectedObject;

        public override void OnInitialized()
        {
            base.OnInitialized();
            RefreshAllScenesCache();
            RegisterSceneEvents();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterSceneEvents();
            m_SceneRootsCache.Clear();
            m_FoldoutDic.Clear();
        }

        public override void OnGUI()
        {
            if (!m_IsInit)
            {
                m_IsInit = true;
                m_StyleNormal = new GUIStyle(GUI.skin.label);
                m_StyleSelect = new GUIStyle(GUI.skin.label);
                m_StyleSelect.normal.background = MakeTex(2, 2, new Color(.3f, .5f, 1f, .2f));
            }

            if (GUILayout.Button("Refresh Hierarchy", GUILayout.Height(30f)))
            {
                RefreshAllScenesCache();
            }
            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
            foreach (var sceneRoots in m_SceneRootsCache)
            {
                DrawSceneHeader(sceneRoots.Key);
                DrawSceneRoots(sceneRoots.Value);
            }
            GUILayout.EndScrollView();
        }

        private void DrawSceneHeader(string sceneName)
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.cyan;
            GUILayout.Label(sceneName, GUILayout.Height(25f));
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
        }

        private void DrawSceneRoots(List<GameObject> rootObjects)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.BeginVertical();
            foreach (var root in rootObjects)
            {
                if (root == null)
                    continue;
                DrawGameObjectHierarchy(root);
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void DrawGameObjectHierarchy(GameObject go)
        {
            if (go == null)
                return;

            if (!m_FoldoutDic.ContainsKey(go))
                m_FoldoutDic[go] = false;

            GUILayout.BeginHorizontal();
            if (go.transform.childCount > 0)
            {
                m_FoldoutDic[go] = GUILayout.Toggle(
                    m_FoldoutDic[go],
                    m_FoldoutDic[go] ? "▼" : "►",
                    GUILayout.Width(25f)
                );
            }
            else
            {
                GUILayout.Space(25f);
            }

            var isSelected = m_SelectedObject == go;
            GUI.contentColor = isSelected ? Color.blue : Color.white;
            if (GUILayout.Button(go.name, isSelected ? m_StyleSelect : m_StyleNormal))
            {
                m_SelectedObject = go;
            }
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();

            if (m_FoldoutDic[go] && go.transform.childCount > 0)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(25f);
                GUILayout.BeginVertical();
                foreach (Transform child in go.transform)
                {
                    DrawGameObjectHierarchy(child.gameObject);
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }

        private void RefreshAllScenesCache()
        {
            var destroyedObjects = new List<GameObject>();
            foreach (var item in m_FoldoutDic)
            {
                if (item.Key == null)
                    destroyedObjects.Add(item.Key);
            }
            foreach (var obj in destroyedObjects)
            {
                m_FoldoutDic.Remove(obj);
            }
            m_SceneRootsCache.Clear();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded)
                    continue;
                var rootObjects = new List<GameObject>();
                scene.GetRootGameObjects(rootObjects);
                rootObjects.RemoveAll(go => go == null);
                var key = $"[Scene] {scene.name}";
                m_SceneRootsCache[key] = rootObjects;
            }
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private void RegisterSceneEvents()
        {
            if (m_IsListeningSceneEvents)
                return;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            m_IsListeningSceneEvents = true;
        }

        private void UnregisterSceneEvents()
        {
            if (!m_IsListeningSceneEvents)
                return;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            m_IsListeningSceneEvents = false;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            RefreshAllScenesCache();
        }

        private void OnSceneUnloaded(Scene scene)
        {
            RefreshAllScenesCache();
        }
    }
}