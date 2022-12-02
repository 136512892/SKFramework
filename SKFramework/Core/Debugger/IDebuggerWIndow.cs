namespace SK.Framework.Debugger
{
    public interface IDebuggerWIndow
    {
        void OnInitilization();

        void OnWindowGUI();

        void OnTermination();
    }
}