namespace SK.Framework.Events
{
    public abstract class EventArgs
    {
        public abstract int ID { get; }

        public virtual void OnInvoke() { }
    }
}