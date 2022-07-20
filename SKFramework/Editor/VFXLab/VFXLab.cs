using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;

namespace SK.Framework
{
    /// <summary>
    /// 特效实验室
    /// </summary>
    public class VFXLab : EditorWindow
    {
        [MenuItem("SKFramework/VFX Lab", priority = 9998)]
        public static void Open()
        {
            var window = GetWindow<VFXLab>("VFX Lab");
            window.minSize = new Vector2(600f, 300f);
            window.Show();
        }

        //服务器IP地址
        private const string ipAddress = "http://1.13.194.97:80";
        //json文件
        private const string json = "vfx.json";
        //检索内容
        private string searchContent;
        //所有分组
        private string[] groups;
        //当前分组的索引值
        private int currentGroupIndex;
        //所有的特效
        private List<VFXInfo> vfxList;
        //用于展示的列表(过滤后的列表)
        private List<VFXInfo> displayList;
        //最新更新时间
        private string lastUpdateTime;
        //用于存储最新更新时间的Key值
        private const string lastUpdateTimeKey = "VFX Last Update Time";
        //当前选中项
        private VFXInfo selected;
        //左侧内容宽度
        private const float leftWidth = 150f;
        //左侧内容滚动
        private Vector2 leftScroll;
        //右侧内容滚动
        private Vector2 rightScroll;
        //标题样式
        private GUIStyle titleStyle;
        //日期样式
        private GUIStyle dateStyle;
        //开发环境样式
        private GUIStyle ideStyle;

