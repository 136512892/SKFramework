/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Networking;

namespace SK.Framework.Config 
{
    public abstract class ConfigLoader : IConfigLoader
    {
        public abstract Dictionary<int, T> Load<T>(string filePath) where T : class;

        public abstract void LoadAsync<T>(string filePath, Action<bool, Dictionary<int, T>> onCompleted) where T : class;

        public abstract void LoadAsyncFromStreamingAssets<T>(string filePath, Action<bool, Dictionary<int, T>> onCompleted) where T : class;

        protected IEnumerator LoadCoroutine<T>(string filePath, Action<string, Dictionary<int, T>> parseAction, Action<bool, Dictionary<int, T>> onCompleted) where T : class
        {
            using (var request = UnityWebRequest.Get(filePath))
            {
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    var dic = new Dictionary<int, T>();
                    parseAction.Invoke(request.downloadHandler.text, dic);
                    onCompleted?.Invoke(true, dic);
                }
                else
                {
                    onCompleted?.Invoke(false, null);
                }
            }
        }
    }
}