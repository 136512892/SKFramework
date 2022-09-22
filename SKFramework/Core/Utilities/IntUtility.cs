using System;

namespace SK.Framework.Utility
{
    public class IntUtility
    {
        /// <summary>
        /// 转化为字母 (1-26表示字母A-Z)
        /// </summary>
        /// <param name="v"></param>
        /// <returns>字母字符</returns>
        public static char ToLetter(int v)
        {
            if (v < 1 || v > 26) return default;
            return Convert.ToChar('A' + v - 1);
        }
        /// <summary>
        /// 阶乘
        /// </summary>
        /// <param name="v"></param>
        /// <returns>阶乘结果</returns>
        public static int Fact(int v)
        {
            if (v == 0)
            {
                return 1;
            }
            else
            {
                return v * Fact(v - 1);
            }
        }
    }
}