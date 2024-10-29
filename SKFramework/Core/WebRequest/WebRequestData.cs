/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Networking
{
    public class WebRequestData
    {
        public WebRequestType requestType { get; private set; }

        public WWWForm wwwForm { get; private set; }

        public Dictionary<string, string> headers { get; private set; }

        public byte[] postData { get; private set; }

        public static WebRequestData Allocate(WebRequestType requestType, Dictionary<string, string> headers = null)
        {
            WebRequestData data = new WebRequestData
            {
                requestType = requestType,
                headers = headers
            };
            return data;
        }
        public static WebRequestData Allocate(WebRequestType requestType, ContentType contentType, Dictionary<string, string> headers = null)
        {
            WebRequestData data = new WebRequestData
            {
                requestType = requestType,
                headers = headers ?? new Dictionary<string, string>()
            };
            AddHeaderWithContentType(data.headers, contentType);
            return data;
        }
        public static WebRequestData Allocate(WebRequestType requestType, byte[] postData, ContentType contentType, Dictionary<string, string> headers = null)
        {
            WebRequestData data = new WebRequestData
            {
                requestType = requestType,
                postData = postData,
                headers = headers ?? new Dictionary<string, string>()
            };
            AddHeaderWithContentType(data.headers, contentType);
            return data;
        }
        public static WebRequestData Allocate(WebRequestType requestType, WWWForm form, ContentType contentType, Dictionary<string, string> headers = null)
        {
            WebRequestData data = new WebRequestData
            {
                requestType = requestType,
                wwwForm = form,
                headers = headers ?? new Dictionary<string, string>()
            };
            AddHeaderWithContentType(data.headers, contentType);
            return data;
        }

        private static void AddHeaderWithContentType(Dictionary<string, string> headers, ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.JSON:
                    headers.Add("Content-Type", "application/json");
                    break;
                case ContentType.X_WWW_FORM_URLENCODED:
                    headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    break;
                default:
                    break;
            }
        }
    }
}