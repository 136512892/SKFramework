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
            var window = GetWindow<AssetBundleConfigure>("Asset Bundle");
            window.minSize = new Vector2(300f, 200f);
            window.Show();
        }

        private List<AssetBundleProfile> m_Profiles;
        private AssetBundleProfile m_Profile;

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

        private bool m_ShowRedundancies = false;
        private readonly List<RedundancyInfo> m_RedundanciesList = new List<RedundancyInfo>();
        private RedundancyInfo m_SelectedRedundancyInfo;
        private Vector2 m_RedundanciesScroll;
        private string m_RedundanciesTargetBundleName = "common";
        private class RedundancyInfo
        {
            public string assetPath;
            public List<string> bundleNames;
        }

        private int m_DependenciesCountPerPage = 10;
        private int m_DependenciesCurrentPage = 1;
        private GUIContent m_ProfileCreateIconContent;
        private GUIContent m_AssetBundleTagIconContent;

        private void OnEnable()
        {
            LoadAssetBundleProfiles();
            m_SelectedAssetBundleInfo = null;
            m_AssetBundleDependencies = null;
            m_SelectedAssetPath = null;
            m_Dependencies = null;
            RefreshAssetBundles();
            m_IsBuilderOpened = false;
            m_IsDependenciesOpened = false;
            m_ShowRedundancies = false;
            m_BuilderAnimFloat = new AnimFloat(0f, Repaint);
        }

        private void OnGUI()
        {
            if (!m_IsInit)
            {
                m_IsInit = true;
                m_SplitterLPos = position.width * .35f;
                m_ProfileCreateIconContent = EditorGUIUtility.IconContent("CreateAddNew@2x", "|Create New AssetBundleProfile");
                m_AssetBundleTagIconContent = EditorGUIUtility.IconContent("AssetLabelIcon");
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
            SaveProfile();
        }

        #region >> Profile Management
        private void LoadAssetBundleProfiles()
        {
            if (m_Profiles == null)
            {
                m_Profiles = new List<AssetBundleProfile>();
                var guids = AssetDatabase.FindAssets($"t:{typeof(AssetBundleProfile).FullName}");
                for (int i = 0; i < guids.Length; i++)
                {
                    var guid = guids[i];
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var profile = AssetDatabase.LoadAssetAtPath<AssetBundleProfile>(path);
                    m_Profiles.Add(profile);
                }
            }

            if (m_Profile == null && m_Profiles.Count > 0)
                m_Profile = m_Profiles.First();
        }

        private void SaveProfile()
        {
            if (m_Profile != null)
            {
                EditorUtility.SetDirty(m_Profile);
                AssetDatabase.SaveAssets();
            }
        }

        private void SwitchProfile(AssetBundleProfile profile)
        {
            if (m_Profile != profile)
            {
                m_Profile = profile;
                RefreshAssetBundles();
                if (m_Builder != null)
                    m_Builder.profile = profile;
                m_RedundanciesList.Clear();
                if (m_ShowRedundancies)
                    CalculateRedundancies();
            }
        }
        #endregion

        private void OnLeftGUI()
        {
            m_LScrollPosition = GUILayout.BeginScrollView(m_LScrollPosition, GUILayout.Width(m_SplitterLPos),
                GUILayout.MaxWidth(m_SplitterLPos), GUILayout.MinWidth(m_SplitterLPos));
            OnAssetBundleProfilesGUI();
            GUILayout.BeginHorizontal();
            OnAssetBundleSearchGUI();
            OnAssetBundleDropdownMenuGUI();
            GUILayout.EndHorizontal();
            DrawSplitLine(2f, true);
            OnAssetBundleListGUI();
            DragDropObject2AssetBundleListRectCheck();
            OnAssetBundleDetailGUI();
            DrawSplitLine(2f, true);
            OnRedundanciesGUI();
            GUILayout.EndScrollView();
        }

        private void OnAssetBundleProfilesGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            if (m_Profiles.Count > 0)
            {
                if (GUILayout.Button(m_Profile != null ? m_Profile.name : null, EditorStyles.toolbarDropDown))
                {
                    var gm = new GenericMenu();
                    for (int i = 0; i < m_Profiles.Count; i++)
                    {
                        var profile = m_Profiles[i];
                        gm.AddItem(new GUIContent(profile.name), m_Profile == profile, () => SwitchProfile(profile));
                    }
                    gm.ShowAsContext();
                }
            }
            else
            {
                GUILayout.Label(EditorGUIUtility.TrTextContentWithIcon("Please create a profile asset first.", "console.warnicon"),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
            }
            bool showRedundancies = GUILayout.Toggle(m_ShowRedundancies, "Redundancies", EditorStyles.toolbarButton, GUILayout.Width(100f));
            if (showRedundancies != m_ShowRedundancies)
            {
                m_ShowRedundancies = showRedundancies;
                if (m_ShowRedundancies)
                    CalculateRedundancies();
                else
                    m_RedundanciesList.Clear();
                Repaint();
            }
            if (GUILayout.Button(m_ProfileCreateIconContent, EditorStyles.toolbarButton, GUILayout.Width(18f)))
            {
                var path = EditorUtility.SaveFilePanel("Save", Application.dataPath, "New AssetBundleProfile", "asset");
                if (!string.IsNullOrEmpty(path) && path.StartsWith(Application.dataPath))
                {
                    path = path.Replace(Application.dataPath, "Assets");
                    var instance = ScriptableObject.CreateInstance<AssetBundleProfile>();
                    AssetDatabase.CreateAsset(instance, path);
                    AssetDatabase.SaveAssets();
                    EditorGUIUtility.PingObject(instance);
                    m_Profiles.Add(instance);
                    SwitchProfile(instance);
                }
            }
            GUILayout.EndHorizontal();
        }

        private void OnAssetBundleSearchGUI()
        {
            m_SearchAssetBundle = GUILayout.TextField(m_SearchAssetBundle, EditorStyles.toolbarSearchField);
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
                        m_Profile.entries.Clear();
                        EditorUtility.SetDirty(m_Profile);
                        m_SelectedAssetBundleInfo = null;
                        m_AssetBundleDependencies = null;
                        m_SelectedAssetPath = null;
                        m_Dependencies = null;
                        RefreshAssetBundles();
                    }
                });
                gm.ShowAsContext();
            }
        }

        private void OnAssetBundleListGUI()
        {
            if (m_AssetBundleList != null && m_AssetBundleList.Count != 0)
            {
                for (int i = 0; i < m_AssetBundleList.Count; i++)
                {
                    var info = m_AssetBundleList[i];

                    bool nameMatch = string.IsNullOrEmpty(m_SearchAssetBundle)
                        || info.name.ToLower().Contains(m_SearchAssetBundle.ToLower());
                    bool tagMatch = false;
                    if (!nameMatch && !string.IsNullOrEmpty(m_SearchAssetBundle))
                        tagMatch = m_Profile.GetTags(info.name).Any(t => t.ToLower().Contains(m_SearchAssetBundle.ToLower()));
                    if (!nameMatch && !tagMatch)
                        continue;

                    List<string> tags = m_Profile.GetTags(info.name);
                    GUILayout.BeginHorizontal(m_SelectedAssetBundleInfo == info
                        ? "MeTransitionSelectHead" : "ProjectBrowserHeaderBgTop", GUILayout.Height(20f));
                    GUILayout.Label(info.name, GUILayout.ExpandWidth(false));
                    GUILayout.FlexibleSpace();

                    if (tags.Count > 0)
                    {
                        foreach (var tag in tags)
                        {
                            GUIStyle tagStyle = new GUIStyle(EditorStyles.miniButton);
                            tagStyle.normal.textColor = Color.white;
                            tagStyle.fontSize = 10;
                            tagStyle.fixedHeight = 16;
                            GUI.backgroundColor = new Color(0.3f, 0.5f, 0.8f);
                            GUILayout.Label(tag, tagStyle, GUILayout.ExpandWidth(false));
                        }
                        GUI.backgroundColor = Color.white;
                    }

                    if (GUILayout.Button(m_AssetBundleTagIconContent,
                        EditorStyles.label, GUILayout.Width(18f), GUILayout.Height(16f)))
                    {
                        PopupWindow.Show(GUILayoutUtility.GetLastRect(),
                            new AssetBundleTagEditPopupContent(info.name, m_Profile));
                    }

                    GUILayout.EndHorizontal();

                    if (Event.current.type == EventType.MouseDown)
                    {
                        Rect rect = GUILayoutUtility.GetLastRect();
                        if (rect.Contains(Event.current.mousePosition))
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
            if (m_SelectedAssetBundleInfo != null)
            {
                DrawSplitLine(2f, true);
                m_AssetBundleDetailScroll = GUILayout.BeginScrollView(m_AssetBundleDetailScroll,
                    GUILayout.Height(100f), GUILayout.MaxHeight(100f), GUILayout.MinHeight(100f));
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
                        GUILayout.Label(m_AssetBundleDependencies[i]);
                }
                GUILayout.EndScrollView();
            }
        }

        private void OnRedundanciesGUI()
        {
            if (!m_ShowRedundancies || m_Profile == null)
                return;
            if (m_RedundanciesList.Count == 0)
            {
                GUILayout.Label("No implicit redundant assets found.", EditorStyles.centeredGreyMiniLabel);
                return;
            }

            EditorGUILayout.LabelField($"Implicit Redundancies ({m_RedundanciesList.Count})", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            m_RedundanciesTargetBundleName = EditorGUILayout.TextField(m_RedundanciesTargetBundleName);
            if (GUILayout.Button("Extract All", GUILayout.Width(80f)))
                ExtractRedundantAssets(m_RedundanciesTargetBundleName);
            EditorGUILayout.EndHorizontal();

            m_RedundanciesScroll = EditorGUILayout.BeginScrollView(m_RedundanciesScroll, GUILayout.Height(100f));
            foreach (var info in m_RedundanciesList)
            {
                GUILayout.BeginHorizontal(m_SelectedRedundancyInfo == info 
                    ? "MeTransitionSelectHead" : "ProjectBrowserHeaderBgTop", GUILayout.Height(20f));
                if (GUILayout.Button(info.assetPath, EditorStyles.label))
                {
                    m_SelectedRedundancyInfo = info;
                    EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(info.assetPath));
                }
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
            if (m_SelectedRedundancyInfo != null)
            {
                GUILayout.BeginVertical("GroupBox");
                EditorGUI.indentLevel++;
                GUILayout.Label($"Dependent Bundles: {string.Join(", ", m_SelectedRedundancyInfo.bundleNames)}", EditorStyles.miniLabel);
                if (GUILayout.Button($"Add to '{m_RedundanciesTargetBundleName}'", EditorStyles.miniButton))
                {
                    ExtractSingleAsset(m_SelectedRedundancyInfo.assetPath, m_RedundanciesTargetBundleName);
                }
                EditorGUI.indentLevel--;
                GUILayout.EndVertical();
            }
        }


        private void RefreshAssetBundles(bool repaint = false)
        {
            if (m_Profile == null)
                return;
            m_AssetBundleList = new List<AssetBundleEditorInfo>();
            foreach (var entry in m_Profile.entries)
            {
                var info = new AssetBundleEditorInfo(entry.bundleName);
                foreach (var path in entry.assetPaths)
                    info.AddAsset(path);
                m_AssetBundleList.Add(info);
            }
            if (m_ShowRedundancies)
                CalculateRedundancies();
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
                    UpdateAssetBundleDependencies();
                    Repaint();
                }
            }
            else if (Event.current.button == 1)
            {
                GenericMenu gm = new GenericMenu();
                gm.AddItem(new GUIContent("Rename"), false,
                    () => PopupWindow.Show(rect, new AssetBundleRenamePopupWindowContent(info, this)));
                gm.AddItem(new GUIContent("Delete"), false,
                    () => DeleteAssetBundle(info));
                gm.ShowAsContext();
            }
        }

        private void UpdateAssetBundleDependencies()
        {
            if (m_SelectedAssetBundleInfo == null)
                return;
            m_AssetBundleDependencies = CalculateDependencies(m_SelectedAssetBundleInfo.name);
        }

        private string[] CalculateDependencies(string bundleName)
        {
            var entry = m_Profile.entries.Find(e => e.bundleName == bundleName);
            if (entry == null) return new string[0];

            var deps = new HashSet<string>();
            foreach (var path in entry.assetPaths)
            {
                foreach (var dep in AssetDatabase.GetDependencies(path, false))
                {
                    if (dep == path)
                        continue;
                    foreach (var other in m_Profile.entries)
                    {
                        if (other.bundleName == bundleName)
                            continue;
                        if (other.assetPaths.Contains(dep))
                        {
                            deps.Add(other.bundleName);
                            break;
                        }
                    }
                }
            }
            return deps.ToArray();
        }

        private void DeleteAssetBundle(AssetBundleEditorInfo info)
        {
            m_Profile.entries.RemoveAll(e => e.bundleName == info.name);
            EditorUtility.SetDirty(m_Profile);
            if (info == m_SelectedAssetBundleInfo)
            {
                m_SelectedAssetBundleInfo = null;
                m_AssetBundleDependencies = null;
                m_SelectedAssetPath = null;
                m_Dependencies = null;
            }
            RefreshAssetBundles(true);
        }

        private void AddNewAssetBundle()
        {
            string newName = "ab" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            m_Profile.entries.Add(new AssetBundleEntry { bundleName = newName });
            EditorUtility.SetDirty(m_Profile);
            RefreshAssetBundles(true);
            m_SelectedAssetBundleInfo = m_AssetBundleList.Find(b => b.name == newName);
            UpdateAssetBundleDependencies();
            Repaint();
        }

        private void OnRightGUI()
        {
            m_RScrollPosition = GUILayout.BeginScrollView(m_RScrollPosition, GUILayout.ExpandWidth(true));
            GUILayout.BeginHorizontal(GUILayout.Height(20f));
            OnAssetSearchGUI();
            DrawSplitLine(2f, false);

            var open = GUILayout.Toggle(m_IsDependenciesOpened, "Dependencies", EditorStyles.toolbarButton, GUILayout.Width(100f));
            if (open != m_IsDependenciesOpened)
                m_IsDependenciesOpened = open;

            open = GUILayout.Toggle(m_IsBuilderOpened, "Builder", EditorStyles.toolbarButton, GUILayout.Width(65f));
            if (open != m_IsBuilderOpened)
            {
                m_IsBuilderOpened = open;
                m_BuilderAnimFloat.target = m_IsBuilderOpened ? (position.width - m_SplitterLPos) * .5f : 0f;
                if (m_IsBuilderOpened)
                {
                    m_Builder = new AssetBundleBuilder(m_Profile);
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
                    Type[] types = m_SelectedAssetBundleInfo.assets
                        .Select(m => AssetDatabase.GetMainAssetTypeAtPath(m.path)).Distinct().ToArray();
                    GenericMenu gm = new GenericMenu();
                    for (int i = 0; i < types.Length; i++)
                    {
                        Type type = types[i];
                        gm.AddItem(new GUIContent(type.Name), false, () =>
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
            if (m_SelectedAssetBundleInfo != null && m_SelectedAssetBundleInfo.assets.Count > 0)
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
                    else if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Delete)
                    {
                        DeleteAssetFromSelectedBundle(m_SelectedAssetPath);
                        Event.current.Use();
                    }
                }
            }
        }

        private void OnAssetDetailGUI()
        {
            if (m_IsDependenciesOpened && m_SelectedAssetPath != null)
            {
                DrawSplitLine(2f, true, ref m_SplitterRBPos, ref m_IsSplitterRBDragging, .2f, .8f, true);
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

                int pages = Mathf.CeilToInt((float)m_Dependencies.Length / m_DependenciesCountPerPage);
                if (GUILayout.Button("<", GUILayout.Width(20f)) && m_DependenciesCurrentPage > 1)
                    m_DependenciesCurrentPage--;
                GUILayout.Label($"{m_DependenciesCurrentPage}/{pages}", GUILayout.Width(40f));
                if (GUILayout.Button(">", GUILayout.Width(20f)) && m_DependenciesCurrentPage < pages)
                    m_DependenciesCurrentPage++;
                GUILayout.EndHorizontal();

                m_AssetDetailScroll = GUILayout.BeginScrollView(m_AssetDetailScroll,
                    GUILayout.Height(m_SplitterRBPos), GUILayout.MaxHeight(m_SplitterRBPos), GUILayout.MinHeight(m_SplitterRBPos));
                int start = (m_DependenciesCurrentPage - 1) * m_DependenciesCountPerPage;
                for (int i = start; i < m_DependenciesCountPerPage * m_DependenciesCurrentPage; i++)
                {
                    if (i >= m_Dependencies.Length) break;
                    string dep = m_Dependencies[i];
                    Type type = AssetDatabase.GetMainAssetTypeAtPath(dep);
                    if (!m_IconMap.TryGetValue(type, out Texture icon))
                    {
                        icon = AssetPreview.GetMiniTypeThumbnail(type);
                        m_IconMap.Add(type, icon);
                    }
                    GUILayout.BeginHorizontal("ProjectBrowserHeaderBgTop");
                    GUILayout.Label(icon, GUILayout.Width(18f), GUILayout.Height(18f));
                    GUILayout.Label(dep);
                    GUILayout.EndHorizontal();
                    if (Event.current.type == EventType.MouseDown
                        && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                        EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(dep));
                }
                GUILayout.EndScrollView();
            }
        }

        private void CalculateRedundancies()
        {
            m_RedundanciesList.Clear();
            if (m_Profile == null) return;

            HashSet<string> explicitAssets = new HashSet<string>();
            foreach (var entry in m_Profile.entries)
                foreach (var path in entry.assetPaths)
                    explicitAssets.Add(path);

            var depToBundles = new Dictionary<string, HashSet<string>>();
            foreach (var entry in m_Profile.entries)
            {
                foreach (var assetPath in entry.assetPaths)
                {
                    string[] deps;
                    try { deps = AssetDatabase.GetDependencies(assetPath, true); }
                    catch { continue; }

                    foreach (var dep in deps)
                    {
                        if (dep == assetPath) continue;
                        if (!depToBundles.ContainsKey(dep))
                            depToBundles[dep] = new HashSet<string>();
                        depToBundles[dep].Add(entry.bundleName);
                    }
                }
            }

            foreach (var kv in depToBundles)
            {
                if (kv.Value.Count > 1 && !explicitAssets.Contains(kv.Key))
                {
                    m_RedundanciesList.Add(new RedundancyInfo
                    {
                        assetPath = kv.Key,
                        bundleNames = kv.Value.ToList()
                    });
                }
            }

            m_RedundanciesList.Sort((a, b) => string.Compare(a.assetPath, b.assetPath, StringComparison.Ordinal));
        }

        private void ExtractRedundantAssets(string targetBundleName)
        {
            if (string.IsNullOrEmpty(targetBundleName)) 
                return;
            var targetEntry = m_Profile.GetOrCreateEntry(targetBundleName);
            foreach (var info in m_RedundanciesList)
                if (!targetEntry.assetPaths.Contains(info.assetPath))
                    targetEntry.assetPaths.Add(info.assetPath);

            EditorUtility.SetDirty(m_Profile);
            RefreshAssetBundles(true);
            CalculateRedundancies();
            Repaint();
        }

        private void ExtractSingleAsset(string assetPath, string targetBundleName)
        {
            if (string.IsNullOrEmpty(targetBundleName)) 
                return;
            var targetEntry = m_Profile.GetOrCreateEntry(targetBundleName);
            if (!targetEntry.assetPaths.Contains(assetPath))
                targetEntry.assetPaths.Add(assetPath);

            EditorUtility.SetDirty(m_Profile);
            RefreshAssetBundles(true);
            CalculateRedundancies();
            Repaint();
        }

        private void OnAssetListItemMouseDown(string assetPath)
        {
            if (Event.current.button == 0)
            {
                m_SelectedAssetPath = assetPath;
                m_Dependencies = AssetDatabase.GetDependencies(m_SelectedAssetPath, false);
                Repaint();
                EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(m_SelectedAssetPath));
            }
            else if (Event.current.button == 1)
            {
                Event.current.Use();
                GenericMenu gm = new GenericMenu();
                gm.AddItem(new GUIContent("Delete"), false, () => DeleteAssetFromSelectedBundle(assetPath));
                gm.ShowAsContext();
            }
        }

        private void DeleteAssetFromSelectedBundle(string assetPath)
        {
            var entry = m_Profile.entries.Find(e => e.bundleName == m_SelectedAssetBundleInfo?.name);
            if (entry != null)
            {
                entry.assetPaths.Remove(assetPath);
                if (entry.assetPaths.Count == 0)
                    m_Profile.entries.Remove(entry);
                EditorUtility.SetDirty(m_Profile);
                m_SelectedAssetPath = null;
                m_Dependencies = null;
                RefreshAssetBundles(true);
                m_SelectedAssetBundleInfo = m_AssetBundleList.Find(b => b.name == entry.bundleName);
                UpdateAssetBundleDependencies();
            }
        }

        private void DragDropObject2AssetBundleListRectCheck()
        {
            DragDropObject2RectCheck(out IEnumerable<Object> objs);
            if (objs != null)
            {
                foreach (Object obj in objs)
                {
                    string path = AssetDatabase.GetAssetPath(obj);
                    if (string.IsNullOrEmpty(path)) continue;
                    string bundleName = "ab_" + System.IO.Path.GetFileNameWithoutExtension(path).ToLower();
                    var entry = m_Profile.GetOrCreateEntry(bundleName);
                    if (!entry.assetPaths.Contains(path))
                        entry.assetPaths.Add(path);
                }
                EditorUtility.SetDirty(m_Profile);
                RefreshAssetBundles(true);
            }
        }

        private void DragDropObject2AssetListRectCheck()
        {
            DragDropObject2RectCheck(out IEnumerable<Object> objs);
            if (objs != null && m_SelectedAssetBundleInfo != null)
            {
                var entry = m_Profile.GetOrCreateEntry(m_SelectedAssetBundleInfo.name);
                foreach (Object obj in objs)
                {
                    string path = AssetDatabase.GetAssetPath(obj);
                    if (string.IsNullOrEmpty(path)) continue;
                    foreach (var e in m_Profile.entries)
                        if (e.bundleName != entry.bundleName)
                            e.assetPaths.Remove(path);
                    if (!entry.assetPaths.Contains(path))
                        entry.assetPaths.Add(path);
                }
                EditorUtility.SetDirty(m_Profile);
                RefreshAssetBundles(true);
                m_SelectedAssetBundleInfo = m_AssetBundleList.Find(b => b.name == entry.bundleName);
                UpdateAssetBundleDependencies();
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
                            gm.AddItem(new GUIContent("Add New AssetBundle"), false, AddNewAssetBundle);
                            gm.ShowAsContext();
                            Event.current.Use();
                        }
                        break;
                }
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
                    case EventType.MouseDown:
                        isDragging = rect.Contains(Event.current.mousePosition);
                        break;
                    case EventType.MouseDrag:
                        if (isDragging)
                        {
                            splitterPos += (horizontal ? Event.current.delta.y : Event.current.delta.x) * (dirInvert ? -1f : 1f);
                            splitterPos = Mathf.Clamp(splitterPos,
                                (horizontal ? position.height : position.width) * minPercent,
                                (horizontal ? position.height : position.width) * maxPercent);
                            Repaint();
                        }
                        break;
                    case EventType.MouseUp:
                        if (isDragging) isDragging = false;
                        break;
                }
            }
        }

        private void OnBuilderGUI()
        {
            GUILayout.BeginVertical(GUILayout.Width(m_BuilderAnimFloat.value + m_SplitterRPos));
            if (m_IsBuilderOpened && m_Builder != null)
                m_Builder.OnGUI();
            GUILayout.EndVertical();
        }

        public void RenameBundle(string oldName, string newName)
        {
            m_Profile.RenameEntry(oldName, newName);
            SaveProfile();
            RefreshAssetBundles(true);
            if (m_SelectedAssetBundleInfo?.name == oldName)
                m_SelectedAssetBundleInfo.name = newName;
        }
    }
}