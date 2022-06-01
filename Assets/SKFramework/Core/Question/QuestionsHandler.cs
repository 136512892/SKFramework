using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// 问题处理器
    /// </summary>
    public class QuestionsHandler
    {
        //问题列表
        private List<QuestionBase> questions;
        //当前题号
        private int currentSequence;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="profile">问题配置文件</param>
        public QuestionsHandler(QuestionsProfile profile)
        {
            Init(profile);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="resourcesPath">配置文件的路径</param>
        public QuestionsHandler(string resourcesPath)
        {
            QuestionsProfile profile = Resources.Load<QuestionsProfile>(resourcesPath);
            if (profile == null)
            {
                Log.Info(Module.Question, string.Format("加载配置文件失败 {0}", resourcesPath));
            }
            else
            {
                Init(profile);
            }
        }

        //初始化
        private void Init(QuestionsProfile profile)
        {
            questions = new List<QuestionBase>();
            for (int i = 0; i < profile.Judges.Count; i++)
            {
                questions.Add(profile.Judges[i]);
            }
            for (int i = 0; i < profile.SingleChoices.Count; i++)
            {
                questions.Add(profile.SingleChoices[i]);
            }
            for (int i = 0; i < profile.MultipleChoices.Count; i++)
            {
                questions.Add(profile.MultipleChoices[i]);
            }
            for (int i = 0; i < profile.Completions.Count; i++)
            {
                questions.Add(profile.Completions[i]);
            }
            for (int i = 0; i < profile.Essays.Count; i++)
            {
                questions.Add(profile.Essays[i]);
            }
        }

        /// <summary>
        /// 上一题
        /// </summary>
        /// <returns></returns>
        public QuestionBase Last()
        {
            currentSequence--;
            currentSequence = Mathf.Clamp(currentSequence, 1, questions.Count);
            return questions.Find(m => m.Sequence == currentSequence);
        }
        /// <summary>
        /// 下一题
        /// </summary>
        /// <returns></returns>
        public QuestionBase Next()
        {
            currentSequence++;
            currentSequence = Mathf.Clamp(currentSequence, 1, questions.Count);
            return questions.Find(m => m.Sequence == currentSequence);

        }
        /// <summary>
        /// 根据题号切换到指定问题
        /// </summary>
        /// <param name="sequence">题号</param>
        /// <returns></returns>
        public QuestionBase Switch(int sequence)
        {
            currentSequence = sequence;
            currentSequence = Mathf.Clamp(currentSequence, 1, questions.Count);
            return questions.Find(m => m.Sequence == currentSequence);
        }

        /// <summary>
        /// 根据题号获取问题
        /// </summary>
        /// <typeparam name="T">题型</typeparam>
        /// <param name="sequence">题号</param>
        /// <returns></returns>
        public T Get<T>(int sequence) where T : QuestionBase
        {
            var question = questions.Find(m => m.Sequence == sequence);
            return question != null ? question as T : null;
        }
    }
}