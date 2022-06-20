using System;

namespace SK.Framework
{
    /// <summary>
    /// 判断题
    /// </summary>
    [Serializable]
    public class JudgeQuestion : QuestionBase
    {
        /// <summary>
        /// 积极选项
        /// </summary>
        public string Positive = "正确";
        /// <summary>
        /// 消极选项
        /// </summary>
        public string Negative = "错误";
        /// <summary>
        /// 答案
        /// </summary>
        public bool Answer;

        public override bool IsCorrect(params object[] answers)
        {
            bool answer = (bool)answers[0];
            return answer == Answer;
        }
    }
}