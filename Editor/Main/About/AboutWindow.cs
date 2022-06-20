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

        #region About
        [MenuItem("SKFramework/About", priority = 0)]
        private static void Open()
        {
            var window = GetWindow<AboutWindow>(true, "About", true);
            window.position = new Rect(200, 200, 360, 450);
            window.minSize = new Vector2(360, 450);
            window.maxSize = new Vector2(360, 450);
            window.Show();
        }

        private const string csdnUrl = "https://coderz.blog.csdn.net/";
        private const string githubUrl = "https://github.com/136512892";
        private Texture csdnLogo;
        private Texture csdnTex;
        private Texture wechatTex;
        private Texture qqTex;
        private Texture githubTex;

        private void OnEnable()
        {
            csdnLogo = AssetDatabase.LoadAssetAtPath<Texture>("Assets/SKFramework/Editor/Main/Texture/CSDNLogo.png");
            csdnTex = AssetDatabase.LoadAssetAtPath<Texture>("Assets/SKFramework/Editor/Main/Texture/CSDN.png");
            wechatTex = AssetDatabase.LoadAssetAtPath<Texture>("Assets/SKFramework/Editor/Main/Texture/WeChat.png");
            qqTex = AssetDatabase.LoadAssetAtPath<Texture>("Assets/SKFramework/Editor/Main/Texture/QQ.png");
            githubTex = AssetDatabase.LoadAssetAtPath<Texture>("Assets/SKFramework/Editor/Main/Texture/Github.png");
        }

        private void OnGUI()
        {
            GUILayout.Label("SKFramework", new GUIStyle(GUI.skin.label) { fontSize = 50, fontStyle = FontStyle.Bold});

            GUILayout.Label("Version: 0.0.1");
            GUILayout.Label("本框架开发所用环境: Unity2020.3.16");
            GUILayout.Label("请将SKFramework直接放在Assets根目录下");

            GUILayout.Space(20f);

            GUILayout.Label("关于作者");
            //简介
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                GUILayout.Label("  CoderZ，一名Unity3d开发工程师，从事游戏、\r\nVR/AR/MR、虚拟仿真、数字孪生等相关项目开\r\n发。" +
                    "个人微信公众号：当代野生程序猿，欢迎扫\r\n码关注。");
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label(wechatTex, GUILayout.Width(75f), GUILayout.Height(75f));
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            //CSDN
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                GUILayout.Label(EditorGUIUtility.TrTextContentWithIcon(" CSDN:", csdnLogo), GUILayout.Width(90f));
                GUILayout.Label("  博客专家认证");
                GUILayout.Label("  Unity领域优质创作者");
                GUILayout.BeginHorizontal();
                GUILayout.Label($"  主页：{csdnUrl}", GUILayout.Width(220f));
                if (GUILayout.Button("访问", GUILayout.Width(35f)))
                {
                    Application.OpenURL(csdnUrl);
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label(csdnTex, GUILayout.Width(75f), GUILayout.Height(75f));
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
            
            EditorGUILayout.Space();

            //Github
            GUILayout.Label(EditorGUIUtility.TrTextContentWithIcon(" Github:", githubTex), GUILayout.Width(90f));
            GUILayout.BeginHorizontal();
            GUILayout.Label($"  主页：{githubUrl}", GUILayout.Width(220f));
            if (GUILayout.Button("访问", GUILayout.Width(35f)))
            {
                Application.OpenURL(githubUrl);
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            //腾讯QQ
            GUILayout.Label(EditorGUIUtility.TrTextContentWithIcon(" 腾讯QQ:", qqTex), GUILayout.Width(90f));
            GUILayout.BeginHorizontal();
            GUILayout.Label("  邮箱：136512892@qq.com", GUILayout.Width(220f));
            if (GUILayout.Button("复制", GUILayout.Width(35f)))
            {
                GUIUtility.systemCopyBuffer = "136512892@qq.com";
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("  QQ群：644180362", GUILayout.Width(220f));
            if (GUILayout.Button("复制", GUILayout.Width(35f)))
            {
                GUIUtility.systemCopyBuffer = "644180362";
            }
            GUILayout.EndHorizontal();
        }
        #endregion
    }
}