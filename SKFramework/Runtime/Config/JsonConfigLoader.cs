/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

using SK.Framework.Logger;
using ILogger = SK.Framework.Logger.ILogger;

namespace SK.Framework.Config
{
    public class JsonConfigLoader : ConfigLoader
    {
        public override Dictionary<int, T> Load<T>(string filePath) where T : class
        {
            var dic = new Dictionary<int, T>();
            TextAsset json = Resources.Load<TextAsset>(filePath);
            if (json == null)
            {
                ILogger logger = SKFramework.Module<Log>().GetLogger<ModuleLogger>();
                logger.Error("Json file not found: {0}", filePath);
                return dic;
            }
            ParseJsonText(json.text, dic);
            return dic;
        }

        public override void LoadAsync<T>(string filePath, Action<bool, Dictionary<int, T>> onCompleted = null) where T : class
        {
            SKFramework.Module<Resource.Resource>().LoadAssetAsync<TextAsset>(filePath, (success, textAsset) =>
            {
                if (success)
                {
                    var dic = new Dictionary<int, T>();
                    ParseJsonText(textAsset.text, dic);
                    onCompleted?.Invoke(true, dic);
                }
                else
                {
                    onCompleted?.Invoke(false, null);
                }
            });
        }

        public override void LoadAsyncFromStreamingAssets<T>(string filePath, Action<bool, Dictionary<int, T>> onCompleted = null) where T : class
        {
            var path = Path.Combine(IOUtility.streamingAssetsPath, filePath);
            SKFramework.Module<Config>().StartCoroutine(LoadCoroutine(path, ParseJsonText, onCompleted));
        }

        private void ParseJsonText<T>(string text, Dictionary<int, T> dic) where T : class
        {
            try
            {
                var wrapper = JsonUtility.FromJson<JsonArrayWapper<T>>($"{{\"items\":{text}}}");
                for (int i = 0; i < wrapper.items.Count; i++)
                {
                    var item = wrapper.items[i];
                    var idProperty = item.GetType().GetField("ID", BindingFlags.Instance | BindingFlags.Public);
                    int id = (int)idProperty.GetValue(item);
                    dic.Add(id, item);
                }
            }
            catch (Exception e)
            {
                ILogger logger = SKFramework.Module<Log>().GetLogger<ModuleLogger>();
                logger.Error("Json parse error: {0}", e.Message);
            }
        }

        [Serializable]
        private class JsonArrayWapper<T>
        {
            public List<T> items;
        }
    }
}