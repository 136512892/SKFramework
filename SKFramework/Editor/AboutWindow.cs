using UnityEditor;
using UnityEngine;

namespace SK.Framework
{
    public class AboutWindow : EditorWindow
    {
        [InitializeOnLoadMethod]
        private static void OnEditorLaunch()
        {
            if (EditorApplication.timeSinceStartup < 30)
            {
                EditorApplication.delayCall += () =>
                {
                    Open();
                };
            }
        }

        [MenuItem("SKFramework/About", priority = 0)]
        private static void Open()
        {
            var window = GetWindow<AboutWindow>(true, "About", true);
            window.position = new Rect(200, 200, 350, 300);
            window.minSize = new Vector2(350, 300);
            window.maxSize = new Vector2(350, 300);
            window.Show();
        }

        private const string csdnUrl = "https://coderz.blog.csdn.net/";
        private const string githubUrl = "https://github.com/136512892";
        private const string qqAccount = "136512892";
        private const string qqGroup = "644180362";
        private const string wechat = "CoderZ1010";
        private const string whcbqhn = "当代野生程序猿";

        private void OnGUI()
        {
            GUILayout.Label("SKFramework", new GUIStyle(GUI.skin.label) { fontSize = 50, fontStyle = FontStyle.Bold});

            GUILayout.Label("Version: 1.4.0");
            GUILayout.Label("本框架开发所用环境: Unity2020.3.16");
            GUILayout.Label("请将SKFramework直接放在Assets根目录下使用");

            GUILayout.Space(20f);

            GUILayout.Label("作者：CoderZ");
            
            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("CSDN主页：{0}", csdnUrl));
            if (GUILayout.Button("访问", GUILayout.Width(40f)))
            {
                Application.OpenURL(csdnUrl);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("GitHub主页：{0}", githubUrl));
            if (GUILayout.Button("访问", GUILayout.Width(40f)))
            {
                Application.OpenURL(githubUrl);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("QQ号：{0}", qqAccount));
            if (GUILayout.Button("复制", GUILayout.Width(40f)))
            {
                GUIUtility.systemCopyBuffer = qqAccount;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("QQ群：{0}", qqGroup));
            if (GUILayout.Button("复制", GUILayout.Width(40f)))
            {
                GUIUtility.systemCopyBuffer = qqGroup;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("微信号：{0}", wechat));
            if (GUILayout.Button("复制", GUILayout.Width(40f)))
            {
                GUIUtility.systemCopyBuffer = wechat;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("微信公众号：{0}", whcbqhn));
            if (GUILayout.Button("复制", GUILayout.Width(40f)))
            {
                GUIUtility.systemCopyBuffer = whcbqhn;
            }
            GUILayout.EndHorizontal();
        }
    }
}