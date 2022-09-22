using System.IO;

namespace SK.Framework.Utility
{
    public class IOUtility
    {
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>是否存在</returns>
        public static bool IsExistsFile(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// 根据路径删除文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>删除结果</returns>
        public static bool DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断文件夹是否存在
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns>是否存在</returns>
        public static bool IsExistsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// 根据路径创建文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns>文件夹路径</returns>
        public static string CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        /// <summary>
        /// 根据路径删除文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns>删除结果</returns>
        public static bool DeleteDirectory(string self)
        {
            if (Directory.Exists(self))
            {
                Directory.Delete(self);
                return true;
            }
            return false;
        }
    }
}