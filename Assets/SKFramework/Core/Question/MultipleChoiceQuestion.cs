using System;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// 多项选择题
    /// </summary>
    [Serializable]
    public class MultipleChoiceQuestion : QuestionBase
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
        public List<int> Answers = new List<int>(0);

        public override bool IsCorrect(params object[] answers)
        {
            if (Answers.Count != answers.Length) return false;
            for(int i = 0; i < answers.Length; i++)
            {
                int answer = (int)answers[i];
                if (Answers[i] != answer)
                {
                    return false;
                }
            }
            return true;
        }
    }
}