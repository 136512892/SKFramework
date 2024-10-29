/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework
{
    public static class DateTimeUtility
    {
        public static double GetTimeStamp(DateTime dateTime)
        {
            return (dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds;
        }

        public static string ToChinese(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday: return "星期一";
                case DayOfWeek.Tuesday: return "星期二";
                case DayOfWeek.Wednesday: return "星期三";
                case DayOfWeek.Thursday: return "星期四";
                case DayOfWeek.Friday: return "星期五";
                case DayOfWeek.Saturday: return "星期六";
                case DayOfWeek.Sunday: return "星期日";
                default: return null;
            }
        }

        public static string ToChinese2(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday: return "周一";
                case DayOfWeek.Tuesday: return "周二";
                case DayOfWeek.Wednesday: return "周三";
                case DayOfWeek.Thursday: return "周四";
                case DayOfWeek.Friday: return "周五";
                case DayOfWeek.Saturday: return "周六";
                case DayOfWeek.Sunday: return "周日";
                default: return null;
            }
        }
    }
}