using System;

namespace SK.Framework
{
    public static class BoolExtension 
    {
        /// <summary>
        /// 如果bool值为true 则执行事件
        /// </summary>
        /// <param name="action">事件</param>
        public static bool Execute(this bool self, Action action)
        {
            if (self == true)
            {
                action();
            }
            return self;
        }
        /// <summary>
        /// 根据bool值执行Action<bool>类型事件
        /// </summary>
        /// <param name="action">事件</param>
        public static bool Execute(this bool self, Action<bool> action)
        {
            action(self);
            return self;
        }
        /// <summary>
        /// bool值为true则执行第一个事件 否则执行第二个事件
        /// </summary>
        public static bool Execute(this bool self, Action actionIfTrue, Action actionIfFalse)
        {
            if (self == true)
            {
                actionIfTrue();
            }
            else
            {
                actionIfFalse();
            }
            return self;
        }
    }
}