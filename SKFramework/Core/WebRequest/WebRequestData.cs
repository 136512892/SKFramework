using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Networking
{
    public class WebRequestData
    {
        public WebRequestType RequestType { get; private set; }

        public WWWForm WWWForm { get; private set; }

        public Dictionary<string, string> headers;

        public byte[] PostData { get; private set; }

        public static WebRequestData Allocate(WebRequestType requestType, ContentType contentType, Dictionary<string, string> headers)
        {
            WebRequestData data = new WebRequestData();
            data.RequestType = requestType;
            data.headers = headers == null ? new Dictionary<string, string>() : headers;
            switch (contentType)
            {
                case ContentType.JSON:
                    data.headers.Add("Content-Type", "application/json");
                    break;
                case ContentType.X_WWW_FORM_URLENCODED:
                    data.headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    break;
                default:
                    break;
            }
            return data;
        }

        public static WebRequestData Allocate(WebRequestType requestType, byte[] postData, ContentType contentType, Dictionary<string, string> headers)
        {
            WebRequestData data = new WebRequestData();
            data.RequestType = requestType;
            data.PostData = postData;
            data.headers = headers == null ? new Dictionary<string, string>() : headers;
            switch (contentType)
            {
                case ContentType.JSON:
                    data.headers.Add("Content-Type", "application/json");
                    break;
                case ContentType.X_WWW_FORM_URLENCODED:
                    data.headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    break;
                default:
                    break;
            }
            return data;
        }

        public static WebRequestData Allocate(WebRequestType requestType, WWWForm form, ContentType contentType, Dictionary<string, string> headers)
        {
            WebRequestData data = new WebRequestData();
            data.RequestType = requestType;
            data.WWWForm = form;
            data.headers = headers == null ? new Dictionary<string, string>() : headers;
            switch (contentType)
            {
                case ContentType.JSON:
                    data.headers.Add("Content-Type", "application/json");
                    break;
                case ContentType.X_WWW_FORM_URLENCODED:
                    data.headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    break;
                default:
                    break;
            }
            return data;
        }
    }
}