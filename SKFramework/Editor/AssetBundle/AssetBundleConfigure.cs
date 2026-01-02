/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using Object = UnityEngine.Object;

namespace SK.Framework.Resource
{
    public class AssetBundleConfigure : EditorWindow
    {
        [MenuItem("SKFramework/Resource/Asset Bundle")]
        public static void Open()
        {
            GetWindow<AssetBundleConfigure>("Asset Bundle").Show();
        }

        private bool m_IsInit;
        private Vector2 m_LScrollPosition, m_RScrollPosition;
        private float m_SplitterLPos, m_SplitterRPos, m_SplitterRBPos;
        private bool m_IsSplitterLDragging, m_IsSplitterRDragging, m_IsSplitterRBDragging;
        private List<AssetBundleEditorInfo> m_AssetBundleList;
        private AssetBundleEditorInfo m_SelectedAssetBundleInfo;
        private string m_SelectedAssetPath;
        private string[] m_AssetBundleDependencies;
        private string[] m_Dependencies;
        private const float m_TypeHeaderWidth = 48f;
        private const float m_MemorySizeHeaderWidth = 100f;
        private Vector2 m_AssetBundleDetailScroll;
        private Vector2 m_AssetDetailScroll;
        private string m_SearchAssetBundle;
        private string m_SearchAssetPath;
        private readonly Dictionary<Type, Texture> m_IconMap = new Dictionary<Type, Texture>();
        private bool m_IsBuilderOpened = false;
        private AnimFloat m_BuilderAnimFloat;
        private AssetBundleBuilder m_Builder;
        private bool m_IsDependenciesOpened = false;
        private int m_DependenciesCountPerPage = 10;
        private int m_DependenciesCurrentPage = 1;

        private void OnEnable()
        {
            m_SelectedAssetBundleInfo = null;
            m_AssetBundleDependencies = null;
            m_SelectedAssetPath = null;
            m_Dependencies = null;
            RefreshAssetBundles();
            m_IsBuilderOpened = false;
            m_IsDependenciesOpened = false;
            m_BuilderAnimFloat = new AnimFloat(0f, Repaint);
        }
        private void OnGUI()
        {
            if (!m_IsInit)
            {
                m_IsInit = true;
                m_SplitterLPos = position.width * .35f;
            }

            GUILayout.BeginHorizontal();
            OnLeftGUI();
            DrawSplitLine(2f, false, ref m_SplitterLPos, ref m_IsSplitterLDragging);
            OnRightGUI();
            if (m_BuilderAnimFloat.target != 0f)
                DrawSplitLine(2f, false, ref m_SplitterRPos, ref m_IsSplitterRDragging,
                    0f, .5f, true);
            OnBuilderGUI();
            GUILayout.EndHorizontal();
        }
        private void OnDisable()
        {
            m_AssetBundleList = null;
            m_SelectedAssetBundleInfo = null;
            m_AssetBundleDependencies = null;
            m_Dependencies = null;
            m_SelectedAssetPath = null;
            m_SearchAssetBundle = null;
            m_SearchAssetPath = null;
            if (m_Builder != null)
            {
                m_Builder.OnDisable();
                m_Builder = null;
            }
            m_BuilderAnimFloat.valueChanged.RemoveAllListeners();
            m_BuilderAnimFloat = null;
        }

