using System;

namespace SK.Framework
{
    /// <summary>
    /// 单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Singleton<T> where T : class, ISingleton
    {
        private static T instance;

        private static readonly object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if(null == instance)
                    {
                        instance = Activator.CreateInstance<T>();
                        instance.OnInit();
                    }
                }
                return instance;
            }
        }
        public static void Dispose()
        {
            instance = null;
        }
    }
}