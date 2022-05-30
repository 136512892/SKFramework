using System;

namespace SK.Framework
{
    public static class ClassExtension 
    {
        public static bool IsNull<T>(this T self) where T : class
        {
            return null == self;
        }
        public static bool Execute<T>(this T self, Action<T> action) where T : class
        {
            if(null != self)
            {
                action(self);
                return true;
            }
            return false;
        }
    }
}