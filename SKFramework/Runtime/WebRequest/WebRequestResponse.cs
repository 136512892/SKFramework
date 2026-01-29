/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

namespace SK.Framework.Networking
{
    public class WebRequestResponse
    {
        public long code;
        public string data;
        public string error;
        public byte[] bytes;

        public WebRequestResponse Copy(WebRequestResponse response)
        {
            code = response.code;
            data = response.data;
            error = response.error;
            bytes = response.bytes;
            return this;
        }
    }
}