using System;
using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// 网络接口配置文件
    /// </summary>
    [CreateAssetMenu]
    public class WebInterfaceProfile : ScriptableObject
    {
        public WebInterface[] data;

        public WebInterface this[int index]
        {
            get
            {
                return data[index];
            }
        }

        public WebInterface this[string name]
        {
            get
            {
                return Array.Find(data, m => m.name == name);
            }
        }
    }
}