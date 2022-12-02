using System;

namespace SK.Framework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PackageAttribute : Attribute
    {
        /// <summary>
        /// 资源包名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 资源包版本
        /// </summary>
        public string Version { get; private set; }

        public PackageAttribute(string name, string version)
        {
            Name = name;
            Version = version;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", Name, Version);
        }
    }
}