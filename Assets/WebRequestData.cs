using System;
using System.Collections.Generic;

#region 首页数据
/// <summary>
/// 首页数据响应数据结构
/// </summary>
[Serializable]
public class HomeDataResponse
{
    /// <summary>
    /// 状态码 0成功 -1系统异常
    /// </summary>
    public int code;
    /// <summary>
    /// 首页数据
    /// </summary>
    public Data data;
    /// <summary>
    /// 描述信息
    /// </summary>
    public string message;

    [Serializable]
    public class Data 
    {
        /// <summary>
        /// 告警日志列表
        /// </summary>
        public Alarm[] alarmList;
        /// <summary>
        /// 电池信息
        /// </summary>
        public Battery battery;
        /// <summary>
        /// 车辆信息
        /// </summary>
        public Car car;
        /// <summary>
        /// 环境检测
        /// </summary>
        public Environment environment;
        /// <summary>
        /// 气体检测
        /// </summary>
        public Gas gas;
        /// <summary>
        /// 巡检任务
        /// </summary>
        public PatrolTask patrolTask;
        /// <summary>
        /// 任务统计
        /// </summary>
        public TaskStatistics taskStatistics;
        /// <summary>
        /// 视频
        /// </summary>
        public Video video;

        [Serializable]
        public class Alarm
        {
            /// <summary>
            /// 报警码
            /// </summary>
            public string alarmCode;
            /// <summary>
            /// 报警信息
            /// </summary>
            public string alarmMessage;
            /// <summary>
            /// 设备
            /// </summary>
            public string deviceName;
            /// <summary>
            /// 项目
            /// </summary>
            public string projectName;
            /// <summary>
            /// 上报时间
            /// </summary>
            public string reportTime;
        }

        [Serializable]
        public class Battery
        {
            /// <summary>
            /// 电流
            /// </summary>
            public string ammeter;
            /// <summary>
            /// 当前时间
            /// </summary>
            public string dateTime;
            /// <summary>
            /// 续航里程
            /// </summary>
            public string endurance;
            /// <summary>
            /// 是否正在充电
            /// </summary>
            public string isCharging;
            /// <summary>
            /// 累计里程
            /// </summary>
            public string odometer;
            /// <summary>
            /// 剩余电量
            /// </summary>
            public string residualFuel;
            /// <summary>
            /// 电池温度
            /// </summary>
            public string temperature;
            /// <summary>
            /// 电池电压
            /// </summary>
            public string voltage;
        }

        [Serializable]
        public class Car
        {
            /// <summary>
            /// 方向
            /// </summary>
            public string altitude;
            /// <summary>
            /// 车头方向 正东为0度 逆时针0-360度
            /// </summary>
            public string azimuth;
            /// <summary>
            /// 刹车力度 (0-100)%
            /// </summary>
            public string brake;
            /// <summary>
            /// 当前时间
            /// </summary>
            public string dateTime;
            /// <summary>
            /// 驾驶模式 Standby、Manual-手动、Auto-自动
            /// </summary>
            public string driveMode;
            /// <summary>
            /// 纬度
            /// </summary>
            public string latitude;
            /// <summary>
            /// 经度
            /// </summary>
            public string longitude;
            /// <summary>
            /// 车辆编号
            /// </summary>
            public string number;
            /// <summary>
            /// 上报时间
            /// </summary>
            public string reportTime;
            /// <summary>
            /// 档位 R=0、P=1、N=2、D=3
            /// </summary>
            public string shift;
            /// <summary>
            /// 速度 单位km/h
            /// </summary>
            public string speed;
            /// <summary>
            /// 前轮摆角
            /// </summary>
            public string steerAngle;
            /// <summary>
            /// 任务状态 0空闲 1执行任务中
            /// </summary>
            public string taskState;
            /// <summary>
            /// 油门开度 (0-100)%
            /// </summary>
            public string throttle;
        }

