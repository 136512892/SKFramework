namespace SK.Framework
{
    public interface IInput
    {
        bool BeginListening();

        bool StopListening();

        void Reset();
    }
}