namespace SK.Framework
{
    /// <summary>
    /// 问题基类
    /// </summary>
    public abstract class QuestionBase
    {
        /// <summary>
        /// 题号
        /// </summary>
        public int Sequence;
        /// <summary>
        /// 问题
        /// </summary>
        public string Question;
        /// <summary>
        /// 题型
        /// </summary>
        public QuestionType Type;
        /// <summary>
        /// 题解
        /// </summary>
        public string Analysis;

        /// <summary>
        /// 判断答案是否正确
        /// </summary>
        /// <param name="answers">答案</param>
        /// <returns>答案正确返回true 否则返回false</returns>
        public abstract bool IsCorrect(params object[] answers);
    }
}