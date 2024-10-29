/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace SK.Framework.UI
{
    public class UI : ModuleBase
    {
        private readonly Dictionary<ViewLevel, Transform> m_LevelDic = new Dictionary<ViewLevel, Transform>();
        private readonly Dictionary<string, IUIView> m_ViewDic = new Dictionary<string, IUIView>();

        public Vector2 resolution { get; private set; }
        
        public override void OnInitialization()
        {
            base.OnInitialization();
            resolution = GetComponent<CanvasScaler>().referenceResolution;
            string[] levels = Enum.GetNames(typeof(ViewLevel));
            for (int i = levels.Length - 1; i >= 0; i--)
            {
                var level = new GameObject(levels[i]);
                Canvas canvas = level.AddComponent<Canvas>();
                level.transform.SetParent(transform, false);
                level.layer = LayerMask.NameToLayer("UI");
                ViewLevel viewLevel = (ViewLevel)Enum.Parse(typeof(ViewLevel), levels[i]);
                m_LevelDic.Add(viewLevel, level.transform);
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = (int)viewLevel;

                CanvasScaler canvasScaler = level.AddComponent<CanvasScaler>();
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScaler.referenceResolution = resolution;
                level.AddComponent<GraphicRaycaster>();

                RectTransform rt = level.GetComponent<RectTransform>();
                rt.sizeDelta = resolution;
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = rt.offsetMax = Vector2.zero;
                rt.SetAsFirstSibling();
            }
        }

        public T LoadView<T>(string viewName, string resourcesPath, ViewLevel level = ViewLevel.COMMON, object data = null) where T : MonoBehaviour, IUIView
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
                    return view as T;
                }
            }
            return view as T;
        }
        public T LoadView<T>(string resourcesPath, ViewLevel level = ViewLevel.COMMON, object data = null) where T : MonoBehaviour, IUIView
        {
            return LoadView<T>(typeof(T).Name, resourcesPath, level, data);
        }
        public T LoadView<T>(ViewLevel level = ViewLevel.COMMON, object data = null) where T : MonoBehaviour, IUIView
        {
            return LoadView<T>(typeof(T).Name, typeof(T).Name, level, data);
        }

        public void LoadViewAsync<T>(string viewAssetPath, ViewLevel level = ViewLevel.COMMON,
            object data = null, Action<float> onLoading = null, Action<bool, T> callback = null) where T : MonoBehaviour, IUIView
        {
            SKFramework.Module<Resource.Resource>().LoadAssetAsync<GameObject>(viewAssetPath, onLoading, (success, obj) =>
            {
                if (success)
                {
                    T t = Instantiate(obj, m_LevelDic[level], false).GetComponent<T>();
                    t.name = typeof(T).Name;
                    t.viewName = typeof(T).Name;
                    t.OnLoad(data);
                    m_ViewDic.Add(t.viewName, t);
                    callback?.Invoke(true, t);
                }
                else
                {
                    callback?.Invoke(false, null);
                }
            });
        }

        public T OpenView<T>(string viewName, string resourcesPath, ViewLevel level = ViewLevel.COMMON, object data = null) where T : MonoBehaviour, IUIView
        {
            if (!m_ViewDic.TryGetValue(viewName, out IUIView view))
                view = LoadView<T>(viewName, resourcesPath, level, data);
            if (view != null)
            {
                view.OnOpen(data);
                return view as T;
            }
            return null;
        }
        public T OpenView<T>(string resourcesPath, ViewLevel level = ViewLevel.COMMON, object data = null) where T : MonoBehaviour, IUIView
        {
            return OpenView<T>(typeof(T).Name, resourcesPath, level, data);
        }
        public T OpenView<T>(ViewLevel level = ViewLevel.COMMON, object data = null) where T : MonoBehaviour, IUIView
        {
            return OpenView<T>(typeof(T).Name, typeof(T).Name, level, data);
        }
        
        public bool CloseView(string viewName)
        {
            if (m_ViewDic.TryGetValue(viewName, out IUIView view))
            {
                view.OnClose();
                return true;
            }
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
            return m_ViewDic.ContainsKey(viewName)
                ? m_ViewDic[viewName] as T
                : default;
        }
        public T GetView<T>() where T : MonoBehaviour, IUIView
        {
            return GetView<T>(typeof(T).Name);
        }

        public bool UnloadView(string viewName)
        {
            if (m_ViewDic.TryGetValue(viewName, out IUIView view))
            {
                view.OnUnload();
                Destroy((view as MonoBehaviour).gameObject);
                m_ViewDic.Remove(viewName);
                return true;
            }
            return false;
        }
        public bool UnloadView<T>() where T : MonoBehaviour, IUIView
        {
            return UnloadView(typeof(T).Name);
        }

        public void CloseAll(bool unload)
        {
            foreach (var view in m_ViewDic.Values)
            {
                view.OnClose();
                if (unload)
                {
                    view.OnUnload();
                    Destroy((view as MonoBehaviour).gameObject);
                }
            }
            if (unload)
                m_ViewDic.Clear();
        }
    }
}