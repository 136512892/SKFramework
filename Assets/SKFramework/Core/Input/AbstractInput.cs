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
            if (!isListening && IsValid)
            {
                Register();
                isListening = true;
                return true;
            }
            return false;
        }
        public bool StopListening()
        {
            if (isListening)
            {
                Unregister();
                isListening = false;
                return true;
            }
            return false;
        }
        public void Reset()
        {
            Value = default;
        }
    }
}