using System;

namespace SK.Framework
{
    public static class ClassExtension 
    {
        /// <summary>
        /// 如果对象不为null则执行事件
        /// </summary>
        /// <param name="action">事件</param>
        /// <returns>执行成功返回true 否则返回false</returns>
        public static bool Execute<T>(this T self, Action<T> action) where T : class
        {
            if (null != self)
            {
                action(self);
                return true;
            }
            return false;
        }
    }
}