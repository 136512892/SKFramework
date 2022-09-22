using System.IO;
using UnityEngine;

namespace SK.Framework.Utility
{
    public class PathUtility
    {
        public static string StreamingAssetsPath
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


        /// <summary>
        /// 合并路径
        /// </summary>
        /// <param name="path">被合并的路径</param>
        /// <param name="beCombined">被合并的路径</param>
        /// <returns>合并后的路径</returns>
        public static string Combine(string path, string beCombined)
        {
            return Path.Combine(path, beCombined);
        }
    }
}