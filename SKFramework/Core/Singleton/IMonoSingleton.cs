namespace SK.Framework
{
    /// <summary>
    /// Mono类型单例接口
    /// </summary>
    public interface IMonoSingleton : ISingleton
    {
        bool IsDontDestroyOnLoad { get; }
    }
}