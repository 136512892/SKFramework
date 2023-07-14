using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEditor;
using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// 资源包管理器
    /// </summary>
    public class PackageManager : EditorWindow
    {
        [MenuItem("SKFramework/Package Manager", priority = 9999)]
        public static void Open()
        {
            var window = GetWindow<PackageManager>();
            window.titleContent = EditorGUIUtility.TrTextContentWithIcon("Package Manager", "Package Manager");
            window.minSize = new Vector2(600f, 300f);
            window.Show();
        }

        //服务器IP地址
        private const string ipAddress = "http://139.224.100.175:80";
        //manifest.json文件
        private const string manifest = "Library/manifest.dat";
        //资源包列表
        private List<PackageInfoDetail> packages;
        //资源包字典
        private Dictionary<string, List<PackageInfoDetail>> dic;
        //折叠栏字典
        private Dictionary<string, bool> foldout;
        //用于存储资源包版本是否可升级的字典
        private Dictionary<string, bool> updatable;
        //检索内容
        private string searchContent;
        //左侧内容滚动
        private Vector2 leftScroll;
        //右侧内容滚动
        private Vector2 rightScroll;
        //左侧内容宽度
        private float leftWidth = 280f;
        //搜索栏宽度
        private const float searchFieldWidth = 200f;
        //最新更新时间
        private string lastUpdateTime;
        //用于存储最新更新时间的Key值
        private const string lastUpdateTimeKey = "Packages Last Update Time";
        //当前所选中的资源包
        private PackageInfoDetail selectedPackage;
        //标题样式
        private GUIStyle titleStyle;
        //版本样式
        private GUIStyle versionStyle;
        //粗体样式
        private GUIStyle boldLabelStyle;
        //依赖引用列表样式
        private GUIStyle dependenciesStyle;
        //依赖项折叠栏
        private bool dependenciesFoldOut = true;
        //GUIContents
        private class Contents
        {
            //已安装
            public static GUIContent installed = new GUIContent("√", "This package is installed.");
            //可升级
            public static GUIContent updatable = new GUIContent("↑", "A newer version of this package is available.");
        }
        //下载中集合
        private readonly Dictionary<string, DownloadInfo> loadingDic = new Dictionary<string, DownloadInfo>();

        private Rect splitterRect;
        private bool isDragging;

        private void OnEnable()
        {
            //如果不存在manifest.json文件
            //直接发起网络请求获取资源包信息
            if (!File.Exists(manifest))
            {
                UpdatePackagesInfo();
            }
            else
            {
                Build();
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
                GUILayout.BeginVertical(GUILayout.ExpandHeight(true), GUILayout.MaxWidth(5f));
                GUILayout.Box(string.Empty, "EyeDropperVerticalLine", GUILayout.ExpandHeight(true));
                GUILayout.EndVertical();
                splitterRect = GUILayoutUtility.GetLastRect();

                //右侧
                GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                OnRightGUI();
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            if (Event.current != null)
            {
                //光标
                EditorGUIUtility.AddCursorRect(splitterRect, MouseCursor.ResizeHorizontal);
                switch (Event.current.rawType)
                {
                    //开始拖拽分割线
                    case EventType.MouseDown:
                        isDragging = splitterRect.Contains(Event.current.mousePosition);
                        break;
                    case EventType.MouseDrag:
                        if (isDragging)
                        {
                            leftWidth += Event.current.delta.x;
                            //限制其最大最小值
                            leftWidth = Mathf.Clamp(leftWidth, position.width * .3f, position.width * .8f);
                            Repaint();
                        }
                        break;
                    //结束拖拽分割线
                    case EventType.MouseUp:
                        if (isDragging)
                            isDragging = false;
                        break;
                }
            }
        }

        //样式初始化
        private void StyleInit()
        {
            titleStyle = new GUIStyle(GUI.skin.label) { fontSize = 18, fontStyle = FontStyle.Bold };
            versionStyle = new GUIStyle(GUI.skin.label) { fontSize = 12, fontStyle = FontStyle.Bold };
            boldLabelStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
            dependenciesStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Italic };
        }
        //顶部GUI 菜单 检索内容
        private void OnTopGUI()
        {
            GUILayout.BeginHorizontal("Toolbar");
            //排序按钮
            GUI.enabled = packages != null;
            if (GUILayout.Button("Sort", "ToolbarDropDownLeft", GUILayout.Width(50f)))
            {
                GenericMenu gm = new GenericMenu();
                gm.AddItem(new GUIContent("Name ↓"), false, () => dic = dic.OrderBy(m => m.Key).ToDictionary(m => m.Key, m => m.Value));
                gm.AddItem(new GUIContent("Name ↑"), false, () => dic = dic.OrderByDescending(m => m.Key).ToDictionary(m => m.Key, m => m.Value));
                gm.AddItem(new GUIContent("Released Date ↓"), false, () => dic = dic.OrderBy(m => m.Value[0].releaseDate).ToDictionary(m => m.Key, m => m.Value));
                gm.AddItem(new GUIContent("Released Date ↑"), false, () => dic = dic.OrderByDescending(m => m.Value[0].releaseDate).ToDictionary(m => m.Key, m => m.Value));
                gm.ShowAsContext();
            }
            GUI.enabled = true;
            GUILayout.FlexibleSpace();
            //检索输入框
            searchContent = GUILayout.TextField(searchContent, "SearchTextField", GUILayout.Width(searchFieldWidth));
            //当点击鼠标且鼠标位置不在输入框中时 取消控件的聚焦
            if (Event.current.type == EventType.MouseDown && !GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                GUI.FocusControl(null);
                Repaint();
            }
            GUILayout.Space(10f);
            //点击该按钮打开博客链接
            if (GUILayout.Button(EditorGUIUtility.IconContent("_Help"), "toolbarbuttonRight", GUILayout.Width(25f)))
            {
                Application.OpenURL("https://blog.csdn.net/qq_42139931/article/details/125108284?spm=1001.2014.3001.5501");
            }
            GUILayout.EndHorizontal();
        }
        //左侧GUI 资源列表信息 最新更新时间
        private void OnLeftGUI()
        {
            //遍历资源包列表
            leftScroll = EditorGUILayout.BeginScrollView(leftScroll);
            if (packages != null)
            {
                foreach (var kv in dic)
                {
                    //获取列表中第一个版本的资源包信息
                    var package = kv.Value[0];
                    //如果检索输入框内容不为空 判断资源包的名称是否包含检索的内容
                    if (!string.IsNullOrEmpty(searchContent) && !package.name.ToLower().Contains(searchContent.ToLower())) continue;
                    //折叠栏
                    GUILayout.BeginHorizontal(selectedPackage == package ? "MeTransitionSelectHead" : "ProjectBrowserHeaderBgTop");
                    foldout[kv.Key] = EditorGUILayout.Foldout(foldout[kv.Key], package.name);
                    GUILayout.FlexibleSpace();
                    //版本信息
                    GUILayout.Label(package.version);
                    //已安装
                    if (package.isInstalled) GUILayout.Label(Contents.installed, GUILayout.Width(15f));
                    //可升级
                    else if (updatable[kv.Key]) GUILayout.Label(Contents.updatable, GUILayout.Width(15f));
                    //未安装
                    else GUILayout.Label(GUIContent.none, GUILayout.Width(15f));
                    GUILayout.EndHorizontal();
                    //鼠标点击选中当前项
                    if (Event.current.type == EventType.MouseDown && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                    {
                        selectedPackage = package;
                        Repaint();
                    }
                    //如果折叠栏为打开状态 展示其他版本信息
                    if (foldout[kv.Key])
                    {
                        if (kv.Value.Count > 1)
                        {
                            for (int i = 1; i < kv.Value.Count; i++)
                            {
                                package = kv.Value[i];
                                GUILayout.BeginHorizontal(selectedPackage == package ? "MeTransitionSelectHead" : "ProjectBrowserHeaderBgTop");
                                GUILayout.FlexibleSpace();
                                //版本信息
                                GUILayout.Label(package.version);
                                //是否已经安装
                                if (package.isInstalled)
                                {
                                    GUILayout.Label(Contents.installed, GUILayout.Width(12f));
                                }
                                GUILayout.EndHorizontal();
                                //鼠标点击选中当前项
                                if (Event.current.type == EventType.MouseDown && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                                {
                                    selectedPackage = package;
                                    Repaint();
                                }
                            }
                        }
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
                UpdatePackagesInfo();
                Repaint();
            }
            GUILayout.EndHorizontal();
        }
        //右侧GUI 资源详情 下载导入
        private void OnRightGUI()
        {
            rightScroll = EditorGUILayout.BeginScrollView(rightScroll);
            {
                if (selectedPackage != null)
                {
                    var package = selectedPackage;
                    //名称
                    GUILayout.Label(package.name, titleStyle);
                    //作者
                    GUILayout.Label(package.author);
                    //版本+发布日期
                    GUILayout.Label(string.Format("Version {0} - {1}", package.version, package.releaseDate), versionStyle);
                    //文档
                    if (!string.IsNullOrEmpty(package.documentationUrl))
                    {
                        EditorGUILayout.Space();
                        if (GUILayout.Button("Documentation", "wordwrapminibutton", GUILayout.Width(100f)))
                        {
                            //访问文档
                            Application.OpenURL(package.documentationUrl);
                        }
                    }
                    //分割线
                    EditorGUILayout.Space();
                    GUILayout.Box(string.Empty, "EyeDropperHorizontalLine", GUILayout.ExpandWidth(true), GUILayout.Height(1f));
                    EditorGUILayout.Space();
                    //介绍
                    GUILayout.Label(package.description);
                    GUILayout.Space(20f);
                    //折叠栏
                    GUILayout.BeginHorizontal("ToolbarBottom");
                    dependenciesFoldOut = EditorGUILayout.Foldout(dependenciesFoldOut, "Dependenceis", true);
                    GUILayout.EndHorizontal();
                    //如果折叠栏为打开状态
                    if (dependenciesFoldOut)
                    {
                        //依赖项
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Space(20f);
                            //Is using
                            GUILayout.BeginVertical(GUILayout.Width(60f));
                            GUILayout.Label("Is using", boldLabelStyle);
                            GUILayout.EndVertical();
                            //依赖项列表
                            GUILayout.BeginVertical();
                            if (package.dependencies != null && package.dependencies.Length > 0)
                            {
                                for (int i = 0; i < package.dependencies.Length; i++)
                                {
                                    var item = package.dependencies[i];
                                    string content = string.Format("{0} - {1}    {2}", item.name, item.version, item.isInstalled ? "(installed)" : string.Empty);
                                    GUILayout.Label(content, dependenciesStyle);
                                }
                            }
                            else GUILayout.Label("(None)", dependenciesStyle);
                            GUILayout.EndVertical();
                        }
                        GUILayout.EndHorizontal();

                        EditorGUILayout.Space();

                        //引用项
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Space(20f);
                            //Used by
                            GUILayout.BeginVertical(GUILayout.Width(60f));
                            GUILayout.Label("Used by", boldLabelStyle);
                            GUILayout.EndVertical();
                            //引用项列表
                            GUILayout.BeginVertical();
                            if (package.referencies != null && package.referencies.Length > 0)
                            {
                                for (int i = 0; i < package.referencies.Length; i++)
                                {
                                    var item = package.referencies[i];
                                    string content = string.Format("{0} - {1}", item.name, item.version);
                                    GUILayout.Label(content, dependenciesStyle);
                                }
                            }
                            else GUILayout.Label("(None)", dependenciesStyle);
                            GUILayout.EndVertical();
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUILayout.EndScrollView();

            GUILayout.FlexibleSpace();

            //底部分割线
            GUILayout.Box(string.Empty, "EyeDropperHorizontalLine", GUILayout.ExpandWidth(true), GUILayout.Height(1f));
            GUILayout.BeginHorizontal();
            GUILayout.Label(GUIContent.none);
            if (selectedPackage != null)
            {
                var package = selectedPackage;
                if (loadingDic.TryGetValue(package.name, out DownloadInfo info))
                {
                    GUILayout.Label(string.Format("Downloading... {0}%", info.progress));
                }
                else
                {
                    if (!package.isInstalled)
                    {
                        //首先判断是否有其他版本的该资源包已经被安装
                        var installed = dic[package.name].Find(m => m.isInstalled);
                        //如果没有 使用Install安装按钮
                        if (installed == null)
                        {
                            //安装按钮
                            if (GUILayout.Button("Install", GUILayout.Width(80f)))
                            {
                                InstallPackage(package);
                            }
                        }
                        //如果有其他版本 显示升级按钮
                        else
                        {
                            if (GUILayout.Button(string.Format("Update to {0}", package.version), GUILayout.Width(120f)))
                            {
                                //首先移除
                                ClearInstalled(installed);
                                //再升级安装
                                InstallPackage(package);
                            }
                        }
                    }
                    else
                    {
                        //移除按钮
                        if (GUILayout.Button("Remove", GUILayout.Width(80f)))
                        {
                            if (package.referencies != null && package.referencies.Length > 0)
                            {
                                if (package.referencies.Any(m => m.isInstalled))
                                {
                                    if (EditorUtility.DisplayDialog("提醒", "有其他工具包依赖于该项，是否确认将其移除？", "确认", "取消"))
                                    {
                                        RemovePackage(package);
                                    }
                                }
                                else RemovePackage(package);
                            }
                            else
                            {
                                RemovePackage(package);
                            }
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private void Build()
        {
            //获取所有类型 遍历判断是否包含PackageAttribute属性
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            //用于存储所有PackageAttribute属性
            List<PackageAttribute> attributes = new List<PackageAttribute>();
            foreach (Assembly assembly in assemblies)
            {
                var assemblyTypes = assembly.GetTypes();
                foreach (var type in assemblyTypes)
                {
                    var attribute = type.GetCustomAttribute<PackageAttribute>();
                    if (attribute != null)
                    {
                        attributes.Add(attribute);
                    }
                }
            }
            //打开文件
            using (FileStream fs = File.Open(manifest, FileMode.Open))
            {
                //反序列化
                BinaryFormatter bf = new BinaryFormatter();
                var deserialize = bf.Deserialize(fs);
                if (deserialize != null)
                    packages = deserialize as List<PackageInfoDetail>;
            }
            //初始化字典
            dic = new Dictionary<string, List<PackageInfoDetail>>();
            foldout = new Dictionary<string, bool>();
            updatable = new Dictionary<string, bool>();
            //遍历资源包列表
            for (int i = 0; i < packages.Count; i++)
            {
                var package = packages[i];
                //判断是否已经有相应的资源包信息 表示是否已经安装
                var target = attributes.Find(m => m.ToString() == package.ToString());
                package.isInstalled = target != null;
                //如果包含依赖项
                if (package.dependencies != null && package.dependencies.Length > 0)
                {
                    //遍历依赖项 查找其是否已经安装
                    for (int j = 0; j < package.dependencies.Length; j++)
                    {
                        var dp = package.dependencies[j];
                        var dpTarget = attributes.Find(m => m.ToString() == dp.ToString());
                        dp.isInstalled = dpTarget != null;
                    }
                }
            }
            //再次遍历 获取引用关系
            for (int i = 0; i < packages.Count; i++)
            {
                var package = packages[i];
                package.referencies = packages.Where(m => m.dependencies != null
                    && Array.Find(m.dependencies, m => m.ToString() == package.ToString()) != null).ToArray();
                //填充字典
                if (!dic.ContainsKey(package.name))
                {
                    dic.Add(package.name, new List<PackageInfoDetail>());
                    foldout.Add(package.name, false);
                    updatable.Add(package.name, false);
                }
                dic[package.name].Add(package);
            }
            //遍历字典 进行排序
            foreach (var kv in dic)
            {
                var list = kv.Value;
                list = list.OrderByDescending(m => m.version).ToList();
                //判断是否有可升级版本
                updatable[kv.Key] = list.Count > 1 && list.OrderBy(m => m.isInstalled).ToList()[0] != list.OrderBy(m => m.version).ToList()[0]
                    && list.Find(m => m.isInstalled) != null && list.Find(m => m.isInstalled) != list[0];
            }
        }
        //刷新资源包信息
        private void UpdatePackagesInfo()
        {
            //URL
            string url = string.Format("{0}/PackageManager/manifest.dat", ipAddress);
            //发起网络请求
            WebRequest request = WebRequest.Create(url);
            WebResponse webResponse = request.GetResponse();
            using (Stream stream = webResponse.GetResponseStream())
            {
                try
                {
                    using (FileStream fs = new FileStream(manifest, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = new byte[1024];
                        int count = 0;
                        while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fs.Write(buffer, 0, count);
                        }
                        //刷新最新更新时间并存储
                        lastUpdateTime = string.Format("Last Update {0}", DateTime.Now);
                        EditorPrefs.SetString(lastUpdateTimeKey, lastUpdateTime);
                        //重置
                        selectedPackage = null;
                    }
                }
                finally
                {
                    Build();
                }
            }
        }
        //安装资源包
        private void InstallPackage(PackageInfoDetail package)
        {
            //下载资源包本身
            InstallPackage(package.name, package.version);
            //下载资源包依赖项
            if (package.dependencies != null && package.dependencies.Length > 0)
            {
                for (int i = 0; i < package.dependencies.Length; i++)
                {
                    var dp = package.dependencies[i];
                    if(!dp.isInstalled)
                        InstallPackage(dp.name, dp.version);
                }
            }
        }
        //下载安装资源包
        private void InstallPackage(string name, string version)
        {
            //URL拼接
            string url = string.Format("{0}/PackageManager/Packages/{1}/{2}.unitypackage", ipAddress, name, version);
            //下载路径
            string path = string.Format("{0}/{1}-{2}.unitypackage", Application.dataPath, name, version);
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            float contentLength = response.ContentLength;
            loadingDic.Add(name, new DownloadInfo(response));
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
                            loadingDic[name].progress = Mathf.Round(totalDownloadBytes / contentLength * 100f);
                            //Debug.Log(string.Format("{0}-{1} 下载进度：{2}%", name, version, loadingDic[name].progress));
                        }
                    }
                    catch (Exception error)
                    {
                        Debug.LogError(error);
                    }
                    finally
                    {
                        loadingDic[name].response.Dispose();
                        loadingDic.Remove(name);
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
                //资源变更事件
                OnPackageChanged(name, version);
            }
        }
        //移除安装的资源包
        private void RemovePackage(PackageInfoDetail package)
        {
            string path = string.Format("{0}/SKFramework/Packages/{1}", Application.dataPath, package.name);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                string metaPath = string.Format("{0}.meta", path);
                if (File.Exists(metaPath))
                {
                    File.Delete(metaPath);
                }

                ////获取上层目录
                //DirectoryInfo parent = new DirectoryInfo(path).Parent;
                ////如果删除后上层目录已为空目录 将其删除
                //if (parent.GetFiles().Length == 0 && parent.GetDirectories().Length == 0)
                //{
                //    Directory.Delete(parent.FullName, true);
                //    File.Delete(string.Format("{0}.meta", parent.FullName));
                //}
            }
            else
            {
                Debug.Log(string.Format("<b><color=yellow>删除资源包[{0}]失败：无效路径-{1}</color></b>", package.name, path));
            }
            
            AssetDatabase.Refresh();
        }
        //清除已安装资源包内容
        private void ClearInstalled(PackageInfoDetail package)
        {
            string path = string.Format("{0}/SKFramework/Packages/{1}", Application.dataPath, package.name);
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
        }
        //资源包变更事件(版本切换)
        private void OnPackageChanged(string name, string version)
        {
            var list = dic[name];
            for (int i = 0; i < list.Count; i++)
            {
                var current = list[i];
                current.isInstalled = current.version == version;
            }
            updatable[name] = list.Count > 1 && list.OrderBy(m => m.isInstalled).ToList()[0] != list.OrderBy(m => m.version).ToList()[0]
                    && list.Find(m => m.isInstalled) != null && list.Find(m => m.isInstalled) != list[0];
        }
    }
}