        private void OnEnable()
        {
            //如果不存在vfx.json文件
            //直接发起网络请求获取资源包信息
            if (!File.Exists(json))
            {
                UpdateVFXInfo();
            }
            else
            {
                //读取vfx.json文件内容
                using (StreamReader sr = new StreamReader(json))
                {
                    string content = sr.ReadToEnd();
                    Init(content);
                }
            }
            //获取最新更新时间
            lastUpdateTime = EditorPrefs.GetString(lastUpdateTimeKey);
        }
        private void OnGUI()
        {
            if (titleStyle == null) StyleInit();

            OnTopGUI();
            GUILayout.BeginHorizontal();
            {
                //左侧
                GUILayout.BeginVertical(GUILayout.Width(leftWidth));
                OnLeftGUI();
                GUILayout.EndVertical();

                //分割线
                GUILayout.BeginVertical(GUILayout.ExpandHeight(true), GUILayout.MaxWidth(1f));
                GUILayout.Box(string.Empty, "EyeDropperVerticalLine", GUILayout.ExpandHeight(true));
                GUILayout.EndVertical();

                //右侧
                GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                OnRightGUI();
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
        //样式初始化
        private void StyleInit()
        {
            titleStyle = new GUIStyle(GUI.skin.label) { fontSize = 18, fontStyle = FontStyle.Bold };
            dateStyle = new GUIStyle(GUI.skin.label) { fontSize = 12, fontStyle = FontStyle.Bold };
            ideStyle = new GUIStyle(GUI.skin.label) { fontSize = 10, fontStyle = FontStyle.Italic };
        }
        //顶部GUI
        private void OnTopGUI()
        {
            GUILayout.BeginHorizontal("Toolbar");
            //分组
            int newIndex = EditorGUILayout.Popup(currentGroupIndex, groups, "ToolbarPopupLeft", GUILayout.Width(150f));
            if (newIndex != currentGroupIndex)
            {
                currentGroupIndex = newIndex;
                if (currentGroupIndex == 0) displayList = vfxList;
                else displayList = vfxList.Where(m => m.group == groups[currentGroupIndex]).ToList();
            }
            //排序
            if (GUILayout.Button("Sort", "ToolbarDropDownLeft", GUILayout.Width(50f)))
            {
                GenericMenu gm = new GenericMenu();
                gm.AddItem(new GUIContent("Name ↓"), false, () => displayList.OrderBy(m => m.name));
                gm.AddItem(new GUIContent("Name ↑"), false, () => displayList.OrderByDescending(m => m.name));
                gm.AddItem(new GUIContent("Released Date ↓"), false, () => displayList.OrderBy(m => m.releaseDate));
                gm.AddItem(new GUIContent("Released Date ↑"), false, () => displayList.OrderByDescending(m => m.releaseDate));
                gm.ShowAsContext();
            }
            GUILayout.Space(5f);
            //检索输入框
            searchContent = GUILayout.TextField(searchContent, "SearchTextField");
            //当点击鼠标且鼠标位置不在输入框中时 取消控件的聚焦
            if (Event.current.type == EventType.MouseDown && !GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                GUI.FocusControl(null);
                Repaint();
            }
            GUILayout.Space(5f);
            //点击该按钮打开博客链接
            if (GUILayout.Button(EditorGUIUtility.IconContent("_Help"), "toolbarbuttonRight", GUILayout.Width(25f)))
            {
                Application.OpenURL("https://blog.csdn.net/qq_42139931/article/details/125413193?spm=1001.2014.3001.5501");
            }
            GUILayout.EndHorizontal();
        }
        //左侧GUI 资源列表信息 最新更新时间
        private void OnLeftGUI()
        {
            //遍历资源包列表
            leftScroll = EditorGUILayout.BeginScrollView(leftScroll);
            if (displayList != null)
            {
                for (int i = 0; i < displayList.Count; i++)
                {
                    var vfx = displayList[i];
                    //如果检索输入框内容不为空 判断名称是否包含检索的内容
                    if (!string.IsNullOrEmpty(searchContent) && !vfx.name.ToLower().Contains(searchContent.ToLower())) continue;
                    GUILayout.BeginHorizontal(selected == vfx ? "MeTransitionSelectHead" : "ProjectBrowserHeaderBgTop");
                    GUILayout.Label(vfx.name);
                    GUILayout.EndHorizontal();
                    //鼠标点击选中当前项
                    if (Event.current.type == EventType.MouseDown && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                    {
                        selected = vfx;
                        Repaint();
                    }
                }
               
            }
            EditorGUILayout.EndScrollView();

            GUILayout.FlexibleSpace();

            //底部分割线
            GUILayout.Box(string.Empty, "EyeDropperHorizontalLine", GUILayout.ExpandWidth(true), GUILayout.Height(1f));
            GUILayout.BeginHorizontal();
            //最新更新时间
            GUILayout.Label(lastUpdateTime);
            //刷新按钮
            if (GUILayout.Button(EditorGUIUtility.IconContent("Refresh"), GUILayout.Width(30f)))
            {
                UpdateVFXInfo();
                Repaint();
            }
            GUILayout.EndHorizontal();
        }
        //右侧GUI 资源详情 下载导入
        private void OnRightGUI()
        {
            rightScroll = EditorGUILayout.BeginScrollView(rightScroll);
            {
                if (selected != null)
                {
                    //名称
                    GUILayout.Label(selected.name, titleStyle);
                    //作者
                    GUILayout.Label(selected.author);
                    //发布日期
                    GUILayout.Label(selected.releaseDate, dateStyle);

                    EditorGUILayout.Space();
                    GUILayout.BeginHorizontal();
                    if (!string.IsNullOrEmpty(selected.viewLink))
                    {
                        if(GUILayout.Button("View", "wordwrapminibutton", GUILayout.Width(50f)))
                        {
                            Application.OpenURL(selected.viewLink);
                        }
                    }
                    //文档
                    if (!string.IsNullOrEmpty(selected.documentationUrl))
                    {
                        if (GUILayout.Button("Documentation", "wordwrapminibutton", GUILayout.Width(100f)))
                        {
                            //访问文档
                            Application.OpenURL(selected.documentationUrl);
                        }
                    }
                    GUILayout.EndHorizontal();
                    //分割线
                    EditorGUILayout.Space();
                    GUILayout.Box(string.Empty, "EyeDropperHorizontalLine", GUILayout.ExpandWidth(true), GUILayout.Height(1f));
                    EditorGUILayout.Space();
                    //开发环境
                    GUILayout.Label(selected.ide, ideStyle);
                    //分割线
                    EditorGUILayout.Space();
                    GUILayout.Box(string.Empty, "EyeDropperHorizontalLine", GUILayout.ExpandWidth(true), GUILayout.Height(1f));
                    EditorGUILayout.Space();
                    //介绍
                    GUILayout.Label(selected.description);
                }
            }
            EditorGUILayout.EndScrollView();

            GUILayout.FlexibleSpace();

            //底部分割线
            GUILayout.Box(string.Empty, "EyeDropperHorizontalLine", GUILayout.ExpandWidth(true), GUILayout.Height(1f));
            GUILayout.BeginHorizontal();
            GUILayout.Label(GUIContent.none);
            if (selected != null)
            {
                //安装按钮
                if (GUILayout.Button("Import", GUILayout.Width(80f)))
                {
                    ImportVFX(selected);
                }
            }
            GUILayout.EndHorizontal();
        }
        //初始化
        private void Init(string content)
        {
            //反序列化
            vfxList = JsonConvert.DeserializeObject<List<VFXInfo>>(content);
            //默认按照名称排序 形成用于展示的列表
            displayList = vfxList.OrderBy(m => m.name).ToList();

            //用于缓存分组名称
            List<string> groupNames = new List<string>();
            //遍历
            for (int i = 0; i < vfxList.Count; i++)
            {
                var vfx = vfxList[i];
                if (!groupNames.Contains(vfx.group))
                {
                    groupNames.Add(vfx.group);
                }
            }
            //形成所有分组
            groups = new string[groupNames.Count + 1];
            groups[0] = "All";
            for (int i = 0; i < groupNames.Count; i++)
            {
                groups[i + 1] = groupNames[i];
            }
        }
        //刷新特效实验室信息
        private void UpdateVFXInfo()
        {
            //URL
            string url = string.Format("{0}/{1}", ipAddress, json);
            //发起网络请求
            WebRequest request = WebRequest.Create(url);
            WebResponse webResponse = request.GetResponse();
            using (Stream stream = webResponse.GetResponseStream())
            {
                //读取流
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    //读取内容
                    string content = reader.ReadToEnd();
                    //写入manifest.json文件
                    using (FileStream fs = new FileStream(json, FileMode.Create))
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(content);
                        fs.Write(bytes, 0, bytes.Length);
                    }
                    //刷新最新更新时间并存储
                    lastUpdateTime = string.Format("Last Update {0}", DateTime.Now);
                    EditorPrefs.SetString(lastUpdateTimeKey, lastUpdateTime);
                    //重置
                    selected = null;
                    //初始化
                    Init(content);
                }
            }
        }
        //下载导入特效
        private void ImportVFX(VFXInfo info)
        {
            //URL拼接
            string url = string.Format("{0}/VFX/{1}/{2}.unitypackage", ipAddress, info.group, info.name);
            //下载路径
            string path = string.Format("{0}/{1}-{2}.unitypackage", Application.dataPath, info.group, info.name);
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    try
                    {
                        byte[] bytes = new byte[1024];
                        int size = stream.Read(bytes, 0, bytes.Length);
                        long totalDownloadBytes = 0;
                        while (size > 0)
                        {
                            totalDownloadBytes += size;
                            fs.Write(bytes, 0, size);
                            size = stream.Read(bytes, 0, bytes.Length);
                            Debug.Log(string.Format("{0}-{1} 下载进度：{2}B", info.group, info.name, totalDownloadBytes));
                        }
                    }
                    catch (Exception error)
                    {
                        Debug.LogError(error);
                    }
                    finally
                    {
                        fs.Close();
                        stream.Close();
                    }
                }
            }
            if (File.Exists(path))
            {
                //下载完成 导入unitypackage包
                AssetDatabase.ImportPackage(path, false);
                //导入完成后 删除下载的文件
                File.Delete(path);
            }
        }
    }
}