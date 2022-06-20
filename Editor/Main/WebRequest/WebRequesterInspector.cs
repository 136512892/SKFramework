using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace SK.Framework
{
    [CustomEditor(typeof(WebRequester))]
    [CSDNUrl("https://coderz.blog.csdn.net/article/details/124808861?spm=1001.2014.3001.5502")]
    public class WebRequesterInspector : AbstractEditor<WebRequester>
    {
        //接口字典
        private Dictionary<string, AbstractWebInterface> dic;
        //折叠栏字典
        private Dictionary<AbstractWebInterface, bool> foldoutDic; 
        
        protected override bool IsEnableBaseInspectorGUI
        {
            get
            {
                return true;
            }
        }

        protected override void OnRuntimeEnable()
        {
            dic = typeof(WebRequester).GetField("dic", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(WebRequester.Instance) as Dictionary<string, AbstractWebInterface>;
            foldoutDic = new Dictionary<AbstractWebInterface, bool>();
        }

        protected override void OnRuntimeInspectorGUI()
        {
            foreach (var kv in dic)
            {
                if (!foldoutDic.ContainsKey(kv.Value))
                {
                    foldoutDic.Add(kv.Value, true);
                }
                GUILayout.BeginHorizontal();
                GUILayout.Space(10f);
                //折叠栏
                foldoutDic[kv.Value] = EditorGUILayout.Foldout(foldoutDic[kv.Value], kv.Key, true);
                GUILayout.EndHorizontal();
                //如果折叠栏为打开状态
                if (foldoutDic[kv.Value])
                {
                    GUILayout.Label(string.Format("  接口名称：{0}", kv.Value.name));
                    GUILayout.Label(string.Format("  接口地址：{0}", kv.Value.url));
                    GUILayout.Label(string.Format("  请求方式：{0}", kv.Value.method));
                    GUILayout.Label(string.Format("  接口参数：{0}", kv.Value.args.Length));
                    for (int i = 0; i < kv.Value.args.Length; i++)
                    {
                        string arg = kv.Value.args[i];
                        GUILayout.Label(string.Format("    参数{0}: {1}", i + 1, arg));
                    }
                }
            }

            //清理折叠栏字典
            foreach (var kv in foldoutDic)
            {
                if (!dic.ContainsValue(kv.Key))
                {
                    foldoutDic.Remove(kv.Key);
                    break;
                }
            }
        }
    }
}