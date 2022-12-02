namespace SK.Framework.Timer
{
    public interface ITimer 
    {
        bool IsCompleted { get; }

        bool IsPaused { get; }

        void Launch();

        void Pause();

        void Resume();

        void Stop();

        bool Execute();
    }
}