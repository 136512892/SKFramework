using System;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Debugger
{
    [Serializable]
    public class ConsoleWindow : IDebuggerWIndow
    {
        //日志列表
        private List<ConsoleItem> logs;
        //列表滚动视图
        private Vector2 listScroll;
        //详情滚动视图
        private Vector2 detailScroll;
        //普通日志数量
        private int infoCount;
        //告警日志数量
        private int warnCount;
        //错误日志数量
        private int errorCount;
        //是否显示普通日志
        [SerializeField] private bool showInfo = true;
        //是否显示告警日志
        [SerializeField] private bool showWarn = true;
        //是否显示错误日志
        [SerializeField] private bool showError = true;
        //当前选中的日志项
        private ConsoleItem currentSelected;
        //是否显示日志时间
        [SerializeField] private bool showTime = true;
        //最大缓存数量
        [SerializeField] private int maxCacheCount = 100;

        private string searchContent;

        public int WarnCount { get { return warnCount; } }
        public int ErrorCount { get { return errorCount;} }

        public void OnInitilization()
        {
            logs = new List<ConsoleItem>();
            Application.logMessageReceived += OnLogMessageReceived;
        }
        public void OnTermination()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
            logs?.Clear();
            logs = null;
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType logType)
        {
            var item = new ConsoleItem(DateTime.Now, logType, condition, stackTrace);
            if (logType == LogType.Log) infoCount++;
            else if (logType == LogType.Warning) warnCount++;
            else errorCount++;
            logs.Add(item);
            if (logs.Count > maxCacheCount)
            {
                logs.RemoveAt(0);
            }
        }

        public void OnWindowGUI()
        {
            OnTopGUI();
            OnListGUI();
            OnDetailGUI();
        }

        private void OnTopGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear", GUILayout.Width(50f)))
            {
                logs.Clear();
                infoCount = 0;
                warnCount = 0;
                errorCount = 0;
                currentSelected = null;
            }
            showTime = GUILayout.Toggle(showTime, "ShowTime", GUILayout.Width(80f));

            searchContent = GUILayout.TextField(searchContent, GUILayout.ExpandWidth(true));

            GUI.contentColor = showInfo ? Color.white : Color.grey;
            showInfo = GUILayout.Toggle(showInfo, string.Format("Info [{0}]", infoCount), GUILayout.Width(60f));
            GUI.contentColor = showWarn ? Color.white : Color.grey;
            showWarn = GUILayout.Toggle(showWarn, string.Format("Warn [{0}]", warnCount), GUILayout.Width(65f));
            GUI.contentColor = showError ? Color.white : Color.grey;
            showError = GUILayout.Toggle(showError, string.Format("Error [{0}]", errorCount), GUILayout.Width(65f));
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
        }

        private void OnListGUI()
        {
            GUILayout.BeginVertical("Box", GUILayout.Height(Screen.height * .3f));
            listScroll = GUILayout.BeginScrollView(listScroll);
            for (int i = logs.Count - 1; i >= 0; i--)
            {
                var temp = logs[i];
                if (!string.IsNullOrEmpty(searchContent) && !temp.message.ToLower().Contains(searchContent.ToLower())) continue;
                bool show = false;
                switch (temp.type)
                {
                    case LogType.Log: if (showInfo) show = true; break;
                    case LogType.Warning: if (showWarn) show = true; GUI.contentColor = Color.yellow; break;
                    case LogType.Error:
                    case LogType.Assert:
                    case LogType.Exception: if (showError) show = true; GUI.contentColor = Color.red; break;
                }
                if (show)
                {
                    if (GUILayout.Toggle(currentSelected == temp, showTime ? temp.brief : temp.message))
                    {
                        currentSelected = temp;
                    }
                }
                GUI.contentColor = Color.white;
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void OnDetailGUI()
        {
            GUILayout.BeginVertical("Box", GUILayout.ExpandHeight(true));
            detailScroll = GUILayout.BeginScrollView(detailScroll);
            if (currentSelected != null)
            {
                GUILayout.Label(currentSelected.detail);
            }
            GUILayout.EndScrollView();
            GUILayout.FlexibleSpace();
            GUI.enabled = currentSelected != null;
            if (GUILayout.Button("Copy", GUILayout.Height(20f)))
            {
                GUIUtility.systemCopyBuffer = currentSelected.detail;
            }
            GUILayout.EndVertical();
        }
    }
}
