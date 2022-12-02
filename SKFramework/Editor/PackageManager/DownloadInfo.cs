using System.Net;

namespace SK.Framework
{
    public class DownloadInfo
    {
        public WebResponse response;

        public float progress;

        public float Length
        {
            get
            {
                return response != null ? response.ContentLength : 0f;
            }
        }

        public DownloadInfo(WebResponse response)
        {
            this.response = response;
        }
    }
}