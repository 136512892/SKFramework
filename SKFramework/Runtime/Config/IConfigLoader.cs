/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections.Generic;

namespace SK.Framework.Config
{
    public interface IConfigLoader
    {
        Dictionary<int, T> Load<T>(string filePath) where T : class;

        void LoadAsync<T>(string filePath, Action<bool, Dictionary<int, T>> onCompleted) where T : class;

        void LoadAsyncFromStreamingAssets<T>(string filePath, Action<bool, Dictionary<int, T>> onCompleted) where T : class;
    }
}