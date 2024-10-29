/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System.IO;
using UnityEngine;

namespace SK.Framework
{
    public static class IOUtility
    {
        public static string streamingAssetsPath
        {
            get
            {
#if UNITY_ANDROID
                return "jar:file://" + Application.dataPath + "!/assets";
#elif UNITY_IOS
                return "file://" + Application.streamingAssetsPath;
#else
                return Application.streamingAssetsPath;
#endif
            }
        }

        public static string Combine(string path, string beCombined)
        {
            return Path.Combine(path, beCombined);
        }

        public static bool IsFileExists(string path)
        {
            return File.Exists(path);
        }

        public static bool DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }

        public static bool IsDirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public static bool CreateDirectory(string path)
        {
            if (!Directory.Exists(path)) 
            {
                Directory.CreateDirectory(path);
                return true;
            }
            return false;
        }

        public static bool DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
                return true;
            }
            return false;
        }
    }
}