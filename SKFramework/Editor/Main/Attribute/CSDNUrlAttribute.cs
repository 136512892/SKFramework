using System;

namespace SK.Framework
{
    /// <summary>
    /// CSDN链接
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CSDNUrlAttribute : Attribute
    {
        public string Url { get; private set; }

        public CSDNUrlAttribute(string url)
        {
            Url = url;
        }
    }
}