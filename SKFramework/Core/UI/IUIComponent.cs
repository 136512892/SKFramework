using System;
using UnityEngine;

namespace SK.Framework.UI
{
    public interface IUIComponent
    {
        /// <summary>
        /// 加载视图（Resources加载方式）
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图命名</param>
        /// <param name="resourcesPath">Resources资源路径</param>
        /// <param name="viewLevel">视图层级</param>
        /// <param name="data">视图数据</param>
        /// <returns>视图</returns>
        T LoadView<T>(string viewName, string resourcesPath, ViewLevel viewLevel = ViewLevel.COMMON, object data = null) where T : MonoBehaviour, IUIView;

        /// <summary>
        /// 异步加载视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图命名</param>
        /// <param name="assetPath">资产路径</param>
        /// <param name="viewLevel">视图层级</param>
        /// <param name="data">视图数据</param>
        /// <param name="onCompleted">加载的回调事件</param>
        void LoadViewAsync<T>(string viewName, string assetPath, ViewLevel viewLevel = ViewLevel.COMMON, object data = null, Action<bool, T> onCompleted = null) where T : MonoBehaviour, IUIView;

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
        T OpenView<T>(string viewName, string resourcesPath, ViewLevel viewLevel = ViewLevel.COMMON, bool instant = false, object data = null) where T : MonoBehaviour, IUIView;

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
        void OpenViewAsync<T>(string viewName, string assetPath, ViewLevel viewLevel = ViewLevel.COMMON, bool instant = false, object data = null, Action<bool, T> onCompleted = null) where T : MonoBehaviour, IUIView;

        /// <summary>
        /// 尝试打开视图（如果视图已经加载，将其打开）
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图名称</param>
        /// <param name="instant">是否立即显示</param>
        /// <param name="data">视图数据</param>
        /// <returns>视图</returns>
        T TryOpenView<T>(string viewName, bool instant = false, object data = null) where T : MonoBehaviour, IUIView;

        /// <summary>
        /// 关闭视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="instant">是否立即关闭</param>
        /// <returns></returns>
        bool CloseView(string viewName, bool instant = false);

        /// <summary>
        /// 是否存在视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <returns>true：存在  false：不存在</returns>
        bool HasView(string viewName);

        /// <summary>
        /// 获取视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewName">视图名称</param>
        /// <returns>视图</returns>
        T GetView<T>(string viewName) where T : IUIView;
        /// <summary>
        /// 卸载视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <returns>true：卸载成功  false：视图不存在 卸载失败</returns>
        bool UnloadView(string viewName);

        /// <summary>
        /// 关闭所有视图
        /// </summary>
        /// <param name="unload">是否卸载</param>
        void CloseAllView(bool unload);
    }
}