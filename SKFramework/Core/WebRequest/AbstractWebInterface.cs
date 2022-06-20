using System.Text;

namespace SK.Framework
{
    /// <summary>
    /// 抽象网络接口
    /// </summary>
    public abstract class AbstractWebInterface
    {
        public string name;

        public string url;

        public string[] args;

        public WebRequestMethod method;

        public abstract void SendWebRequest(params string[] args);

        public string GetUrl(params string[] args)
        {
            if (this.args.Length != args.Length) return null;
            StringBuilder sb = new StringBuilder();
            sb.Append(url);
            if (args.Length > 0)
            {
                sb.Append(string.Format("?{0}={1}", this.args[0], args[0]));
            }
            if (args.Length > 1)
            {
                for (int i = 1; i < args.Length; i++)
                {
                    sb.Append(string.Format("&{0}={1}", this.args[i], args[i]));
                }
            }
            return sb.ToString();
        }
    }
}