        [Serializable]
        public class Environment
        {
            /// <summary>
            /// 大气压强
            /// </summary>
            public string atmosPressure;
            /// <summary>
            /// 倾倒状态
            /// </summary>
            public string dumpState;
            /// <summary>
            /// 湿度
            /// </summary>
            public string humidity;
            /// <summary>
            /// 光照强度
            /// </summary>
            public string lightIntensity;
            /// <summary>
            /// 雨量
            /// </summary>
            public string rainfall;
            /// <summary>
            /// 温度
            /// </summary>
            public string temperature;
            /// <summary>
            /// 风向
            /// </summary>
            public string windDirection;
            /// <summary>
            /// 风速
            /// </summary>
            public string windSpeed;
        }

        [Serializable]
        public class Gas
        {
            /// <summary>
            /// 一氧化碳
            /// </summary>
            public string co;
            /// <summary>
            /// 二氧化氮
            /// </summary>
            public string no2;
            /// <summary>
            /// 臭氧
            /// </summary>
            public string ozone;
            /// <summary>
            /// PM10
            /// </summary>
            public string pm10;
            /// <summary>
            /// PM2.5
            /// </summary>
            public string pm2d5;
            /// <summary>
            /// 二氧化硫
            /// </summary>
            public string so2;
        }

        [Serializable]
        public class PatrolTask
        {
            /// <summary>
            /// 执行车辆
            /// </summary>
            public string carName;
            /// <summary>
            /// 途径点名称列表
            /// </summary>
            public string[] pointNameList;
            /// <summary>
            /// 任务名称
            /// </summary>
            public string taskName;
            /// <summary>
            /// 任务状态
            /// </summary>
            public string taskState;
            /// <summary>
            /// 任务类型
            /// </summary>
            public string taskType;
        }

        [Serializable]
        public class TaskStatistics
        {
            /// <summary>
            /// 执行中数量
            /// </summary>
            public int executingCount;
            /// <summary>
            /// 空闲数量
            /// </summary>
            public int idleCount;
            /// <summary>
            /// 维修数量
            /// </summary>
            public int repairCount;
            /// <summary>
            /// 5G车总数
            /// </summary>
            public int totalCarCount;
            /// <summary>
            /// 任务总数
            /// </summary>
            public int totalTaskCount;
        }

        [Serializable]
        public class Video
        {
            /// <summary>
            /// 巡检视频URL（后面）
            /// </summary>
            public string backPatrolUrl;
            /// <summary>
            /// 巡检视频URL（前面）
            /// </summary>
            public string frontPatrolUrl;
            /// <summary>
            /// 实时监控URL（前面）
            /// </summary>
            public string liveUrl;
        }
    }
}
#endregion

#region 环境/气体数据监测统计(按小时)
[Serializable]
public class HourStatisticsRequest
{
    /// <summary>
    /// 日期 格式20200426
    /// </summary>
    public string day;
    /// <summary>
    /// 类型 1温度
    /// </summary>
    public int type;

    public HourStatisticsRequest()
    {
        day = string.Format("{0:yyyyMMdd}", DateTime.Now);
        type = 1;
    }

    public HourStatisticsRequest(string day, int type)
    {
        this.day = day;
        this.type = type;
    }
}

[Serializable]
public class HourStatisticsResponse
{
    public int code;

    public Data[] data;

    public string message;

    [Serializable]
    public class Data
    {
        public string[] categories;

        public Series[] series;

        [Serializable]
        public class Series
        {
            public Data[] data;

            public string name;
        }
    }
}
#endregion

#region 行为感知
/// <summary>
/// 行为感知
/// </summary>
public class ActionPerceptionResponse
{
    public int code;

    public Data data;

    public string message;

    [Serializable]
    public class Data
    {
        public Alarm[] alarmList; 

        public Device[] deviceList;

        public Location location;

        public Room[] roomList;

        [Serializable]
        public class Alarm
        {
            public string alarmType;

            public string reportTime;

            public string roomName;
        }
        [Serializable]
        public class Device
        {
            public string name;

            public string roomName;

            public string state;
        }
        [Serializable]
        public class Location
        {
            public string flatImageUrl;

            public string targets;
        }
        [Serializable]
        public class Room
        {
            public string name;

            public string state;
        }
    }
}
#endregion