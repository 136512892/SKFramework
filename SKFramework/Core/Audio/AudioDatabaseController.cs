using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Audio
{
    /// <summary>
    /// 音频库管理器
    /// </summary>
    public class AudioDatabaseController 
    {
        //音频库列表
        private readonly List<AudioDatabase> list;

        public AudioDatabaseController()
        {
            list = new List<AudioDatabase>();
        }
        
        /// <summary>
        /// 加载音频库
        /// </summary>
        /// <param name="resourcesPath">音频库资源路径</param>
        /// <param name="database">音频库</param>
        /// <returns>0：加载成功； -1：目标音频库已存在，无需重复加载； -2：加载失败</returns>
        public int Load(string resourcesPath, out AudioDatabase database)
        {
            database = Resources.Load<AudioDatabase>(resourcesPath);
            if (database != null)
            {
                string databaseName = database.databaseName;
                var index = list.FindIndex(m => m.databaseName == databaseName);
                if (index == -1)
                {
                    list.Add(database);
                    return 0;
                }
                return -1;
            }
            return -2;
        }
        /// <summary>
        /// 卸载音频库
        /// </summary>
        /// <param name="databaseName">音频库名称</param>
        /// <returns>true：卸载成功； false：卸载失败</returns>
        public bool Unload(string databaseName)
        {
            var target = list.Find(m => m.databaseName == databaseName);
            if (target != null)
            {
                list.Remove(target);
                Resources.UnloadAsset(target);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取音频库
        /// </summary>
        /// <param name="databaseName">音频库名称</param>
        /// <returns>音频库</returns>
        public AudioDatabase Get(string databaseName)
        {
            return list.Find(m => m.databaseName == databaseName);
        }
    }
}