        private void OnLeftGUI()
        {
            m_LScrollPosition = GUILayout.BeginScrollView(m_LScrollPosition, GUILayout.Width(m_SplitterLPos),
                GUILayout.MaxWidth(m_SplitterLPos), GUILayout.MinWidth(m_SplitterLPos));
            GUILayout.BeginHorizontal();
            OnAssetBundleSearchGUI();
            OnAssetBundleDropdownMenuGUI();
            GUILayout.EndHorizontal();
            DrawSplitLine(2f, true);
            OnAssetBundleListGUI();
            DragDropObject2AssetBundleListRectCheck();
            DrawSplitLine(2f, true);
            OnAssetBundleDetailGUI();
            GUILayout.EndScrollView();
        }
        private void OnAssetBundleSearchGUI()
        {
            m_SearchAssetBundle = GUILayout.TextField(m_SearchAssetBundle, EditorStyles.toolbarSearchField);
            //当点击鼠标且鼠标位置不在输入框中时 取消控件的聚焦
            if (Event.current.type == EventType.MouseDown
                && !GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                GUI.FocusControl(null);
                Repaint();
            }
        }
        private void OnAssetBundleDropdownMenuGUI()
        {
            if (GUILayout.Button(GUIContent.none, EditorStyles.toolbarDropDown, GUILayout.Width(18f)))
            {
                GenericMenu gm = new GenericMenu();
                gm.AddItem(new GUIContent("Add New AssetBundle"), false, AddNewAssetBundle);
                gm.AddItem(new GUIContent("Remove Unused AssetBundles"), false, AssetDatabase.RemoveUnusedAssetBundleNames);
                gm.AddItem(new GUIContent("Refresh List"), false, () =>
                {
                    m_SelectedAssetBundleInfo = null;
                    m_AssetBundleDependencies = null;
                    m_SelectedAssetPath = null;
                    m_Dependencies = null;
                    RefreshAssetBundles();
                });
                gm.AddSeparator(string.Empty);
                gm.AddItem(new GUIContent("Clear All AssetBundles"), false, () =>
                {
                    if (EditorUtility.DisplayDialog("警告", "是否确认清除所有的AssetBundles？", "确认", "取消"))
                    {
                        string[] assetBundles = AssetDatabase.GetAllAssetBundleNames();
                        for (int i = 0; i < assetBundles.Length; i++)
                        {
                            AssetBundleUtility.DeleteAssetBundle(assetBundles[i], false);
                        }
                        m_SelectedAssetBundleInfo = null;
                        m_AssetBundleDependencies = null;
                        m_SelectedAssetPath = null;
                        m_Dependencies = null;
                        RefreshAssetBundles();
                    }
                });
                gm.ShowAsContext();
                gm.ShowAsContext();
            }
        }
        private void OnAssetBundleListGUI()
        {
            if (m_AssetBundleList.Count != 0)
            {
                for (int i = 0; i < m_AssetBundleList.Count; i++)
                {
                    AssetBundleEditorInfo info = m_AssetBundleList[i];
                    if (!string.IsNullOrEmpty(m_SearchAssetBundle)
                        && !info.name.ToLower().Contains(m_SearchAssetBundle.ToLower())) continue;

                    GUILayout.BeginHorizontal(m_SelectedAssetBundleInfo == info
                        ? "MeTransitionSelectHead" : "ProjectBrowserHeaderBgTop", GUILayout.Height(20f));
                    GUILayout.Label(info.name);
                    GUILayout.EndHorizontal();

                    if (Event.current.type == EventType.MouseDown)
                    {
                        Rect rect = GUILayoutUtility.GetLastRect();
                        Vector2 mousePosition = Event.current.mousePosition;
                        if (rect.Contains(mousePosition))
                        {
                            OnAssetBundleListItemMouseDown(info, rect);
                            Event.current.Use();
                        }
                    }
                }
            }
        }
        private void OnAssetBundleDetailGUI()
        {
            m_AssetBundleDetailScroll = GUILayout.BeginScrollView(m_AssetBundleDetailScroll,
                GUILayout.Height(100f), GUILayout.MaxHeight(100f), GUILayout.MinHeight(100f));
            if (m_SelectedAssetBundleInfo != null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Count:", EditorStyles.boldLabel, GUILayout.Width(45f));
                GUILayout.Label(m_SelectedAssetBundleInfo.assets.Count.ToString());
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Memory Size:", EditorStyles.boldLabel, GUILayout.Width(90f));
                GUILayout.Label(m_SelectedAssetBundleInfo.memorySizeFormat);
                GUILayout.EndHorizontal();
                if (m_AssetBundleDependencies != null)
                {
                    GUILayout.Label("Dependencies:", EditorStyles.boldLabel);
                    for (var i = 0; i < m_AssetBundleDependencies.Length; i++)
                    {
                        GUILayout.Label(m_AssetBundleDependencies[i]);
                    }
                }
            }
            GUILayout.EndScrollView();
        }
        private void RefreshAssetBundles(bool repaint = false)
        {
            string[] assetBundeNames = AssetDatabase.GetAllAssetBundleNames();
            m_AssetBundleList = new List<AssetBundleEditorInfo>();
            for (int i = 0; i < assetBundeNames.Length; i++)
                m_AssetBundleList.Add(new AssetBundleEditorInfo(assetBundeNames[i]));
            if (repaint)
                Repaint();
        }
        private void OnAssetBundleListItemMouseDown(AssetBundleEditorInfo info, Rect rect)
        {
            if (Event.current.button == 0)
            {
                if (m_SelectedAssetBundleInfo != info)
                {
                    m_SelectedAssetBundleInfo = info;
                    m_SelectedAssetPath = null;
                    m_Dependencies = null;
                    m_AssetBundleDependencies = AssetDatabase.GetAssetBundleDependencies(m_SelectedAssetBundleInfo.name, true);
                    Repaint();
                }
            }
            else if (Event.current.button == 1)
            {
                GenericMenu gm = new GenericMenu();
                gm.AddItem(new GUIContent("Rename"), false,
                    () => PopupWindow.Show(rect, new AssetBundleRenamePopupWindowContent(info)));
                gm.AddItem(new GUIContent("Delete"), false,
                    () => DeleteAssetBundle(info));
                gm.ShowAsContext();
            }
        }
        private void DeleteAssetBundle(AssetBundleEditorInfo info)
        {
            if (AssetBundleUtility.DeleteAssetBundle(info.name))
            {
                if (info == m_SelectedAssetBundleInfo)
                {
                    m_SelectedAssetBundleInfo = null;
                    m_AssetBundleDependencies = null;
                    m_SearchAssetPath = null;
                }
                m_AssetBundleList.Remove(info);
                Repaint();
            }
        }
        private void AddNewAssetBundle()
        {
            var instance = new AssetBundleEditorInfo(string.Format("ab{0}",
                DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            m_AssetBundleList.Add(instance);
            m_SelectedAssetBundleInfo = instance;
            m_AssetBundleDependencies = AssetDatabase.GetAssetBundleDependencies(m_SelectedAssetBundleInfo.name, true);
            Repaint();
        }

        private void OnRightGUI()
        {
            m_RScrollPosition = GUILayout.BeginScrollView(m_RScrollPosition, GUILayout.ExpandWidth(true));
            GUILayout.BeginHorizontal(GUILayout.Height(20f));
            OnAssetSearchGUI();
            DrawSplitLine(2f, false);
            bool open = GUILayout.Toggle(m_IsDependenciesOpened, "Dependencies", GUILayout.Width(100f));
            if (open != m_IsDependenciesOpened)
            {
                m_IsDependenciesOpened = open;
            }

            open = GUILayout.Toggle(m_IsBuilderOpened, "Builder", GUILayout.Width(65f));
            if (open != m_IsBuilderOpened)
            {
                m_IsBuilderOpened = open;
                m_BuilderAnimFloat.target = m_IsBuilderOpened ? (position.width - m_SplitterLPos) * .5f : 0f;
                if (m_IsBuilderOpened)
                {
                    m_Builder = new AssetBundleBuilder();
                    m_Builder.OnEnable(this);
                }
                else
                {
                    m_Builder.OnDisable();
                    m_Builder = null;
                }
            }
            GUILayout.EndHorizontal();
            DrawSplitLine(2f, true);
            OnHeaderGUI();
            OnAssetListGUI();
            DragDropObject2AssetListRectCheck();
            DrawSplitLine(2f, true, ref m_SplitterRBPos, ref m_IsSplitterRBDragging, .2f, .8f, true);
            OnAssetDetailGUI();
            GUILayout.EndScrollView();
        }
        private void OnAssetSearchGUI()
        {
            m_SearchAssetPath = GUILayout.TextField(m_SearchAssetPath, EditorStyles.toolbarSearchField);
            if (Event.current.type == EventType.MouseDown
                && !GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                GUI.FocusControl(null);
                Repaint();
            }
        }
        private void OnHeaderGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Height(20f), GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Type", EditorStyles.toolbarDropDown, GUILayout.Width(m_TypeHeaderWidth)))
            {
                if (m_SelectedAssetBundleInfo != null)
                {
                    Type[] types = m_SelectedAssetBundleInfo.assets.Select(
                        m => AssetDatabase.GetMainAssetTypeAtPath(m.path)).Distinct().ToArray();
                    GenericMenu gm = new GenericMenu();
                    for (int i = 0; i < types.Length; i++)
                    {
                        Type type = types[i];
                        string typeName = type.Name;
                        gm.AddItem(new GUIContent(typeName), false, () =>
                        {
                            m_SelectedAssetBundleInfo.assets = m_SelectedAssetBundleInfo.assets.OrderBy(
                                m => AssetDatabase.GetMainAssetTypeAtPath(m.path) != type).ToList();
                        });
                    }
                    gm.ShowAsContext();
                }
            }
            if (GUILayout.Button("Asset Path", EditorStyles.toolbarDropDown, GUILayout.ExpandWidth(true)))
            {
                GenericMenu gm = new GenericMenu();
                gm.AddItem(new GUIContent("Name ↑"), false, () =>
                {
                    if (m_SelectedAssetBundleInfo != null)
                        m_SelectedAssetBundleInfo.assets = m_SelectedAssetBundleInfo.assets.OrderBy(m => m.path).ToList();
                });
                gm.AddItem(new GUIContent("Name ↓"), false, () =>
                {
                    if (m_SelectedAssetBundleInfo != null)
                        m_SelectedAssetBundleInfo.assets = m_SelectedAssetBundleInfo.assets.OrderByDescending(m => m.path).ToList();
                });
                gm.ShowAsContext();
            }
            if (GUILayout.Button("Memory Size", EditorStyles.toolbarDropDown, GUILayout.Width(m_MemorySizeHeaderWidth)))
            {
                GenericMenu gm = new GenericMenu();
                gm.AddItem(new GUIContent("Size ↑"), false, () =>
                {
                    if (m_SelectedAssetBundleInfo != null)
                        m_SelectedAssetBundleInfo.assets = m_SelectedAssetBundleInfo.assets.OrderBy(m => m.memorySize).ToList();
                });
                gm.AddItem(new GUIContent("Size ↓"), false, () =>
                {
                    if (m_SelectedAssetBundleInfo != null)
                        m_SelectedAssetBundleInfo.assets = m_SelectedAssetBundleInfo.assets.OrderByDescending(m => m.memorySize).ToList();
                });
                gm.ShowAsContext();
            }
            GUILayout.EndHorizontal();
        }
        private void OnAssetListGUI()
        {
            if (m_SelectedAssetBundleInfo != null)
            {
                if (m_SelectedAssetBundleInfo.assets.Count != 0)
                {
                    for (int i = 0; i < m_SelectedAssetBundleInfo.assets.Count; i++)
                    {
                        var assetInfo = m_SelectedAssetBundleInfo.assets[i];
                        if (!string.IsNullOrEmpty(m_SearchAssetPath)
                            && !assetInfo.path.ToLower().Contains(m_SearchAssetPath.ToLower())) continue;
                        GUILayout.BeginHorizontal(m_SelectedAssetPath == assetInfo.path
                            ? "MeTransitionSelectHead" : "ProjectBrowserHeaderBgTop", GUILayout.Height(20f));
                        Type type = AssetDatabase.GetMainAssetTypeAtPath(assetInfo.path);
                        if (!m_IconMap.TryGetValue(type, out Texture icon))
                        {
                            icon = AssetPreview.GetMiniTypeThumbnail(type);
                            m_IconMap.Add(type, icon);
                        }
                        GUILayout.Label(icon, GUILayout.Width(m_TypeHeaderWidth), GUILayout.Height(18f));
                        GUILayout.Label(assetInfo.path, GUILayout.ExpandWidth(true));
                        GUILayout.Label(assetInfo.memorySizeFormat, GUILayout.Width(m_MemorySizeHeaderWidth - 5f));
                        GUILayout.EndHorizontal();
                        if (Event.current.type == EventType.MouseDown)
                        {
                            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                            {
                                OnAssetListItemMouseDown(assetInfo.path);
                                Event.current.Use();
                            }
                        }
                        else if (Event.current.type == EventType.KeyDown)
                        {
                            if (Event.current.keyCode == KeyCode.Delete)
                            {
                                DeleteAssetFromAssetBundle(m_SelectedAssetPath);
                                Event.current.Use();
                            }
                        }
                    }
                }
            }
        }
        private void OnAssetDetailGUI()
        {
            if (m_IsDependenciesOpened)
            {
                if (m_SelectedAssetPath != null)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Dependencies:", EditorStyles.boldLabel);
                    GUILayout.FlexibleSpace();
                    int countPerPage = EditorGUILayout.IntField(m_DependenciesCountPerPage, GUILayout.Width(35f));
                    if (countPerPage != m_DependenciesCountPerPage)
                    {
                        countPerPage = Mathf.Max(0, countPerPage);
                        if (countPerPage != m_DependenciesCountPerPage)
                        {
                            m_DependenciesCountPerPage = countPerPage;
                            m_DependenciesCurrentPage = 1;
                        }
                    }
                    GUILayout.Label("Per Page", GUILayout.Width(70f));

                    int pages = m_Dependencies.Length / m_DependenciesCountPerPage;
                    if (m_Dependencies.Length % m_DependenciesCountPerPage != 0)
                        pages++;
                    if (GUILayout.Button("<", GUILayout.Width(20f)))
                    {
                        if (m_DependenciesCurrentPage > 1)
                            m_DependenciesCurrentPage--;
                    }
                    GUILayout.Label(string.Format("{0}/{1}", m_DependenciesCurrentPage, pages), GUILayout.Width(30f));
                    if (GUILayout.Button(">", GUILayout.Width(20f)))
                    {
                        if (m_DependenciesCurrentPage < pages)
                            m_DependenciesCurrentPage++;
                    }
                    GUILayout.EndHorizontal();

                    m_AssetDetailScroll = GUILayout.BeginScrollView(m_AssetDetailScroll,
                        GUILayout.Height(m_SplitterRBPos), GUILayout.MaxHeight(m_SplitterRBPos), GUILayout.MinHeight(m_SplitterRBPos));
                    int start = (m_DependenciesCurrentPage - 1) * m_DependenciesCountPerPage;
                    for (int i = start; i < m_DependenciesCountPerPage * m_DependenciesCurrentPage; i++)
                    {
                        if (i >= m_Dependencies.Length)
                            break;
                        string dep = m_Dependencies[i];
                        Type type = AssetDatabase.GetMainAssetTypeAtPath(dep);
                        if (!m_IconMap.TryGetValue(type, out Texture icon))
                        {
                            icon = AssetPreview.GetMiniTypeThumbnail(type);
                            m_IconMap.Add(type, icon);
                        }
                        GUILayout.BeginHorizontal("ProjectBrowserHeaderBgTop");
                        GUILayout.Label(icon, GUILayout.Width(18f), GUILayout.Height(18f));
                        GUILayout.Label(m_Dependencies[i]);
                        GUILayout.EndHorizontal();
                        if (Event.current.type == EventType.MouseDown
                            && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                            EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(dep));
                    }
                    GUILayout.EndScrollView();
                }
            }
        }
        private void OnAssetListItemMouseDown(string assetPath)
        {
            if (Event.current.button == 0)
            {
                m_SelectedAssetPath = assetPath;
                m_Dependencies = AssetDatabase.GetDependencies(m_SelectedAssetPath);
                Repaint();
                EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(m_SelectedAssetPath));
            }
            else if (Event.current.button == 1)
            {
                Event.current.Use();
                GenericMenu gm = new GenericMenu();
                gm.AddItem(new GUIContent("Delete"), false, () => DeleteAssetFromAssetBundle(assetPath));
                gm.ShowAsContext();
            }
        }
        private void DeleteAssetFromAssetBundle(string assetPath)
        {
            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
            if (importer != null)
            {
                importer.assetBundleName = null;
                m_SelectedAssetPath = null;
                m_Dependencies = null;
                m_SelectedAssetBundleInfo.DeleteAsset(assetPath);
                m_AssetBundleDependencies = AssetDatabase.GetAssetBundleDependencies(m_SelectedAssetBundleInfo.name, true);
                Repaint();
            }
        }

