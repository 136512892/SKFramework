using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SK.Framework.UI
{
    public class UI : MonoBehaviour
    {
        private static UI instance;

        private Dictionary<string, IUIView> viewDic;

        internal static UI Instance
        {
            get
            {
                if (instance == null)
                {
                    UI res = Resources.Load<UI>("UI");
                    if (null == res)
                    {
                        Debug.LogError("加载UI预制体失败");
                    }
                    else
                    {
                        instance = Instantiate(res);
                        instance.name = "[SKFramework.UI]";
                        instance.viewDic = new Dictionary<string, IUIView>();

                        string[] levelNames = Enum.GetNames(typeof(ViewLevel));
                        for (int i = levelNames.Length - 1; i >= 0; i--)
                        {
                            string levelName = levelNames[i];
                            var levelInstance = new GameObject(levelName);
                            levelInstance.layer = LayerMask.NameToLayer("UI");
                            levelInstance.transform.SetParent(instance.transform, false);
                            RectTransform rectTransform = levelInstance.AddComponent<RectTransform>();
                            rectTransform.sizeDelta = instance.GetComponent<CanvasScaler>().referenceResolution;
                            rectTransform.anchorMin = Vector2.zero;
                            rectTransform.anchorMax = Vector2.one;
                            rectTransform.offsetMin = rectTransform.offsetMax = Vector2.zero;
                            rectTransform.SetAsFirstSibling();
                        }
                        DontDestroyOnLoad(instance);
                    }
                }
                return instance;
            }
        }

        public static Canvas Canvas
        {
            get
            {
                return Instance.GetComponent<Canvas>();
            }
        }

        public static Camera Camera
        {
            get
            {
                return Instance.GetComponentInChildren<Camera>();
            }
        }

        public static Vector2 Resolution
        {
            get
            {
                return Instance.GetComponent<CanvasScaler>().referenceResolution;
            }
        }

        /// <summary>
        /// 加载视图
        /// </summary>
        /// <param name="viewName">视图命名</param>
        /// <param name="viewResourcePath">视图资源路径</param>
        /// <param name="level">视图层级</param>
        /// <param name="view">视图</param>
        /// <param name="data">视图数据</param>
        /// <param name="instant">是否立即显示</param>
        /// <returns>成功加载返回true 否则返回false</returns>
        public bool LoadView(string viewName, string viewResourcePath, ViewLevel level, out IUIView view, IViewData data = null, bool instant = false)
        {
            if (!viewDic.TryGetValue(viewName, out view))
            {
                GameObject viewPrefab = Resources.Load<GameObject>(viewResourcePath);
                if (null != viewPrefab)
                {
                    var instance = Instantiate(viewPrefab);
                    instance.transform.SetParent(transform.GetChild((int)level), false);
                    instance.name = viewName;

                    view = instance.GetComponent<IUIView>();
                    view.Name = viewName;
                    view.Init(data, instant);

                    viewDic.Add(viewName, view);
                    return true;
                }
            }
            return false;
        }
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
        /// 卸载视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="instant">是否立即卸载</param>
        /// <returns>成功卸载返回true 否则返回false</returns>
        public bool UnloadView(string viewName, bool instant = false)
        {
            if (viewDic.TryGetValue(viewName, out IUIView view))
            {
                viewDic.Remove(viewName);
                view.Unload(instant);
                return true;
            }
            return false;
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
        /// 从字典中移除
        /// </summary>
        /// <param name="viewName">视图名称</param>
        public void Remove(string viewName)
        {
            if (viewDic.ContainsKey(viewName))
            {
                viewDic.Remove(viewName);
            }
        }
    }
}