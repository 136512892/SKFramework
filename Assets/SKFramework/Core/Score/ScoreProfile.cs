using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// 分数配置文件
    /// </summary>
    [CreateAssetMenu]
    public class ScoreProfile : ScriptableObject
    {
        public ScoreInfo[] scores = new ScoreInfo[0];
    }
}