        private void DrawSplitLine(float thickness, bool horizontal)
        {
            if (horizontal)
                GUILayout.Box(string.Empty, "EyeDropperHorizontalLine", GUILayout.Height(thickness),
                    GUILayout.MaxHeight(thickness), GUILayout.MinHeight(thickness), GUILayout.ExpandWidth(true));
            else
                GUILayout.Box(string.Empty, "EyeDropperVerticalLine", GUILayout.Width(thickness),
                    GUILayout.MaxWidth(thickness), GUILayout.MinWidth(thickness), GUILayout.ExpandHeight(true));
        }
        private void DrawSplitLine(float thickness, bool horizontal, ref float splitterPos,
            ref bool isDragging, float minPercent = .2f, float maxPercent = .8f, bool dirInvert = false)
        {
            DrawSplitLine(thickness, horizontal);
            Rect rect = GUILayoutUtility.GetLastRect();

            if (Event.current != null)
            {
                EditorGUIUtility.AddCursorRect(rect, horizontal ? MouseCursor.ResizeVertical : MouseCursor.ResizeHorizontal);
                switch (Event.current.rawType)
                {
                    //开始拖拽分割线
                    case EventType.MouseDown:
                        isDragging = rect.Contains(Event.current.mousePosition);
                        break;
                    case EventType.MouseDrag:
                        if (isDragging)
                        {
                            splitterPos += (horizontal ? Event.current.delta.y : Event.current.delta.x)
                                * (dirInvert ? -1f : 1f);
                            splitterPos = Mathf.Clamp(
                                splitterPos,
                                (horizontal ? position.height : position.width) * minPercent,
                                (horizontal ? position.height : position.width) * maxPercent);
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

        private void DragDropObject2AssetBundleListRectCheck()
        {
            DragDropObject2RectCheck(out IEnumerable<Object> objs);
            if (objs != null)
            {
                bool flag = true;
                foreach (Object obj in objs)
                    flag &= AssetBundleUtility.CreateAssetBundle4Object(obj);
                if (flag)
                    RefreshAssetBundles(true);
            }
        }
        private void DragDropObject2AssetListRectCheck()
        {
            DragDropObject2RectCheck(out IEnumerable<Object> objs);
            if (objs != null && m_SelectedAssetBundleInfo != null)
            {
                foreach (Object obj in objs)
                {
                    string path = AssetDatabase.GetAssetPath(obj);
                    AssetImporter importer = AssetImporter.GetAtPath(path);
                    if (importer != null)
                    {
                        if (!string.IsNullOrEmpty(importer.assetBundleName))
                        {
                            var target = m_AssetBundleList.Find(m => m.name == importer.assetBundleName);
                            target?.DeleteAsset(path);
                        }
                        importer.assetBundleName = m_SelectedAssetBundleInfo.name;
                        m_SelectedAssetBundleInfo.AddAsset(path);
                    }
                }
                m_AssetBundleDependencies = AssetDatabase.GetAssetBundleDependencies(m_SelectedAssetBundleInfo.name, true);
                Repaint();
            }
        }
        private void DragDropObject2RectCheck(out IEnumerable<Object> objs)
        {
            objs = null;
            GUILayout.Label(GUIContent.none, GUILayout.ExpandHeight(true));
            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                switch (Event.current.type)
                {
                    case EventType.DragUpdated:
                        bool flag = DragAndDrop.objectReferences.OfType<Object>().Any();
                        DragAndDrop.visualMode = flag ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.Rejected;
                        Event.current.Use();
                        Repaint();
                        break;
                    case EventType.DragPerform:
                        objs = DragAndDrop.objectReferences.OfType<Object>();
                        break;
                    case EventType.MouseDown:
                        if (Event.current.button == 1)
                        {
                            GenericMenu gm = new GenericMenu();
                            gm.AddItem(new GUIContent("Add New AssetBundle"), false, () => AddNewAssetBundle());
                            gm.ShowAsContext();
                            Event.current.Use();
                        }
                        break;
                }
            }
        }

        private void OnBuilderGUI()
        {
            GUILayout.BeginVertical(GUILayout.Width(m_BuilderAnimFloat.value + m_SplitterRPos));
            if (m_IsBuilderOpened)
                m_Builder.OnGUI();
            GUILayout.EndVertical();
        }
    }
}