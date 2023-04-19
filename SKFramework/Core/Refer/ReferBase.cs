namespace SK.Framework.Refer
{
    public abstract class ReferBase : IRefer
    {
        //被引用数量
        private int referCount;

        public int Count { get { return referCount; } }

        public virtual string Name { get; internal set; }

        public virtual void Apply()
        {
            referCount++;
            if (referCount == 1)
            {
                OnNotZero();
            }
        }

        public virtual void Release()
        {
            referCount--;
            if (referCount == 0)
            {
                OnZero();
            }
        }

        //初始化事件
        public virtual void OnInitialization() { }

        //销毁事件
        public virtual void OnDestroy() { }

        //引用数量变为0事件
        protected abstract void OnZero();
        
        //引用数量不再为0事件
        protected abstract void OnNotZero();
    }
}