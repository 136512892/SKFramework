using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// 批量打包配置文件
    /// </summary>
    [CreateAssetMenu]
    public class BatchBuildProfile : ScriptableObject
    {
        public List<BuildTask> tasks = new List<BuildTask>(0);
    }
}