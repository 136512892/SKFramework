using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace SK.Framework
{
    /// <summary>
    /// 场景加载器
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        private GetSceneMode getSceneMode = GetSceneMode.Name;
        private LoadSceneMode loadSceneMode = LoadSceneMode.Single;
        private string sceneName;
        private int sceneBuildIndex;
        private float sceneActivationDelay = 3f;

        //加载开始事件
        private Action onBegan;
        //加载中事件
        private Action<float> onLoading;
        //加载完成事件
        private Action onCompleted;

        /// <summary>
        /// 加载进度 0-1
        /// </summary>
        public float Progress { get; private set; }

        private IEnumerator LoadCoroutine()
        {
            yield return null;
            //调用加载开始事件
            onBegan?.Invoke();
            yield return null;
            AsyncOperation asyncOperation = null;
            switch (getSceneMode)
            {
                case GetSceneMode.Name: asyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode); break;
                case GetSceneMode.BuildIndex: asyncOperation = SceneManager.LoadSceneAsync(sceneBuildIndex, loadSceneMode); break;
            }
            //不允许场景激活
            asyncOperation.allowSceneActivation = false;
            while (asyncOperation.progress < .9f)
            {
                //真实加载进度占总进度20%
                Progress = Mathf.Clamp01(asyncOperation.progress / .9f) * .2f;
                onLoading?.Invoke(Progress);
                Log.Info("<color=cyan><b>[SKFramework.Scene.Info]</b></color> 场景加载进度[{0}]", Progress);
                yield return null;
            }
            //开始延时时间
            float delayBeginTime = Time.realtimeSinceStartup;
            while ((Time.realtimeSinceStartup - delayBeginTime) < sceneActivationDelay)
            {
                //延时进度
                float t = (Time.realtimeSinceStartup - delayBeginTime) / sceneActivationDelay;
                //延时进度占总进度的80%
                Progress = Mathf.Clamp01(t) * .8f + .2f;
                onLoading?.Invoke(Progress);
                Log.Info("<color=cyan><b>[SKFramework.Scene.Info]</b></color> 场景加载进度[{0}]", Progress);
                yield return null;
            }
            //延时完成后允许场景激活
            asyncOperation.allowSceneActivation = true;
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
            Log.Info(message: "<color=cyan><b>[SKFramework.Scene.Info]</b></color> 场景加载完成");

            //调用加载完成事件
            onCompleted?.Invoke();
            Destroy(gameObject);
        }

        public SceneLoader LoadAsync()
        {
            StartCoroutine(LoadCoroutine());
            return this;
        }
        public SceneLoader LoadAsync(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            this.sceneName = sceneName;
            getSceneMode = GetSceneMode.Name;
            this.loadSceneMode = loadSceneMode;
            StartCoroutine(LoadCoroutine());
            return this;
        }
        public SceneLoader LoadAsync(int sceneBuildIndex, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            this.sceneBuildIndex = sceneBuildIndex;
            getSceneMode = GetSceneMode.BuildIndex;
            this.loadSceneMode = loadSceneMode;
            StartCoroutine(LoadCoroutine());
            return this;
        }

        /// <summary>
        /// 设置场景激活延时
        /// </summary>
        /// <param name="sceneActivationDelay">延时时长</param>
        /// <returns></returns>
        public SceneLoader SetSceneActivationDelay(float sceneActivationDelay)
        {
            this.sceneActivationDelay = sceneActivationDelay;
            return this;
        }
        /// <summary>
        /// 设置开始加载事件
        /// </summary>
        /// <param name="onBegan"></param>
        /// <returns></returns>
        public SceneLoader OnBegan(Action onBegan)
        {
            this.onBegan = onBegan;
            return this;
        }
        /// <summary>
        /// 设置加载中事件
        /// </summary>
        /// <param name="onLoading"></param>
        /// <returns></returns>
        public SceneLoader OnLoading(Action<float> onLoading)
        {
            this.onLoading = onLoading;
            return this;
        }
        /// <summary>
        /// 设置加载完成事件
        /// </summary>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public SceneLoader OnCompleted(Action onCompleted)
        {
            this.onCompleted = onCompleted;
            return this;
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="sceneActivationDelay">激活延迟时长</param>
        /// <param name="loadSceneMode">场景加载方式</param>
        /// <returns></returns>
        public static SceneLoader LoadAsync(string sceneName, float sceneActivationDelay = 3f, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            var loader = new GameObject(string.Format("[SceneLoader.{0}]", sceneName)).AddComponent<SceneLoader>();
            DontDestroyOnLoad(loader);
            loader.SetSceneActivationDelay(sceneActivationDelay);
            loader.LoadAsync(sceneName, loadSceneMode);
            return loader;
        }
        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneBuildIndex">场景指针</param>
        /// <param name="sceneActivationDelay">激活延迟时长</param>
        /// <param name="loadSceneMode">场景加载方式</param>
        /// <returns></returns>
        public static SceneLoader LoadAsync(int sceneBuildIndex, float sceneActivationDelay = 3f, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            var loader = new GameObject(string.Format("[SceneLoader.{0}]", sceneBuildIndex)).AddComponent<SceneLoader>();
            DontDestroyOnLoad(loader);
            loader.SetSceneActivationDelay(sceneActivationDelay);
            loader.LoadAsync(sceneBuildIndex, loadSceneMode);
            return loader;
        }
    }
}