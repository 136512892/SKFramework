using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SK.Framework
{
    public static class StringExtension
    {
        /// <summary>
        /// 计算目标字符在字符串中出现的次数 不区分大小写
        /// </summary>
        /// <param name="target">目标字符</param>
        /// <returns>次数</returns>
        public static int CharCount(this string self, char target)
        {
            char[] charArray = self.ToCharArray();
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
        /// 尝试转化为枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns>转化结果</returns>
        public static T ToEnum<T>(this string self)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), self);
            }
            catch
            {
                return default;
            }
        }
        /// <summary>
        /// 尝试转化为枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns>转化结果</returns>
        public static T ToEnum<T>(this string self, bool ignoreCase)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), self, ignoreCase);
            }
            catch
            {
                return default;
            }
        }
        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <returns>字符串</returns>
        public static string UppercaseFirst(this string self)
        {
            return char.ToUpper(self[0]) + self.Substring(1);
        }
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <returns>是否存在</returns>
        public static bool FileExists(this string self)
        {
            return File.Exists(self);
        }
        /// <summary>
        /// 根据路径删除文件
        /// </summary>
        /// <returns>删除结果</returns>
        public static bool DeleteFile(this string self)
        {
            if (File.Exists(self))
            {
                File.Delete(self);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断文件夹是否存在
        /// </summary>
        /// <returns>是否存在</returns>
        public static bool DirectoryExists(this string self)
        {
            return Directory.Exists(self);
        }
        /// <summary>
        /// 根据路径创建文件夹
        /// </summary>
        /// <returns>文件夹路径</returns>
        public static string CreateDirectory(this string self)
        {
            if (!Directory.Exists(self))
            {
                Directory.CreateDirectory(self);
            }
            return self;
        }
        /// <summary>
        /// 根据路径删除文件夹
        /// </summary>
        /// <returns>删除结果</returns>
        public static bool DeleteDirectory(this string self)
        {
            if (Directory.Exists(self))
            {
                Directory.Delete(self);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 合并路径
        /// </summary>
        /// <param name="beCombined">被合并的路径</param>
        /// <returns>合并后的路径</returns>
        public static string PathCombine(this string self, string beCombined)
        {
            return Path.Combine(self, beCombined);
        }
        /// <summary>
        /// 转化为base64字符串
        /// </summary>
        /// <returns>base64字符串</returns>
        public static string ToBase64String(this string self)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(self));
        }
        /// <summary>
        /// 判断字符串是否包含中文
        /// </summary>
        /// <param name="self">字符串</param>
        /// <returns>若字符串包含中文返回true,否则返回false</returns>
        public static bool IsContainChinese(this string self)
        {
            return Regex.IsMatch(self, @"[\u4e00-\u9fa5]");
        }
        /// <summary>
        /// 判断字符串是否为16进制数据
        /// </summary>
        /// <returns>若字符串符合16进制数据格式返回true,否则返回false</returns>
        public static bool IsMatchHexadecimal(this string self)
        {
            return Regex.IsMatch(self, @"/^#?([a-f0-9]{6}|[a-f0-9]{3})$/");
        }
        /// <summary>
        /// 判断字符串是否为url
        /// </summary>
        /// <returns>若字符串符合url地址格式返回true,否则返回false</returns>
        public static bool IsMatchURL(this string self)
        {
            return Regex.IsMatch(self, @"/^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/");
        }
        /// <summary>
        /// 判断字符串是否为IP地址
        /// </summary>
        /// <returns>若字符串符合IP地址格式返回true,否则返回false</returns>
        public static bool IsMatchIPAddress(this string self)
        {
            return Regex.IsMatch(self, @"/^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/");
        }
        /// <summary>
        /// 判断字符串是否为Email邮箱
        /// </summary>
        /// <returns>若字符串符合Email邮箱格式返回true,否则返回false</returns>
        public static bool IsMatchEmail(this string self)
        {
            return Regex.IsMatch(self, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        /// <summary>
        /// 判断字符串是否为手机号码
        /// </summary>
        /// <returns>若字符串符合手机号码格式返回true,否则返回false</returns>
        public static bool IsMatchMobilePhoneNumber(this string self)
        {
            return Regex.IsMatch(self, @"^0{0,1}(13[4-9]|15[7-9]|15[0-2]|18[7-8])[0-9]{8}$");
        }
    }
}