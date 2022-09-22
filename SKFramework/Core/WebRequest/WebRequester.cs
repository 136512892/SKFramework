using System;
using UnityEngine;
using System.Text;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace SK.Framework.WebRequest
{
    /// <summary>
    /// 网络请求器
    /// </summary>
    public class WebRequester : MonoBehaviour
    {
        private static WebRequester instance;
        private WebInterfaceProfile profile;
        private Dictionary<string, AbstractWebInterface> dic;

        public static WebRequester Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new GameObject("[SKFramework.WebRequest]").AddComponent<WebRequester>();
                    instance.dic = new Dictionary<string, AbstractWebInterface>();
                    instance.profile = Resources.Load<WebInterfaceProfile>("WebInterface Profile");
                    if (instance.profile == null)
                    {
                        Debug.LogError("加载配置文件失败");
                    }
                    DontDestroyOnLoad(instance);
                }
                return instance;
            }
        }

        /// <summary>
        /// GET方式调用接口
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="callback">回调函数</param>
        public static void GET(string url, Action<string> callback)
        {
            Instance.StartCoroutine(GetCoroutine(url, callback));
        }
        private static IEnumerator GetCoroutine(string url, Action<string> callback)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();
                //if (!request.isHttpError && !request.isNetworkError)
                if (request.result == UnityWebRequest.Result.Success)
                {
                    callback?.Invoke(request.downloadHandler.text);
                }
                else
                {
                    Debug.LogError(string.Format("发起网络请求[{0}]失败: {1}", url, request.error));
                }
            }
        }
        /// <summary>
        /// POST方式调用接口
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="postData">POST数据</param>
        /// <param name="callback">回调函数</param>
        /// <param name="headers">请求头 例如Content-Type=application/json</param>
        public static void POST(string url, string postData, Action<string> callback, params string[] headers)
        {
            Instance.StartCoroutine(PostCoroutine(url, postData, callback, headers));
        }
        private static IEnumerator PostCoroutine(string url, string postData, Action<string> callback, params string[] headers)
        {
            using(UnityWebRequest request = UnityWebRequest.Post(url, UnityWebRequest.kHttpVerbPOST))
            {
                if (!string.IsNullOrEmpty(postData))
                {
                    byte[] postBytes = Encoding.UTF8.GetBytes(postData);
                    request.uploadHandler = new UploadHandlerRaw(postBytes);
                }
                request.downloadHandler = new DownloadHandlerBuffer();
                for (int i = 0; i < headers.Length; i++)
                {
                    string[] kv = headers[i].Split('=');
                    request.SetRequestHeader(kv[0], kv[1]);
                }
                yield return request.SendWebRequest();
                //if (!request.isHttpError && !request.isNetworkError)
                if (request.result == UnityWebRequest.Result.Success)
                {
                    callback?.Invoke(request.downloadHandler.text);
                }
                else
                {
                    Debug.LogError(string.Format("发起网络请求[{0}]失败: {1}", url, request.error));
                }
            }
        }


        /// <summary>
        /// 注册网络接口
        /// </summary>
        /// <typeparam name="T">网络接口类型</typeparam>
        /// <param name="webInterfaceName">网络接口名称</param>
        /// <param name="t">网络接口</param>
        /// <returns>注册成功返回true 否则返回false</returns>
        public static bool RegisterInterface<T>(string webInterfaceName, out T target) where T : AbstractWebInterface
        {
            target = null;
            if (!Instance.dic.TryGetValue(webInterfaceName, out AbstractWebInterface webInterface))
            {
                var info = Array.Find(instance.profile.data, m => m.name == webInterfaceName);
                if (info != null)
                {
                    T t = Activator.CreateInstance<T>();
                    t.name = info.name;
                    t.url = info.url;
                    t.method = info.method;
                    t.args = info.args;
                    Instance.dic.Add(webInterfaceName, t);
                    target = t;

                    return true;
                }
                return false;
            }
            return false;
        }
        /// <summary>
        /// 注销网络接口
        /// </summary>
        /// <param name="webInterfaceName">网络接口名称</param>
        /// <returns>注销成功返回true 否则返回false</returns>
        public static bool UnregisterInterface(string webInterfaceName)
        {
            if (Instance.dic.ContainsKey(webInterfaceName))
            {
                Instance.dic.Remove(webInterfaceName);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 调用网络接口 发起网络请求
        /// </summary>
        /// <param name="webInterfaceName">网络接口名称</param>
        /// <param name="args">参数</param>
        /// <returns>成功发起请求返回true 否则返回false</returns>
        public static bool SendWebRequest(string webInterfaceName, params string[] args)
        {
            if (Instance.dic.TryGetValue(webInterfaceName, out var webInterface))
            {
                webInterface.SendWebRequest(args);
                return true;
            }
            return false;
        }
    }
}