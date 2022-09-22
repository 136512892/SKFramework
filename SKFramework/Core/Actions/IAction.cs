namespace SK.Framework.Actions
{
    /// <summary>
    /// 事件接口
    /// </summary>
    public interface IAction
    {
        bool Invoke();

        void Reset();
    }
}