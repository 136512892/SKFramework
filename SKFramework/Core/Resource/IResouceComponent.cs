using System;
using Object = UnityEngine.Object;

namespace SK.Framework.Resource
{
    public interface IResouceComponent
    {
        /// <summary>
        /// 异步加载资产
        /// </summary>
        /// <typeparam name="T">资产类型</typeparam>
        /// <param name="assetPath">资产路径</param>
        /// <param name="onCompleted">加载完成回调事件</param>
        /// <param name="onLoading">加载进度回调事件</param>
        void LoadAssetAsync<T>(string assetPath, Action<bool, T> onCompleted, Action<float> onLoading) where T : Object;

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneAssetPath">场景资产路径</param>
        /// <param name="onCompleted">加载完成回调事件</param>
        /// <param name="onLoading">加载过程回调事件</param>
        void LoadSceneAsync(string sceneAssetPath, Action<bool> onCompleted, Action<float> onLoading);

        /// <summary>
        /// 卸载资产
        /// </summary>
        /// <param name="assetPath">资产路径</param>
        /// <param name="unloadAllLoadedObjects">是否卸载相关实例化对象</param>
        void UnloadAsset(string assetPath, bool unloadAllLoadedObjects);

        /// <summary>
        /// 卸载所有资产
        /// </summary>
        /// <param name="unloadAllLoadedObjects">是否卸载相关实例化对象</param>
        void UnloadAllAsset(bool unloadAllLoadedObjects);

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneAssetPath">场景资产路径</param>
        /// <returns>true：卸载成功  false：卸载失败</returns>
        bool UnloadScene(string sceneAssetPath);

        /// <summary>
        /// 异步实例化
        /// </summary>
        /// <typeparam name="T">资产类型</typeparam>
        /// <param name="assetPath">资产路径</param>
        /// <param name="onCompleted">结果回调事件</param>
        /// <param name="onProgress">进度回调事件</param>
        void InstantiateAsync<T>(string assetPath, Action<bool, T> onCompleted, Action<float> onProgress) where T : Object;
    }
}