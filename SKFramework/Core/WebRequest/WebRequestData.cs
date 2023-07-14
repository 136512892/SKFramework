using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Networking
{
    /// <summary>
    /// 网络请求数据
    /// </summary>
    public class WebRequestData
    {
        /// <summary>
        /// 请求类型 GET/POST
        /// </summary>
        public WebRequestType RequestType { get; private set; }

        /// <summary>
        /// 表单
        /// </summary>
        public WWWForm WWWForm { get; private set; }

        /// <summary>
        /// 请求头
        /// </summary>
        public Dictionary<string, string> Headers { get; private set; }

        /// <summary>
        /// 上传数据
        /// </summary>
        public byte[] PostData { get; private set; }

        public static WebRequestData Allocate(WebRequestType requestType, Dictionary<string, string> headers = null)
        {
            WebRequestData data = new WebRequestData
            {
                RequestType = requestType,
                Headers = headers
            };
            return data;
        }
        public static WebRequestData Allocate(WebRequestType requestType, ContentType contentType, Dictionary<string, string> headers = null)
        {
            WebRequestData data = new WebRequestData
            {
                RequestType = requestType,
                Headers = headers ?? new Dictionary<string, string>()
            };
            switch (contentType)
            {
                case ContentType.JSON:
                    data.Headers.Add("Content-Type", "application/json");
                    break;
                case ContentType.X_WWW_FORM_URLENCODED:
                    data.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    break;
                default:
                    break;
            }
            return data;
        }
        public static WebRequestData Allocate(WebRequestType requestType, byte[] postData, ContentType contentType, Dictionary<string, string> headers = null)
        {
            WebRequestData data = new WebRequestData
            {
                RequestType = requestType,
                PostData = postData,
                Headers = headers ?? new Dictionary<string, string>()
            };
            switch (contentType)
            {
                case ContentType.JSON:
                    data.Headers.Add("Content-Type", "application/json");
                    break;
                case ContentType.X_WWW_FORM_URLENCODED:
                    data.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    break;
                default:
                    break;
            }
            return data;
        }
        public static WebRequestData Allocate(WebRequestType requestType, WWWForm form, ContentType contentType, Dictionary<string, string> headers = null)
        {
            WebRequestData data = new WebRequestData
            {
                RequestType = requestType,
                WWWForm = form,
                Headers = headers ?? new Dictionary<string, string>()
            };
            switch (contentType)
            {
                case ContentType.JSON:
                    data.Headers.Add("Content-Type", "application/json");
                    break;
                case ContentType.X_WWW_FORM_URLENCODED:
                    data.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    break;
                default:
                    break;
            }
            return data;
        }
    }
}