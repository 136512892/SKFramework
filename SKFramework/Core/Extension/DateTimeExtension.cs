using System;

namespace SK.Framework
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns>时间戳</returns>
        public static double GetTimeStamp(this DateTime self)
        {
            //计算机元年 1970年1月1日0时0分0秒
            return (self - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
        }

        /// <summary>
        /// 转换为中文
        /// </summary>
        /// <param name="prefix">前缀 周/星期</param>
        /// <returns></returns>
        public static string ToChinese(this DayOfWeek self, string prefix)
        {
            switch (self)
            {
                case DayOfWeek.Monday: return $"{prefix}一";
                case DayOfWeek.Tuesday: return $"{prefix}二";
                case DayOfWeek.Wednesday: return $"{prefix}三";
                case DayOfWeek.Thursday: return $"{prefix}四";
                case DayOfWeek.Friday: return $"{prefix}五";
                case DayOfWeek.Saturday: return $"{prefix}六";
                case DayOfWeek.Sunday: return $"{prefix}日";
                default: return null;
            }
        }

        // yyyy/MM/dd HH:mm:ss:fff
    }
}