using UnityEngine;

namespace SK.Framework
{
    public static class ObjectExtension 
    {
        public static T As<T>(this Object self) where T : Object
        {
            return self as T;
        }
        public static T Instantiate<T>(this T self) where T : Object
        {
            return Object.Instantiate(self);
        }
        public static T DontDestroyOnLoad<T>(this T self) where T : Object
        {
            Object.DontDestroyOnLoad(self);
            return self;
        }
        public static void Destroy<T>(this T self) where T : Object
        {
            if(null != self)
            {
                Object.Destroy(self);
            }
        }
        public static void Destroy<T>(this T self, float delay) where T : Object
        {
            if(null != self)
            {
                Object.Destroy(self, delay);
            }
        }
    }
}