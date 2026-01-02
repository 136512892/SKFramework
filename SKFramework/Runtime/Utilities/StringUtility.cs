/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Text.RegularExpressions;

namespace SK.Framework
{
    public static class StringUtility
    {
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

        public static bool IsContainChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        public static bool IsMatchHexadecimal(string str)
        {
            return Regex.IsMatch(str, @"/^#?([a-f0-9]{6}|[a-f0-9]{3})$/");
        }

        public static bool IsMatchURL(string str)
        {
            return Regex.IsMatch(str, @"/^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/");
        }

        public static bool IsMatchIPAddress(string str)
        {
            return Regex.IsMatch(str, @"/^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/");
        }

        public static bool IsMatchEmail(string str)
        {
            return Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static bool IsMatchMobilePhoneNumber(string str)
        {
            return Regex.IsMatch(str, @"^1[3-9]\d{9}$");
        }
    }
}