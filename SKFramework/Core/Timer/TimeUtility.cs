using System;

namespace SK.Framework.Timer
{
    public class TimeUtility
    {
        /// <summary>
        /// 根据时间单位转化为秒数
        /// 例如传入(2,TimeUnit.Hour) 将返回7200秒
        /// </summary>
        /// <param name="v"></param>
        /// <param name="timeUnit"></param>
        /// <returns></returns>
        public static float Convert2Seconds(float v, TimeUnit timeUnit) 
        {
            switch (timeUnit)
            {
                case TimeUnit.Millsecond: return v * .001f;
                case TimeUnit.Minute: return v * 60f;
                case TimeUnit.Hour: return v * 3600f;
                case TimeUnit.Day: return v * 3600f * 24f;
                default: return v;
            }
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="dt">时间</param>
        /// <returns>时间戳</returns>
        public static double GetTimeStamp(DateTime dt)
        {
            TimeSpan ts = dt.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return ts.TotalMilliseconds;
        }
        /// <summary>
        /// 将秒数转化为HH:mm:ss格式字符串
        /// </summary>
        /// <param name="seconds">秒数</param>
        /// <returns></returns>
        public static string ToStandardTimeFormat(float seconds)
        {
            int second = (int)seconds;
            int hour = second / 3600;
            int minute = second % 3600 / 60;
            second = second % 3600 % 60;
            return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
        }
        /// <summary>
        /// 将秒数转化为mm:ss格式字符串
        /// </summary>
        /// <param name="seconds">秒数</param>
        /// <returns></returns>
        public static string ToMSTimeFormat(float seconds)
        {
            int s = (int)seconds;
            int minutes = s / 60;
            s %= 60;
            return string.Format("{0:D2}:{1:D2}", minutes, s);
        }
        /// <summary>
        /// 将秒数转化为HH:mm:ss:fff格式字符串
        /// </summary>
        /// <param name="seconds">秒数</param>
        /// <returns></returns>
        public static string ToHMSFTimeFormat(float seconds)
        {
            int millseconds = (int)(seconds * 1000);
            int hour = millseconds / 3600000;
            int minute = millseconds % 3600000 / 60000;
            int second = millseconds % 3600000 % 60000 / 1000;
            millseconds = millseconds % 3600000 % 60000 % 1000;
            return string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", hour, minute, second, millseconds);
        }
        /// <summary>
        /// 将秒数转化为mm:ss:fff格式字符串
        /// </summary>
        /// <param name="seconds">秒数</param>
        /// <returns></returns>
        public static string ToMSFTimeFormat(float seconds)
        {
            int millseconds = (int)(seconds * 1000);
            int minute = millseconds % 3600000 / 60000;
            int second = millseconds % 3600000 % 60000 / 1000;
            millseconds = millseconds % 3600000 % 60000 % 1000;
            return string.Format("{0:D2}:{1:D2}:{2:D3}", minute, second, millseconds);
        }
    }
}