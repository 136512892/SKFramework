namespace SK.Framework.ObjectPool
{
    public interface IPoolable
    {
        bool IsRecycled { get; set; }

        void OnRecycled();
    } 
}