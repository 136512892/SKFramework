using System;
using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// 响应结果为text文本类型的网络接口
    /// </summary>
    public class TextResponseWebInterface : AbstractWebInterface
    {
        private Action<string> callback;

        public override void SendWebRequest(params string[] args)
        {
            switch (method)
            {
                case WebRequestMethod.GET:
                    string url = GetUrl(args);
                    if (string.IsNullOrEmpty(url))
                    {
                        Debug.LogError(string.Format("调用网络接口{0}失败：参数数量不匹配", name));
                        return;
                    }
                    WebRequester.GET(url, callback);
                    break;
                case WebRequestMethod.POST:
                    string postData = null;
                    //第一个参数为postData
                    if (args.Length > 0)
                    {
                        postData = args[0];
                    }
                    //后面参数为请求头
                    if (args.Length > 1)
                    {
                        string[] headers = new string[args.Length - 1];
                        for (int i = 1; i < args.Length; i++)
                        {
                            headers[i - 1] = args[i];
                        }
                        WebRequester.POST(this.url, postData, callback, headers);
                        return;
                    }
                    WebRequester.POST(this.url, postData, callback);
                    break;
            }
        }

        public TextResponseWebInterface OnCompleted(Action<string> onCompleted)
        {
            callback = onCompleted;
            return this;
        }
    }
}