using UnityEngine;
using SK.Framework;

public class WebRequestHandler : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this);

        //注册所有接口
        WebRequester.RegisterInterface<TextResponseWebInterface>("首页数据", out var homeData);
        WebRequester.RegisterInterface<TextResponseWebInterface>("环境气体数据监测统计", out var hourStatistics);
        WebRequester.RegisterInterface<TextResponseWebInterface>("行为感知", out var actionPerception);

        //添加接口调用的回调函数
        homeData.OnCompleted(response => Messenger.Publish(1001, response));
        hourStatistics.OnCompleted(response => Messenger.Publish(1002, response));
        actionPerception.OnCompleted(s => Debug.Log(s));

        //每5秒调用一次
        this.EverySeconds(5f, () =>
        {
            //调用接口、发起网络请求
            WebRequester.SendWebRequest("首页数据");
            WebRequester.SendWebRequest("环境气体数据监测统计", JsonUtility.ToJson(new HourStatisticsRequest()), RequestHeader.CONTENTTYPE_APPLICATIONJSON);
            WebRequester.SendWebRequest("行为感知");
        }).Launch();
    }
}