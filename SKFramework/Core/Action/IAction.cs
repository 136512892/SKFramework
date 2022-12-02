namespace SK.Framework.Actions
{
    public interface IAction
    {
        bool Invoke();

        void Reset();
    }
}