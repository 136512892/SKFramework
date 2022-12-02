using System;

namespace SK.Framework.Utility
{
    public class DateTimeUtility
    {
        public static double GetTimeStamp(DateTime dateTime)
        {
            return (dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds;
        }

        public static string ToChinese(DayOfWeek dayOfWeek, string prefix)
        {
            switch (dayOfWeek)
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

        //yyyy/MM/dd HH:mm:ss:fff
    }
}