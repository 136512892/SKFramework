using System;

namespace SK.Framework
{
    /// <summary>
    /// 论述题
    /// </summary>
    [Serializable]
    public class EssayQuestion : QuestionBase
    {
        /// <summary>
        /// 答案
        /// </summary>
        public string Answer;

        public override bool IsCorrect(params object[] answers)
        {
            string answer = answers[0] as string;
            return answer == Answer;

            //TODO 论述题答案判断 
        }
    }
}