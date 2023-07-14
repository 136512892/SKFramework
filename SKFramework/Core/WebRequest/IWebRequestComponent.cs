using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace SK.Framework.Networking
{
    public interface IWebRequestComponent 
    {
        /// <summary>
        /// 发起网络请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="webRequestType">请求类型 GET/POST请求</param>
        /// <param name="headers">请求头</param>
        /// <param name="onSuccess">请求成功回调事件</param>
        /// <param name="onFailure">请求失败回调事件</param>
        void Send(string url, WebRequestType webRequestType, Dictionary<string, string> headers, Action<DownloadHandler> onSuccess, Action<string> onFailure);

        /// <summary>
        /// 发起网络请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="webRequestType">请求类型 GET/POST请求</param>
        /// <param name="contentType">CONTENT-TYPE</param>
        /// <param name="headers">请求头</param>
        /// <param name="onSuccess">请求成功回调事件</param>
        /// <param name="onFailure">请求失败回调事件</param>
        void Send(string url, WebRequestType webRequestType, ContentType contentType, Dictionary<string, string> headers, Action<DownloadHandler> onSuccess, Action<string> onFailure);

        /// <summary>
        /// 发起网络请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="webRequestType">请求类型 GET/POST请求</param>
        /// <param name="contentType">CONTENT-TYPE</param>
        /// <param name="postData">上传数据</param>
        /// <param name="headers">请求头</param>
        /// <param name="onSuccess">请求成功回调事件</param>
        /// <param name="onFailure">请求失败回调事件</param>
        void Send(string url, WebRequestType webRequestType, ContentType contentType, byte[] postData, Dictionary<string, string> headers, Action<DownloadHandler> onSuccess, Action<string> onFailure);

        /// <summary>
        /// 发起网络请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="webRequestType">请求类型 GET/POST请求</param>
        /// <param name="contentType">CONTENT-TYPE</param>
        /// <param name="form">表单</param>
        /// <param name="headers">请求头</param>
        /// <param name="onSuccess">请求成功回调事件</param>
        /// <param name="onFailure">请求失败回调事件</param>
        void Send(string url, WebRequestType webRequestType, ContentType contentType, WWWForm form, Dictionary<string, string> headers, Action<DownloadHandler> onSuccess, Action<string> onFailure);
    }
}