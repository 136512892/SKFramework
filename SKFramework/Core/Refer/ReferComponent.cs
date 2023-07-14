using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Refer
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/Refer")]
    public class ReferComponent : MonoBehaviour, IReferComponent
    {
        //引用列表
        private readonly List<IRefer> referList = new List<IRefer>();

        private void Update()
        {
            for (int i = 0; i < referList.Count; i++)
            {
                referList[i]?.OnUpdate();
            }
        }

        /// <summary>
        /// 创建引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="referName">引用命名</param>
        /// <returns>引用</returns>
        public T Create<T>(string referName) where T : IRefer, new()
        {
            if (referList.Find(m => m.Name == referName) == null)
            {
                T refer = new T { Name = referName };
                refer.OnInitialization();
                referList.Add(refer);
                return refer;
            }
            return default;
        }
        public T Create<T>() where T : IRefer, new()
        {
            return Create<T>(typeof(T).Name);
        }

        /// <summary>
        /// 销毁引用
        /// </summary>
        /// <param name="referName">引用名称</param>
        /// <returns>true：销毁成功 false：销毁失败</returns>
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
        public bool Destroy<T>() where T : IRefer
        {
            return Destroy(typeof(T).Name);
        }

        /// <summary>
        /// 是否存在指定引用
        /// </summary>
        /// <param name="referName">引用名称</param>
        /// <returns>true：存在 false：不存在</returns>
        public bool IsExists(string referName)
        {
            return referList.FindIndex(m => m.Name == referName) != -1;
        }
        public bool IsExists<T>() where T : IRefer
        {
            return IsExists(typeof(T).Name);
        }

        /// <summary>
        /// 获取引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="referName">引用名称</param>
        /// <returns>引用</returns>
        public T Get<T>(string referName) where T : IRefer
        {
            var target = referList.Find(m => m.Name == referName);
            return target != null ? (T)target : default;
        }
        public T Get<T>() where T : IRefer
        {
            return Get<T>(typeof(T).Name);
        }

        /// <summary>
        /// 尝试获取引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="referName">引用名称</param>
        /// <param name="refer">引用</param>
        /// <returns>true：获取成功 false：获取失败</returns>
        public bool TryGet<T>(string referName, out T refer) where T : IRefer
        {
            int index = referList.FindIndex(m => m.Name == referName);
            if(index != -1)
            {
                refer = (T)referList[index];
                return true;
            }
            else
            {
                refer = default;
                return false;
            }
        }
        public bool TryGet<T>(out T refer) where T : IRefer
        {
            return TryGet(typeof(T).Name, out refer);
        }

        /// <summary>
        /// 获取或创建引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="referName">引用名称或引用命名</param>
        /// <returns>引用</returns>
        public T GetOrCreate<T>(string referName) where T : IRefer, new()
        {
            var refer = referList.Find(m => m.Name == referName);
            if (refer != null)
                return (T)refer;
            else
            {
                refer = new T() { Name = referName };
                refer.OnInitialization();
                referList.Add(refer);
                return (T)refer;
            }
        }
        public T GetOrCreate<T>() where T : IRefer, new()
        {
            return GetOrCreate<T>(typeof(T).Name);
        }
    }
}