using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// 问题配置文件
    /// </summary>
    [CreateAssetMenu(fileName = "New Questions Profile", menuName = "Question Profile")]
    public sealed class QuestionsProfile : ScriptableObject
    {
        /// <summary>
        /// 判断题列表
        /// </summary>
        public List<JudgeQuestion> Judges = new List<JudgeQuestion>(0);
        /// <summary>
        /// 单向选择题列表
        /// </summary>
        public List<SingleChoiceQuestion> SingleChoices = new List<SingleChoiceQuestion>(0);
        /// <summary>
        /// 多项选择题列表
        /// </summary>
        public List<MultipleChoiceQuestion> MultipleChoices = new List<MultipleChoiceQuestion>(0);
        /// <summary>
        /// 填空题列表
        /// </summary>
        public List<CompletionQuestion> Completions = new List<CompletionQuestion>(0);
        /// <summary>
        /// 论述题列表
        /// </summary>
        public List<EssayQuestion> Essays = new List<EssayQuestion>();

        public QuestionBase Get(int sequence)
        {
            JudgeQuestion judge = Judges.Find(m => m.Sequence == sequence);
            if (judge != null) return judge;
            SingleChoiceQuestion single = SingleChoices.Find(m => m.Sequence == sequence);
            if (single != null) return single;
            MultipleChoiceQuestion multiple = MultipleChoices.Find(m => m.Sequence == sequence);
            if (multiple != null) return multiple;
            CompletionQuestion completion = Completions.Find(m => m.Sequence == sequence);
            if (completion != null) return completion;
            EssayQuestion essay = Essays.Find(m => m.Sequence == sequence);
            if (essay != null) return essay;
            return null;
        }
    }
}