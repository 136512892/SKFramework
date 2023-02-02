using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

namespace SK.Framework.Networking
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/Web Request")]
    public class WebRequestComponent : MonoBehaviour
    {
        public void Send(string url, WebRequestData data, Action<DownloadHandler> onSuccess, Action<string> onFailure = null, MonoBehaviour sender = null)
        {
            (sender != null ? sender : this).StartCoroutine(SendWebRequestCoroutine(url, data, onSuccess, onFailure));
        }

        private IEnumerator SendWebRequestCoroutine(string url, WebRequestData data, Action<DownloadHandler> onSuccess, Action<string> onFailure = null)
        {
            using (UnityWebRequest request = data.RequestType == WebRequestType.GET
                ? UnityWebRequest.Get(url)
                : data.WWWForm == null
                ? UnityWebRequest.Post(url, UnityWebRequest.kHttpVerbPOST)
                : UnityWebRequest.Post(url, data.WWWForm))
            {
                if (data.PostData != null)
                {
                    request.uploadHandler = new UploadHandlerRaw(data.PostData);
                    request.downloadHandler = new DownloadHandlerBuffer();
                }
                if (data.Headers != null && data.Headers.Count > 0)
                {
                    foreach (var kv in data.Headers)
                    {
                        request.SetRequestHeader(kv.Key, kv.Value);
                    }
                }
#if UNITY_2017_2_OR_NEWER
                yield return request.SendWebRequest();
#else
                yield return request.Send();
#endif
                bool flag = false;
#if UNITY_2020_2_OR_NEWER
                flag = request.result == UnityWebRequest.Result.Success;
#elif UNITY_2017_1_OR_NEWER
                flag = !(request.isNetworkError || request.isHttpError);
#else
                flag = !request.isError;
#endif
                if (flag)
                {
                    onSuccess?.Invoke(request.downloadHandler);
                }
                else
                {
                    onFailure?.Invoke(request.error);
                }
            }
        }
    }
}