using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace SK.Framework.Resource
{
    public class AssetBundleConfigure : EditorWindow
    {
        [MenuItem("SKFramework/Resource/AssetBundle Configure")]
        public static void Open()
        {
            GetWindow<AssetBundleConfigure>("AssetBundle Configure").Show();
        }

        private Vector2 lScrollPosition, rScrollPosition;
        //分割线宽度
        private const float splitterWidth = 2f;
        //分割线位置
        private float splitterPos;
        private Rect splitterRect;
        //是否正在拖拽分割线
        private bool isDragging;

        //AssetBundle名称集合
        private string[] assetBundleNames;
        //<AssetBundle名称，Assets路径集合>
        private Dictionary<string, string[]> map;
        //当前选中的AssetBundle名称
        private string selectedAssetBundleName;
        //当前选中的Asset路径
        private string selectedAssetPath;

        //检索AssetBundle
        private string searchAssetBundle;
        //检索Asset路径
        private string searchAssetPath;

        private void OnEnable()
        {
            splitterPos = position.width * .5f;

            Init();
        }

        private void OnDisable()
        {
            map = null;
            searchAssetBundle = null;
            selectedAssetBundleName = null;
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            {
                lScrollPosition = GUILayout.BeginScrollView(lScrollPosition, GUILayout.Width(splitterPos), GUILayout.MaxWidth(splitterPos), GUILayout.MinWidth(splitterPos));
                OnLeftGUI();
                GUILayout.EndScrollView();

                //分割线
                GUILayout.Box(string.Empty, GUILayout.Width(splitterWidth), GUILayout.MaxWidth(splitterWidth), GUILayout.MinWidth(splitterWidth), GUILayout.ExpandHeight(true));
                splitterRect = GUILayoutUtility.GetLastRect();

                rScrollPosition = GUILayout.BeginScrollView(rScrollPosition, GUILayout.ExpandWidth(true));
                OnRightGUI();
                GUILayout.EndScrollView();
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
                            splitterPos += Event.current.delta.x;
                            //限制其最大最小值
                            splitterPos = Mathf.Clamp(splitterPos, position.width * .2f, position.width * .8f);
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

        private void Init()
        {
            selectedAssetBundleName = null;
            selectedAssetPath = null;
            //获取所有AssetBundle名称
            assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
            //初始化map字典
            map = new Dictionary<string, string[]>();
            for (int i = 0; i < assetBundleNames.Length; i++)
            {
                map.Add(assetBundleNames[i], AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleNames[i]));
            }
        }

        private void OnLeftGUI()
        {
            //刷新 重新加载AssetBundle信息
            if (GUILayout.Button("Refresh"))
            {
                Init();
            }
            if (GUILayout.Button("RemoveUnusedABNames"))
            {
                AssetDatabase.RemoveUnusedAssetBundleNames();
                Init();
            }
            searchAssetBundle = GUILayout.TextField(searchAssetBundle, EditorStyles.toolbarSearchField);
            //当点击鼠标且鼠标位置不在输入框中时 取消控件的聚焦
            if (Event.current.type == EventType.MouseDown && !GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                GUI.FocusControl(null);
                Repaint();
            }

            if (assetBundleNames.Length == 0) return;
            for (int i = 0; i < assetBundleNames.Length; i++)
            {
                string assetBundleName = assetBundleNames[i];
                if (!string.IsNullOrEmpty(searchAssetBundle) && !assetBundleName.ToLower().Contains(searchAssetBundle.ToLower())) continue;
                GUILayout.BeginHorizontal(selectedAssetBundleName == assetBundleName ? "MeTransitionSelectHead" : "ProjectBrowserHeaderBgTop", GUILayout.Height(20f));
                GUILayout.Label(assetBundleName);
                GUILayout.EndHorizontal();
                //鼠标点击选中当前项
                if (Event.current.type == EventType.MouseDown && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                {
                    selectedAssetBundleName = assetBundleName;
                    Repaint();
                }
            }
        }

        private void OnRightGUI()
        {
            searchAssetPath = GUILayout.TextField(searchAssetPath, EditorStyles.toolbarSearchField);
            //当点击鼠标且鼠标位置不在输入框中时 取消控件的聚焦
            if (Event.current.type == EventType.MouseDown && !GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                GUI.FocusControl(null);
                Repaint();
            }
            if (selectedAssetBundleName == null) return;
            string[] assetPaths = map[selectedAssetBundleName];
            if (assetPaths.Length == 0) return;
            for (int i = 0; i < assetPaths.Length; i++)
            {
                string assetPath = assetPaths[i];
                if (!string.IsNullOrEmpty(searchAssetPath) && !assetPath.ToLower().Contains(searchAssetPath.ToLower())) continue;
                GUILayout.BeginHorizontal(selectedAssetPath == assetPath ? "MeTransitionSelectHead" : "ProjectBrowserHeaderBgTop", GUILayout.Height(20f));
                GUILayout.Label(assetPaths[i]);
                GUILayout.EndHorizontal();
                //鼠标点击选中当前项
                if (Event.current.type == EventType.MouseDown && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                {
                    selectedAssetPath = assetPath;
                    Repaint();
                    EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(selectedAssetPath));
                }
            }
        }
    }
}