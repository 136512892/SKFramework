using System;

namespace SK.Framework
{
    public static class IntExtension
    {
        /// <summary>
        /// 转化为字母 (1-26表示字母A-Z)
        /// </summary>
        /// <returns>字母字符</returns>
        public static char ToLetter(this int self)
        {
            if (self < 1 || self > 26) return default;
            return Convert.ToChar('A' + self - 1);
        }
        /// <summary>
        /// 阶乘
        /// </summary>
        /// <returns>阶乘结果</returns>
        public static int Fact(this int self)
        {
            if (self == 0)
            {
                return 1;
            }
            else
            {
                return self * Fact(self - 1);
            }
        }
    }
}