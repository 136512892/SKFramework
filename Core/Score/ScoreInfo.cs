using System;
using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// 分数信息
    /// </summary>
    [Serializable]
    public class ScoreInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        [ScoreID]
        public int id;
        /// <summary>
        /// 描述
        /// </summary>
        [TextArea]
        public string description;
        /// <summary>
        /// 分值
        /// </summary>
        public float value;
    }
}