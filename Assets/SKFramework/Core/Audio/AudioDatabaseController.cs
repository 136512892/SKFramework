using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework
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
        /// <returns>加载成功返回true 否则返回false</returns>
        public bool Load(string resourcesPath, out AudioDatabase database)
        {
            database = Resources.Load<AudioDatabase>(resourcesPath);
            if (database != null)
            {
                string databaseName = database.databaseName;
                var index = list.FindIndex(m => m.databaseName == databaseName);
                if (index == -1)
                {
                    list.Add(database);
                    Log.Info(Module.Audio, string.Format("成功加载音频库 {0}", database.name));
                    return true;
                }
                Log.Info(Module.Audio, string.Format("音频库[{0}]已存在 无需重复加载", database.name));
                return false;
            }
            Log.Error(Module.Audio, string.Format("加载音频库失败 {0}", resourcesPath));
            return false;
        }
        /// <summary>
        /// 卸载音频库
        /// </summary>
        /// <param name="databaseName">音频库名称</param>
        /// <returns>卸载成功返回true 否则返回false</returns>
        public bool Unload(string databaseName)
        {
            var target = list.Find(m => m.databaseName == databaseName);
            if (target != null)
            {
                list.Remove(target);
                Resources.UnloadAsset(target);
                Log.Info(Module.Audio, string.Format("成功卸载音频库 {0}", databaseName));
                return true;
            }
            Log.Error(Module.Audio, string.Format("卸载音频库[{0}]失败 不存在", databaseName));
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