using System;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// 填空题
    /// </summary>
    [Serializable]
    public class CompletionQuestion : QuestionBase
    {
        /// <summary>
        /// 答案
        /// </summary>
        public List<string> Answers = new List<string>(0);

        public override bool IsCorrect(params object[] answers)
        {
            if (answers.Length != Answers.Count) return false;
            for(int i = 0; i < answers.Length; i++)
            {
                string answer = answers[i] as string;
                if (answer != Answers[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}