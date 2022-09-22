namespace SK.Framework.Message
{
    /// <summary>
    /// 消息接口
    /// </summary>
    public interface IMessage { }

    public class Message<T> : IMessage
    {
        public T content;
    }
    public class Message<T1, T2> : IMessage
    {
        public T1 content1;
        public T2 content2;
    }
    public class Message<T1, T2, T3> : IMessage
    {
        public T1 content1;
        public T2 content2;
        public T3 content3;
    }
    public class Message<T1, T2, T3, T4> : IMessage
    {
        public T1 content1;
        public T2 content2;
        public T3 content3;
        public T4 content4;
    }
    public class Message<T1, T2, T3, T4, T5> : IMessage
    {
        public T1 content1;
        public T2 content2;
        public T3 content3;
        public T4 content4;
        public T5 content5;
    }
}