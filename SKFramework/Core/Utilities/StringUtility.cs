using System;
using System.Text.RegularExpressions;

namespace SK.Framework.Utility
{
    public class StringUtility
    {
        /// <summary>
        /// 计算目标字符在字符串中出现的次数 不区分大小写
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="target">目标字符</param>
        /// <returns>次数</returns>
        public static int CharCount(string str, char target)
        {
            char[] charArray = str.ToCharArray();
            string targetStr = target.ToString().ToLower();
            int retV = 0;
            for (int i = 0; i < charArray.Length; i++)
            {
                if (targetStr == charArray[i].ToString().ToLower())
                {
                    retV++;
                }
            }
            return retV;
        }

        /// <summary>
        /// 尝试将字符串转化为枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="str">字符串</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns>转化结果</returns>
        public static T ToEnum<T>(string str, bool ignoreCase)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), str, ignoreCase);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// 判断字符串是否包含中文
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>若字符串包含中文返回true,否则返回false</returns>
        public static bool IsContainChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        /// <summary>
        /// 判断字符串是否为16进制数据
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>若字符串符合16进制数据格式返回true,否则返回false</returns>
        public static bool IsMatchHexadecimal(string str)
        {
            return Regex.IsMatch(str, @"/^#?([a-f0-9]{6}|[a-f0-9]{3})$/");
        }

        /// <summary>
        /// 判断字符串是否为url
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>若字符串符合url地址格式返回true,否则返回false</returns>
        public static bool IsMatchURL(string str)
        {
            return Regex.IsMatch(str, @"/^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/");
        }

        /// <summary>
        /// 判断字符串是否为IP地址
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>若字符串符合IP地址格式返回true,否则返回false</returns>
        public static bool IsMatchIPAddress(string str)
        {
            return Regex.IsMatch(str, @"/^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/");
        }

        /// <summary>
        /// 判断字符串是否为Email邮箱
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>若字符串符合Email邮箱格式返回true,否则返回false</returns>
        public static bool IsMatchEmail(string str)
        {
            return Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 判断字符串是否为手机号码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>若字符串符合手机号码格式返回true,否则返回false</returns>
        public static bool IsMatchMobilePhoneNumber(string str)
        {
            return Regex.IsMatch(str, @"^0{0,1}(13[4-9]|15[7-9]|15[0-2]|18[7-8])[0-9]{8}$");
        }
    }
}