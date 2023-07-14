using System.Collections.Generic;

namespace SK.Framework.Refer
{
    /// <summary>
    /// 引用基类
    /// </summary>
    public abstract class ReferBase : IRefer
    {
        protected readonly List<object> applicants = new List<object>();

        /// <summary>
        /// 引用名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 被引用的数量
        /// </summary>
        public int Count { get; protected set; }

        /// <summary>
        /// 申请引用
        /// </summary>
        /// <param name="applicant">申请者</param>
        public virtual void Apply(object applicant = null)
        {
            if (applicant != null && !applicants.Contains(applicant))
                applicants.Add(applicant);
            if (++Count == 1)
                OnNotZero();
        }

        /// <summary>
        /// 释放引用
        /// </summary>
        /// <param name="applicant">申请者</param>
        public virtual void Release(object applicant = null)
        {
            if (applicant != null && applicants.Contains(applicant))
                applicants.Remove(applicant);
            if (--Count == 0)
                OnZero();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        public virtual void OnInitialization() { }

        /// <summary>
        /// 刷新事件
        /// </summary>
        public virtual void OnUpdate() 
        {
            for (int i = 0; i < applicants.Count; i++)
            {
                var applicant = applicants[i];
                if (applicant == null)
                {
                    applicants.RemoveAt(i);
                    i--;
                    if (--Count == 0)
                        OnZero();
                }
            }
        }

        /// <summary>
        /// 销毁事件
        /// </summary>
        public virtual void OnDestroy() { }

        //引用数量变为0事件
        protected abstract void OnZero();
        
        //引用数量不再为0事件
        protected abstract void OnNotZero();
    }
}