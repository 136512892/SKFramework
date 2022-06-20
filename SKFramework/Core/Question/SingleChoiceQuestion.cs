using System;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// 单项选择题
    /// </summary>
    [Serializable]
    public class SingleChoiceQuestion : QuestionBase
    {
        /// <summary>
        /// 选项类型
        /// </summary>
        public ChoiceType choiceType;
        /// <summary>
        /// 选项
        /// </summary>
        public List<QuestionChoice> Choices = new List<QuestionChoice>(0);
        /// <summary>
        /// 答案
        /// </summary>
        public int Answer;

        public override bool IsCorrect(params object[] answers)
        {
            int answer = (int)answers[0];
            return answer == Answer;
        }
    }
}