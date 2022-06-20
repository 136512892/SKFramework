using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// 分数组合
    /// </summary>
    public class ScoreGroup
    {
        /// <summary>
        /// 组合描述
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// 计分模式
        /// Additive表示组合内分值进行累加
        /// MutuallyExclusive表示组内各分数项互斥 获得其中一项分值 则取消其它项分值
        /// </summary>
        public ValueMode ValueMode { get; private set; }
        /// <summary>
        /// 分数项列表
        /// </summary>
        public readonly List<ScoreItem> Scores;

        public ScoreGroup(string description, ValueMode valueMode, params ScoreItem[] scores)
        {
            Description = description;
            ValueMode = valueMode;
            Scores = new List<ScoreItem>(scores);
        }

        /// <summary>
        /// 获取指定标识分数项的分值
        /// </summary>
        /// <param name="flag">分数项标识</param>
        /// <returns>获取成功返回true 否则返回false</returns>
        public bool Obtain(string flag)
        {
            var target = Scores.Find(m => m.Flag == flag);
            if (target != null)
            {
                switch (ValueMode)
                {
                    case ValueMode.Additive: target.IsObtained = true; break;
                    case ValueMode.MutuallyExclusive:
                        for (int i = 0; i < Scores.Count; i++)
                        {
                            Scores[i].IsObtained = Scores[i] == target;
                        }
                        break;
                    default: break;
                }
                Log.Info("<color=cyan><b>[SKFramework.Score.Info]</b></color> 获得分数组合[{0}]中标识为[{1}]的分数项的分值", Description, flag);
                return true;
            }
            Log.Info("<color=cyan><b>[SKFramework.Score.Info]</b></color> 分数组合[{0}]不存在标识为[{1}]的分数项", Description, flag);
            return false;
        }
        /// <summary>
        /// 取消指定分数项的分值
        /// </summary>
        /// <param name="flag">分数项标识</param>
        /// <returns>取消成功返回true 否则返回false</returns>
        public bool Cancle(string flag)
        {
            var target = Scores.Find(m => m.Flag == flag);
            if (target != null)
            {
                if (target.IsObtained)
                {
                    target.IsObtained = false;
                    Log.Info("<color=cyan><b>[SKFramework.Score.Info]</b></color> 取消分数组合[{0}]中标识为[{1}]的分数项的分值", Description, flag);
                    return true;
                }
                return false;
            }
            Log.Info("<color=cyan><b>[SKFramework.Score.Info]</b></color> 分数组合[{0}]不存在标识为[{1}]的分数项", Description, flag);
            return false;
        }
        /// <summary>
        /// 删除指定的分数项
        /// </summary>
        /// <param name="flag">分数项标识</param>
        /// <returns>成功删除返回true 否则返回false</returns>
        public bool Delete(string flag)
        {
            var target = Scores.Find(m => m.Flag == flag);
            if (target != null)
            {
                Scores.Remove(target);
                Log.Info("<color=cyan><b>[SKFramework.Score.Info]</b></color> 分数组合[{0}]删除标识为[{1}]的分数项", Description, flag);
                return true;
            }
            Log.Info("<color=cyan><b>[SKFramework.Score.Info]</b></color> 分数组合[{0}]不存在标识为[{1}]的分数项", Description, flag);
            return false;
        }
        /// <summary>
        /// 该分数组合已经获得的总分值
        /// </summary>
        /// <returns></returns>
        public float GetSum()
        {
            float retV = 0f;
            for (int i = 0; i < Scores.Count; i++)
            {
                var score = Scores[i];
                retV += score.IsObtained ? score.Value : 0f;
            }
            return retV;
        }
    }
}