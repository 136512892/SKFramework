using UnityEngine;

namespace SK.Framework
{
    public abstract class AbstractInput<K, V> : IInput
    {
        [SerializeField] protected K key;

        protected bool isListening;

        public virtual bool IsValid
        {
            get
            {
                return true;
            }
        }
        public K Key
        {
            get
            {
                return key;
            }
            set
            {
                if (!IsEqual(key, value))
                {
                    if (isListening)
                    {
                        Unregister();
                    }
                    key = value;
                    if (isListening && IsValid)
                    {
                        Register();
                    }
                }
            }
        }
        public V Value;

        public AbstractInput() { }
        public AbstractInput(K key)
        {
            this.key = key;
        }

        protected abstract bool IsEqual(K k1, K k2);
        protected abstract void Register();
        protected abstract void Unregister();

        public bool BeginListening()
        {
            if (isListening)
            {
                Log.Info(Module.Input, string.Format("[{0}]已经在监听状态", key));
                return false;
            }
            if (!IsValid)
            {
                Log.Error(Module.Input, string.Format("开启监听失败 无效值[{0}]", key));
                return false;
            }
            Register();
            isListening = true;
            return true;
        }
        public bool StopListening()
        {
            if (!isListening)
            {
                Log.Info(Module.Input, string.Format("终止监听失败 [{0}]不在监听状态", key));
                return false;
            }
            Unregister();
            isListening = false;
            return true;
        }
        public void Reset()
        {
            Value = default;
        }
    }
}