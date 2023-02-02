using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Networking
{
    public class WebRequestData
    {
        public WebRequestType RequestType { get; private set; }

        public WWWForm WWWForm { get; private set; }

        public Dictionary<string, string> Headers { get; private set; }

        public byte[] PostData { get; private set; }

        public static WebRequestData Allocate(WebRequestType requestType)
        {
            WebRequestData data = new WebRequestData();
            data.RequestType = requestType;
            return data;
        }

        public static WebRequestData Allocate(WebRequestType requestType, ContentType contentType, Dictionary<string, string> headers = null)
        {
            WebRequestData data = new WebRequestData();
            data.RequestType = requestType;
            data.Headers = headers == null ? new Dictionary<string, string>() : headers;
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
            WebRequestData data = new WebRequestData();
            data.RequestType = requestType;
            data.PostData = postData;
            data.Headers = headers == null ? new Dictionary<string, string>() : headers;
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
            WebRequestData data = new WebRequestData();
            data.RequestType = requestType;
            data.WWWForm = form;
            data.Headers = headers == null ? new Dictionary<string, string>() : headers;
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