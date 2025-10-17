/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using SK.Framework.Logger;
using UnityEditor;
using ILogger = SK.Framework.Logger.ILogger;

namespace SK.Framework.UI
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/SKFramework.UI")]
    public class UI : ModuleBase
    {
        [SerializeField] private Vector2 m_Resolotion = new Vector2(1920f, 1080f);
        [SerializeField] private RenderMode m_CanvasRenderMode = RenderMode.ScreenSpaceOverlay;

        private readonly Dictionary<ViewLevel, Transform> m_LevelDic = new Dictionary<ViewLevel, Transform>();
        private readonly Dictionary<string, IUIView> m_ViewDic = new Dictionary<string, IUIView>();
        private readonly List<IUIView> m_ViewListCache = new List<IUIView>();
        private ILogger m_Logger;
        
        public Vector2 resolution => m_Resolotion;

        protected internal override void OnInitialization()
        {
            base.OnInitialization();
            m_Logger = SKFramework.Module<Log>().GetLogger<ModuleLogger>();

            string[] levels = Enum.GetNames(typeof(ViewLevel));
            for (int i = levels.Length - 1; i >= 0; i--)
            {
                var level = new GameObject(levels[i]);
                Canvas canvas = level.AddComponent<Canvas>();
                canvas.renderMode = m_CanvasRenderMode;
                level.transform.SetParent(transform, false);
                level.transform.SetAsFirstSibling();
                level.layer = LayerMask.NameToLayer("UI");
                ViewLevel viewLevel = (ViewLevel)Enum.Parse(typeof(ViewLevel), levels[i]);
                canvas.sortingOrder = (int)viewLevel;
                m_LevelDic.Add(viewLevel, level.transform);

                CanvasScaler canvasScaler = level.AddComponent<CanvasScaler>();
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScaler.referenceResolution = m_Resolotion;
                
                level.AddComponent<GraphicRaycaster>();
            }

            if (m_CanvasRenderMode == RenderMode.ScreenSpaceCamera)
            {
                var camera = new GameObject("UI Camera").AddComponent<Camera>();
                camera.transform.SetParent(transform);
                camera.cullingMask = 1 << LayerMask.NameToLayer("UI");
                camera.orthographic = true;
                foreach(var level in m_LevelDic.Values)
                {
                    level.GetComponent<Canvas>().worldCamera = camera;
                }
            }
        }

        protected internal override void OnTermination()
        {
            base.OnTermination();
            m_LevelDic.Clear();
            m_ViewDic.Clear();
        }

        private void Update()
        {
            m_ViewListCache.Clear();
            m_ViewListCache.AddRange(m_ViewDic.Values);
            for (int i = 0; i < m_ViewListCache.Count; i++)
            {
                m_ViewListCache[i].OnUpdate();
            }
        }

        public T LoadView<T>(string viewName, string resourcesPath, ViewLevel level = ViewLevel.COMMON, 
            object data = null) where T : MonoBehaviour, IUIView
        {
            if (!m_ViewDic.TryGetValue(viewName, out IUIView view))
            {
                GameObject viewPrefab = Resources.Load<GameObject>(resourcesPath);
                if (viewPrefab != null)
                {
                    GameObject instance = Instantiate(viewPrefab, m_LevelDic[level], false);
                    instance.name = viewName;
                    view = instance.GetComponent<IUIView>();
                    view.viewName = viewName;
                    view.OnLoad(data);
                    m_ViewDic.Add(viewName, view);
                    m_Logger.Info("[UI] Load view {0}", viewName);
                    return view as T;
                }
                else
                {
                    m_Logger.Error("[UI] Failed to load view asset from the Resources folder, please check the file exists at {0}.", resourcesPath);
                    return null;
                }
            }
            m_Logger.Warning("[UI] A view with name {0} already exists.", viewName);
            return view as T;
        }

        public T LoadView<T>(string resourcesPath, ViewLevel level = ViewLevel.COMMON, 
            object data = null) where T : MonoBehaviour, IUIView
        {
            return LoadView<T>(typeof(T).Name, resourcesPath, level, data);
        }

        public T LoadView<T>(ViewLevel level = ViewLevel.COMMON, object data = null) where T : MonoBehaviour, IUIView
        {
            Type type = typeof(T);
            return LoadView<T>(type.Name, type.Name, level, data);
        }

        public void LoadViewAsync<T>(string viewName, string assetPath, ViewLevel level = ViewLevel.COMMON, object data = null, 
            Action<T> onCompleted = null) where T : MonoBehaviour, IUIView
        {
            if (!m_ViewDic.TryGetValue(assetPath, out IUIView view))
            {
                SKFramework.Module<Resource.Resource>().LoadAssetAsync<GameObject>(assetPath, (success, obj) =>
                {
                    if (success)
                    {
                        GameObject instance = Instantiate(obj, m_LevelDic[level], false);
                        instance.name = viewName;
                        view = instance.GetComponent<IUIView>();
                        view.viewName = viewName;
                        view.OnLoad(data);
                        m_ViewDic.Add(viewName, view);
                        m_Logger.Info("[UI] Load view {0} from path: {1}", viewName, assetPath);
                        onCompleted?.Invoke(view as T);
                    }
                    else
                    {
                        onCompleted?.Invoke(null);
                        m_Logger.Error("[UI] Load view {0} from path {1} failed.", viewName, assetPath);
                    }
                });
            }
            else
            {
                m_Logger.Warning("[UI] A view with name {0} already exists.", viewName);
            }
        }

        public void LoadViewAsync<T>(string assetPath, ViewLevel level = ViewLevel.COMMON, object data = null, 
            Action<T> onCompleted = null) where T : MonoBehaviour, IUIView
        {
            LoadViewAsync(typeof(T).Name, assetPath, level, data, onCompleted);
        }

        public T OpenView<T>(string viewName, ViewLevel level = ViewLevel.COMMON,
            object data = null) where T : MonoBehaviour, IUIView
        {
            if (m_ViewDic.TryGetValue(viewName, out IUIView view) && view is MonoBehaviour mono)
            {
                var instance = mono.gameObject;
                instance.SetActive(true);
                if (instance.transform.parent != m_LevelDic[level])
                    instance.transform.SetParent(m_LevelDic[level], false);
                instance.transform.SetAsLastSibling();
                view.OnOpen(data);
                m_Logger.Info("[UI] Open view {0}", viewName);
                return view as T;
            }
            m_Logger.Warning("[UI] A view with name {0} does not exists.", viewName);
            return null;
        }

        public T OpenView<T>(ViewLevel level = ViewLevel.COMMON, object data = null) where T : MonoBehaviour, IUIView
        {
            return OpenView<T>(typeof(T).Name, level, data);
        }

        public bool CloseView(string viewName)
        {
            if (m_ViewDic.TryGetValue(viewName, out IUIView view) && view is MonoBehaviour mono)
            {
                var instance = mono.gameObject;
                instance.SetActive(false);
                view.OnClose();
                m_Logger.Info("[UI] Close view {0}", viewName);
                return true;
            }
            m_Logger.Warning("[UI] A view with name {0} does not exists.", viewName);
            return false;
        }

        public bool CloseView<T>() where T : MonoBehaviour, IUIView
        {
            return CloseView(typeof(T).Name);
        }

        public bool HasView(string viewName)
        {
            return m_ViewDic.ContainsKey(viewName);
        }

        public bool HasView<T>() where T : MonoBehaviour, IUIView
        {
            return HasView(typeof(T).Name);
        }

        public T GetView<T>(string viewName) where T : MonoBehaviour, IUIView
        {
            return m_ViewDic.TryGetValue(viewName, out IUIView view) ? view as T : null;
        }

        public T GetView<T>() where T : MonoBehaviour, IUIView
        {
            return GetView<T>(typeof(T).Name);
        }

        public bool TryGetView<T>(string viewName, out T view) where T : MonoBehaviour, IUIView
        {
            if (m_ViewDic.TryGetValue(viewName, out IUIView target))
            {
                view = target as T;
                return true;
            }
            view = null;
            return false;
        }

        public bool TryGetView<T>(out T view) where T : MonoBehaviour, IUIView
        {
            return TryGetView<T>(typeof(T).Name, out view);
        }

        public bool UnloadView(string viewName)
        {
            if (m_ViewDic.TryGetValue(viewName, out IUIView view) && view is MonoBehaviour mono)
            {
                var instance = mono.gameObject;
                if (instance.activeSelf)
                    view.OnClose();
                view.OnUnload();
                Destroy(instance);
                m_ViewDic.Remove(viewName);
                m_Logger.Info("[UI] Unload view {0}", viewName);
                return true;
            }
            m_Logger.Warning("[UI] A view with name {0} does not exists.", viewName);
            return false;
        }

        public bool UnloadView<T>() where T : MonoBehaviour, IUIView
        {
            return UnloadView(typeof(T).Name);
        }

        public void CloseAll()
        {
            foreach (var view in m_ViewDic.Values)
            {
                CloseView(view.viewName);
            }
        }

        public void UnloadAll()
        {
            foreach (var view in m_ViewDic.Values)
            {
                UnloadView(view.viewName);
            }
            m_ViewDic.Clear();
        }
    }
}