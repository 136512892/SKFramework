using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SK.Framework.UI
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/UI")]
    public class UIComponent : MonoBehaviour, IUIComponent
    {
        //字典存储已经加载的视图
        private readonly Dictionary<string, IUIView> viewDic = new Dictionary<string, IUIView>();

        public Canvas Canvas { get; private set; }

        public Vector2 Resolution { get; private set; }

        private void Awake()
        {
            Canvas = GetComponent<Canvas>();
            Resolution = GetComponent<CanvasScaler>().referenceResolution;

            string[] levelNames = Enum.GetNames(typeof(ViewLevel));
            for (int i = levelNames.Length - 1; i >= 0; i--)
            {
                string levelName = levelNames[i];
                var levelInstance = new GameObject(levelName);
                levelInstance.transform.SetParent(transform, false);
                levelInstance.layer = LayerMask.NameToLayer("UI");
                RectTransform rectTransform = levelInstance.AddComponent<RectTransform>();
                rectTransform.sizeDelta = Resolution;
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.offsetMin = rectTransform.offsetMax = Vector2.zero;
                rectTransform.SetAsFirstSibling();
            }
        }

        /// <summary>
        /// 加载视图（Resources加载方式）
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图命名</param>
        /// <param name="resourcesPath">Resources资源路径</param>
        /// <param name="viewLevel">视图层级</param>
        /// <param name="data">视图数据</param>
        /// <returns>视图</returns>
        public T LoadView<T>(string viewName, string resourcesPath, ViewLevel viewLevel = ViewLevel.COMMON, object data = null) where T : MonoBehaviour, IUIView
        {
            //该视图尚未加载
            if (!viewDic.ContainsKey(viewName))
            {
                //加载视图资产
                GameObject viewPrefab = Resources.Load<GameObject>(resourcesPath);
                if (viewPrefab != null)
                {
                    //实例化
                    GameObject instance = Instantiate(viewPrefab);
                    //设置层级
                    instance.transform.SetParent(transform.GetChild((int)viewLevel), false);
                    //设置名称
                    instance.name = viewName;
                    //获取视图组件
                    IUIView view = instance.GetComponent<IUIView>();
                    //为视图命名
                    view.Name = viewName;
                    //调用其初始化事件
                    view.OnInit(data);
                    //添加到视图字典
                    viewDic.Add(viewName, view);
                    return view as T;
                }
            }
            return null;
        }
        public T LoadView<T>(string resourcesPath, ViewLevel viewLevel = ViewLevel.COMMON, object data = null) where T : MonoBehaviour, IUIView
        {
            return LoadView<T>(typeof(T).Name, resourcesPath, viewLevel, data);
        }
        public T LoadView<T>(ViewLevel viewLevel = ViewLevel.COMMON, object data = null) where T: MonoBehaviour, IUIView
        {
            return LoadView<T>(typeof(T).Name, typeof(T).Name, viewLevel, data);
        }

        /// <summary>
        /// 异步加载视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图命名</param>
        /// <param name="assetPath">资产路径</param>
        /// <param name="viewLevel">视图层级</param>
        /// <param name="data">视图数据</param>
        /// <param name="onCompleted">加载的回调事件</param>
        public void LoadViewAsync<T>(string viewName, string assetPath, ViewLevel viewLevel = ViewLevel.COMMON, object data = null, Action<bool, T> onCompleted = null) where T : MonoBehaviour, IUIView
        {
            //该视图尚未加载
            if (!viewDic.ContainsKey(viewName))
            {
                //异步加载资产
                Main.Resource.LoadAssetAsync<GameObject>(assetPath, onCompleted: (success, obj) =>
                {
                    //资产加载成功
                    if (success)
                    {
                        //实例化
                        GameObject instance = Instantiate(obj);
                        //设置层级
                        instance.transform.SetParent(transform.GetChild((int)viewLevel), false);
                        //设置名称
                        instance.name = viewName;
                        //获取视图组件
                        IUIView view = instance.GetComponent<IUIView>();
                        //为视图命名
                        view.Name = viewName;
                        //调用其初始化事件
                        view.OnInit(data);
                        //添加到视图字典
                        viewDic.Add(viewName, view);
                        //执行回调
                        onCompleted?.Invoke(true, view as T);
                    }
                    //资产加载失败
                    else
                    {
                        //执行回调
                        onCompleted?.Invoke(false, null);
                    }
                });
            }
        }
        public void LoadViewAsync<T>(string assetPath, ViewLevel viewLevel = ViewLevel.COMMON, object data = null, Action<bool, T> onCompleted = null) where T : MonoBehaviour, IUIView
        {
            LoadViewAsync(typeof(T).Name, assetPath, viewLevel, data, onCompleted);
        }

        /// <summary>
        /// 打开视图（如果视图尚未加载，会通过Resources方式进行加载）
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图命名或视图名称</param>
        /// <param name="resourcesPath">Resources资源路径</param>
        /// <param name="viewLevel">视图层级</param>
        /// <param name="instant">是否立即显示</param>
        /// <param name="data">视图数据</param>
        /// <returns>视图</returns>
        public T OpenView<T>(string viewName, string resourcesPath, ViewLevel viewLevel = ViewLevel.COMMON, bool instant = false, object data = null) where T : MonoBehaviour, IUIView
        {
            //打开视图时 首先判断该视图是否已加载
            if (!viewDic.TryGetValue(viewName, out IUIView view))
            {
                //加载视图资产
                GameObject viewPrefab = Resources.Load<GameObject>(resourcesPath);
                if (viewPrefab != null)
                {
                    //实例化
                    GameObject instance = Instantiate(viewPrefab);
                    //设置层级
                    instance.transform.SetParent(transform.GetChild((int)viewLevel), false);
                    //设置名称
                    instance.name = viewName;
                    //获取视图组件
                    view = instance.GetComponent<IUIView>();
                    //为视图命名
                    view.Name = viewName;
                    //调用其初始化事件
                    view.OnInit(data);
                    //添加到视图字典
                    viewDic.Add(viewName, view);
                }
            }
            //从字典获取到或者已经执行过加载后 判断非空
            if (view != null)
            {
                //执行视图的打开事件
                view.OnOpen(instant, data);
                return view as T;
            }
            return null;
        }
        public T OpenView<T>(string resourcesPath, ViewLevel viewLevel = ViewLevel.COMMON, bool instant = false, object data = null) where T : MonoBehaviour, IUIView
        {
            return OpenView<T>(typeof(T).Name, resourcesPath, viewLevel, instant, data);
        }
        public T OpenView<T>(ViewLevel viewLevel = ViewLevel.COMMON, bool instant = false, object data = null) where T : MonoBehaviour, IUIView
        {
            return OpenView<T>(typeof(T).Name, typeof(T).Name, viewLevel, instant, data);
        }

        /// <summary>
        /// 异步打开视图（如果视图尚未加载，会通过异步方式进行加载）
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图命名或视图名称</param>
        /// <param name="assetPath">资产路径</param>
        /// <param name="viewLevel">视图层级</param>
        /// <param name="instant">是否立即显示</param>
        /// <param name="data">视图数据</param>
        /// <param name="onCompleted">加载的回调事件 如果未执行加载过程不执行</param>
        public void OpenViewAsync<T>(string viewName, string assetPath, ViewLevel viewLevel = ViewLevel.COMMON, bool instant = false, object data = null, Action<bool, T> onCompleted = null) where T : MonoBehaviour, IUIView
        {
            //该视图尚未加载
            if (!viewDic.TryGetValue(viewName, out IUIView view))
            {
                //异步加载资产
                Main.Resource.LoadAssetAsync<GameObject>(assetPath, onCompleted: (success, obj) =>
                {
                    //资产加载成功
                    if (success)
                    {
                        //实例化
                        GameObject instance = Instantiate(obj);
                        //设置层级
                        instance.transform.SetParent(transform.GetChild((int)viewLevel), false);
                        //设置名称
                        instance.name = viewName;
                        //获取视图组件
                        view = instance.GetComponent<IUIView>();
                        //为视图命名
                        view.Name = viewName;
                        //调用其初始化事件
                        view.OnInit(data);
                        //添加到视图字典
                        viewDic.Add(viewName, view);
                        //执行回调
                        onCompleted?.Invoke(true, view as T);

                        //执行视图的打开事件
                        view.OnOpen(instant, data);
                    }
                    //资产加载失败
                    else
                    {
                        //执行回调
                        onCompleted?.Invoke(false, null);
                    }
                });
            }
            else
            {
                //执行视图的打开事件
                view.OnOpen(instant, data);
            }
        }
        public void OpenViewAsync<T>(string assetPath, ViewLevel viewLevel = ViewLevel.COMMON, bool instant = false, object data = null, Action<bool, T> onCompleted = null) where T : MonoBehaviour, IUIView
        {
            OpenViewAsync(typeof(T).Name, assetPath, viewLevel, instant, data, onCompleted);
        }

        /// <summary>
        /// 尝试打开视图（如果视图已经加载，将其打开）
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图名称</param>
        /// <param name="instant">是否立即显示</param>
        /// <param name="data">视图数据</param>
        /// <returns>视图</returns>
        public T TryOpenView<T>(string viewName, bool instant = false, object data = null) where T : MonoBehaviour, IUIView
        {
            if (viewDic.TryGetValue(viewName, out IUIView view))
            {
                //执行视图的打开事件
                view.OnOpen(instant, data);
                return view as T;
            }
            return null;
        }
        public T TryOpenView<T>(bool instant = false, object data = null) where T : MonoBehaviour, IUIView
        {
            return TryOpenView<T>(typeof(T).Name, instant, data);
        }

        /// <summary>
        /// 关闭视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="instant">是否立即关闭</param>
        /// <returns></returns>
        public bool CloseView(string viewName, bool instant = false)
        {
            //从字典中获取视图
            if (viewDic.TryGetValue(viewName, out IUIView view))
            {
                //调用其关闭事件
                view.OnClose(instant);
                return true;
            }
            return false;
        }
        public bool CloseView<T>(bool instant = false) where T : IUIView
        {
            return CloseView(typeof(T).Name, instant);
        }

        /// <summary>
        /// 是否存在视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <returns>true：存在  false：不存在</returns>
        public bool HasView(string viewName)
        {
            return viewDic.ContainsKey(viewName);
        } 
        public bool HasView<T>() where T : MonoBehaviour, IUIView
        {
            return HasView(typeof(T).Name);
        }

        /// <summary>
        /// 获取视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图名称</param>
        /// <returns>视图</returns>
        public T GetView<T>(string viewName) where T : IUIView
        {
            if (viewDic.ContainsKey(viewName))
            {
                return (T)viewDic[viewName];
            }
            return default;
        }
        public T GetView<T>() where T : MonoBehaviour, IUIView
        {
            return GetView<T>(typeof(T).Name);
        }

        /// <summary>
        /// 卸载视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <returns>true：卸载成功  false：视图不存在 卸载失败</returns>
        public bool UnloadView(string viewName) 
        {
            if (viewDic.TryGetValue(viewName, out var view))
            {
                view.OnUnload();
                Destroy((view as MonoBehaviour).gameObject);
                viewDic.Remove(viewName);
                return true;
            }
            return false;
        }
        public bool UnloadView<T>() where T : IUIView
        {
            return UnloadView(typeof (T).Name);
        }

        /// <summary>
        /// 关闭所有视图
        /// </summary>
        /// <param name="unload">是否卸载</param>
        public void CloseAllView(bool unload)
        {
            foreach (var view in viewDic.Values)
            {
                //调用其关闭事件
                view.OnClose(true);

                //卸载
                if (unload)
                {
                    //首先调用其卸载事件
                    view.OnUnload();
                    //再销毁其物体
                    Destroy((view as MonoBehaviour).gameObject);
                }
            }
            if (unload)
                viewDic.Clear();
        }
    }
}