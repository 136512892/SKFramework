namespace SK.Framework
{
    /// <summary>
    /// 分数项
    /// </summary>
    public class ScoreItem
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string Flag { get; private set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// 分值
        /// </summary>
        public float Value { get; private set; }
        /// <summary>
        /// 是否已经获得分值
        /// </summary>
        public bool IsObtained { get; set; }

        public ScoreItem(string flag, string description, float value)
        {
            Flag = flag;
            Description = description;
            Value = value;
        }
    }
}