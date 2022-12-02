namespace SK.Framework.Networking
{
    public class WebRequestResponse<T>
    {
        public int code;

        public string message;

        public T data;
    }
}