namespace SK.Framework
{
    /// <summary>
    /// 可开关物体接口
    /// </summary>
    public interface ISwitchableObject
    {
        SwitchState State { get; }

        void Switch();

        void Open();

        void Close();
    }
}