using System;
using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// 问题选项
    /// </summary>
    [Serializable]
    public class QuestionChoice
    {
        public string text;
        public Sprite pic;

        public QuestionChoice(string text, Sprite pic)
        {
            this.text = text;
            this.pic = pic;
        }
    }
}