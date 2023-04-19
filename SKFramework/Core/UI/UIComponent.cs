using System;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.UI
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/UI")]
    public class UIComponent : MonoBehaviour
    {
        [SerializeField] private Vector2 resolution = new Vector2(1920f, 1080f);

        private Dictionary<string, IUIView> viewDic;

        private void Awake()
        {
            viewDic = new Dictionary<string, IUIView>();

            string[] levelNames = Enum.GetNames(typeof(ViewLevel));
            for (int i = levelNames.Length - 1; i >= 0; i--)
            {
                string levelName = levelNames[i];
                var levelInstance = new GameObject(levelName);
                levelInstance.transform.SetParent(transform, false);
                levelInstance.layer = LayerMask.NameToLayer("UI");
                RectTransform rectTransform = levelInstance.AddComponent<RectTransform>();
                rectTransform.sizeDelta = resolution;
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.offsetMin = rectTransform.offsetMax = Vector2.zero;
                rectTransform.SetAsFirstSibling();
            }
        }

        #region >> Resources模式加载视图
        /// <summary>
        /// 加载视图
        /// </summary>
        /// <param name="viewName">视图命名</param>
        /// <param name="viewResourcePath">视图资源路径</param>
        /// <param name="level">视图层级</param>
        /// <param name="view">视图</param>
        /// <param name="data">视图数据</param>
        /// <param name="instant">是否立即显示</param>
        /// <returns>0：加载成功  -1：视图已存在，无需重复加载  -2：加载失败，请检查资源路径</returns>
        public int LoadView(string viewName, string viewResourcePath, ViewLevel level, out IUIView view, IViewData data = null, bool instant = false)
        {
            if (!viewDic.TryGetValue(viewName, out view))
            {
                GameObject viewPrefab = Resources.Load<GameObject>(viewResourcePath);
                if (viewPrefab == null) return -2;
                else
                {
                    var instance = Instantiate(viewPrefab);
                    instance.transform.SetParent(transform.GetChild((int)level), false);
                    instance.name = viewName;

                    view = instance.GetComponent<IUIView>();
                    view.Name = viewName;
                    view.Init(data, instant);

                    viewDic.Add(viewName, view);
                    return 0;
                }
            }
            return -1;
        }
        /// <summary>
        /// 加载视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图命名</param>
        /// <param name="viewResourcePath">视图资源路径</param>
        /// <param name="level">视图层级</param>
        /// <param name="data">视图数据</param>
        /// <param name="instant">是否立即显示</param>
        /// <returns>加载成功返回视图，否则返回null</returns>
        public T LoadView<T>(string viewName, string viewResourcePath, ViewLevel level = ViewLevel.COMMON, IViewData data = null, bool instant = false) where T : UIView
        {
            if (LoadView(viewName, viewResourcePath, level, out IUIView view, data, instant) == 0)
            {
                return view as T;
            }
            return null;
        }
        /// <summary>
        /// 加载视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="level">视图层级</param>
        /// <param name="data">视图数据</param>
        /// <param name="instant">是否立即显示</param>
        /// <returns>加载成功返回视图，否则返回null</returns>
        public T LoadView<T>(ViewLevel level = ViewLevel.COMMON, IViewData data = null, bool instant = false) where T : UIView
        {
            string viewName = typeof(T).Name;
            if (LoadView(viewName, viewName, level, out IUIView view, data, instant) == 0)
            {
                return view as T;
            }
            return null;
        }
        #endregion

        #region >> AssetBundle模式加载视图
        /// <summary>
        /// 异步加载视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图命名</param>
        /// <param name="assetPath">视图资源路径</param>
        /// <param name="level">视图层级</param>
        /// <param name="data">视图数据</param>
        /// <param name="instant">是否立即显示</param>
        /// <param name="onLoading">加载中事件</param>
        /// <param name="onCompleted">加载完成事件</param>
        public void LoadViewAsync<T>(string viewName, string assetPath, ViewLevel level = ViewLevel.COMMON, IViewData data = null, bool instant = false, Action<float> onLoading = null, Action<bool, T> onCompleted = null) where T : UIView
        {
            if (!viewDic.ContainsKey(viewName))
            {
                Main.Resource.LoadAssetAsync<GameObject>(assetPath, onLoading, (success, obj) =>
                {
                    if (success)
                    {
                        var instance = Instantiate(obj);
                        instance.transform.SetParent(transform.GetChild((int)level), false);
                        instance.name = viewName;

                        T view = instance.GetComponent<T>();
                        view.Name = viewName;
                        view.Init(data, instant);
                        viewDic.Add(viewName, view);

                        onCompleted?.Invoke(true, view);
                    }
                    else
                    {
                        onCompleted?.Invoke(false, null);
                    }
                });
            }
            else
            {
                Main.Log.Warning("视图{0}已加载", viewName);
            }
        }
        /// <summary>
        /// 异步加载视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="assetPath">视图资源路径</param>
        /// <param name="level">视图层级</param>
        /// <param name="data">视图数据</param>
        /// <param name="instant">是否立即显示</param>
        /// <param name="onLoading">加载中事件</param>
        /// <param name="onCompleted">加载完成事件</param>
        public void LoadViewAsync<T>(string assetPath, ViewLevel level = ViewLevel.COMMON, IViewData data = null, bool instant = false, Action<float> onLoading = null, Action<bool, T> onCompleted = null) where T : UIView
        {
            LoadViewAsync(typeof(T).Name, assetPath, level, data, instant, onLoading, onCompleted);
        }
        #endregion

        /// <summary>
        /// 显示视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="data">视图数据</param>
        /// <param name="instant">是否立即显示</param>
        /// <returns>视图</returns>
        public IUIView ShowView(string viewName, IViewData data = null, bool instant = false)
        {
            if (viewDic.TryGetValue(viewName, out IUIView view))
            {
                view.Show(data, instant);
                return view;
            }
            return null;
        }
        /// <summary>
        /// 显示视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="data">视图数据</param>
        /// <param name="instant">是否立即显示</param>
        /// <returns>视图</returns>
        public T ShowView<T>(IViewData data = null, bool instant = false) where T : UIView
        {
            IUIView view = ShowView(typeof(T).Name, data, instant);
            return view != null ? view as T : null;
        }

        /// <summary>
        /// 隐藏视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="instant">是否立即隐藏</param>
        /// <returns>视图</returns>
        public IUIView HideView(string viewName, bool instant = false)
        {
            if (viewDic.TryGetValue(viewName, out IUIView view))
            {
                view.Hide(instant);
                return view;
            }
            return null;
        }
        /// <summary>
        /// 隐藏视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="instant">是否立即隐藏</param>
        /// <returns>视图</returns>
        public T HideView<T>(bool instant = false) where T : UIView
        {
            IUIView view = HideView(typeof(T).Name, instant);
            return view != null ? view as T : null;
        }

        /// <summary>
        /// 卸载视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="instant">是否立即卸载</param>
        /// <returns>成功卸载返回true 否则返回false</returns>
        public bool UnloadView(string viewName, bool instant = false)
        {
            if (viewDic.TryGetValue(viewName, out IUIView view))
            {
                view.Unload(instant);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 卸载视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="instant">是否立即卸载</param>
        /// <returns>成功卸载返回true 否则返回false</returns>
        public bool UnloadView<T>(bool instant = false) where T : UIView
        {
            return UnloadView(typeof(T).Name, instant);
        }
        /// <summary>
        /// 卸载所有视图
        /// </summary>
        public void UnloadAll()
        {
            List<IUIView> views = new List<IUIView>();
            foreach (var kv in viewDic)
            {
                views.Add(kv.Value);
            }
            for (int i = 0; i < views.Count; i++)
            {
                views[i].Unload(true);
                views.RemoveAt(i);
                i--;
            }
            viewDic.Clear();
        }

        /// <summary>
        /// 获取视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <returns>视图</returns>
        public IUIView GetView(string viewName)
        {
            viewDic.TryGetValue(viewName, out IUIView view);
            return view;
        }
        /// <summary>
        /// 获取视图 
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <returns>视图</returns>
        public T GetView<T>() where T : UIView
        {
            IUIView view = GetView(typeof(T).Name);
            return view != null ? view as T : null;
        }
        /// <summary>
        /// 获取或加载视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <returns>视图</returns>
        public T GetOrLoadView<T>() where T : UIView
        {
            T view = GetView<T>() ?? LoadView<T>();
            return view;
        }

        /// <summary>
        /// 从字典中移除
        /// </summary>
        /// <param name="viewName">视图名称</param>
        internal void Remove(string viewName)
        {
            if (viewDic.ContainsKey(viewName))
            {
                viewDic.Remove(viewName);
            }
        }
    }
}