using System;

namespace SK.Framework
{
    public static class ClassExtension 
    {
        public static T Execute<T>(this T self, Action<T> action) where T : class
        {
            if (null != self)
            {
                action(self);
            }
            return self;
        }
    }
}