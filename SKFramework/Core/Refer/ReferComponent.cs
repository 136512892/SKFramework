using System;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Refer
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/Refer")]
    public class ReferComponent : MonoBehaviour
    {
        //引用列表
        private readonly List<ReferBase> referList = new List<ReferBase>();

        /// <summary>
        /// 创建引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="referName">引用命名</param>
        /// <returns>引用</returns>
        public T Create<T>(string referName = null) where T : ReferBase, new()
        {
            Type type = typeof(T);
            referName = string.IsNullOrEmpty(referName) ? type.Name : referName;
            if (referList.Find(m => m.Name == referName) == null)
            {
                T refer = Activator.CreateInstance(type) as T;
                refer.Name = referName;
                refer.OnInitialization();
                referList.Add(refer);
                return refer;
            }
            return null;
        }

        /// <summary>
        /// 销毁引用
        /// </summary>
        /// <param name="referName">引用名称</param>
        /// <returns>true：销毁成功  false：目标引用不存在，销毁失败</returns>
        public bool Destroy(string referName)
        {
            var targetRefer = referList.Find(m => m.Name == referName);
            if (targetRefer != null)
            {
                targetRefer.OnDestroy();
                referList.Remove(targetRefer);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 销毁引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="refer">引用</param>
        /// <returns>true：销毁成功  false：目标引用不存在，销毁失败</returns>
        public bool Destroy<T>(T refer) where T : ReferBase, new()
        {
            if (referList.Contains(refer))
            {
                refer.OnDestroy();
                referList.Remove(refer);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="referName">引用名称</param>
        /// <returns>引用</returns>
        public T GetRefer<T>(string referName = null) where T : ReferBase, new()
        {
            referName = string.IsNullOrEmpty(referName) ? typeof(T).Name : referName;
            var target = referList.Find(m => m.Name == referName);
            return target != null ? target as T : null;
        }
    }
}