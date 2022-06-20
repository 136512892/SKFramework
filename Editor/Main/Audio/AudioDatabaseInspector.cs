using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;

namespace SK.Framework
{
    [CustomEditor(typeof(AudioDatabase))]
    [CSDNUrl("https://coderz.blog.csdn.net/article/details/124712128?spm=1001.2014.3001.5502")]
    public class AudioTargetInspector : AbstractEditor<AudioDatabase>
    {
        private AnimBool foldout;
        private int currentIndex = -1;
        private Dictionary<AudioData, AudioSource> players;

        protected override void OnBaseEnable()
        {
            foldout = new AnimBool(false, Repaint);
            players = new Dictionary<AudioData, AudioSource>();
            EditorApplication.update += Update;
        }
        protected override void OnBaseInspectorGUI()
        {
            //音频库名称
            EGLTextField("Name", ref Target.databaseName);
            //音频库输出混音器
            EGLObjectField("Output AudioMixerGroup", ref Target.outputAudioMixerGroup, false);

            //音频数据折叠栏 使用AnimBool实现动画效果
            foldout.target = EditorGUILayout.Foldout(foldout.target, "Datasets");
            if (EditorGUILayout.BeginFadeGroup(foldout.faded))
            {
                for (int i = 0; i < Target.datasets.Count; i++)
                {
                    var data = Target.datasets[i];
                    GUILayout.BeginHorizontal();
                    //绘制音频图标
                    GUILayout.Label(EditorGUIUtility.IconContent("SceneViewAudio"), GUILayout.Width(20f));

                    //音频数据名称
                    EGLTextField(ref data.name, GUILayout.Width(120f));
                    //使用音频名称绘制Button按钮 点击后使用PingObject方法定位该音频资源
                    Color colorCache = GUI.color;
                    GUI.color = currentIndex == i ? Color.cyan : colorCache;
                    if (GUILayout.Button(data.clip != null ? data.clip.name : "Null"))
                    {
                        currentIndex = i;
                        EditorGUIUtility.PingObject(data.clip);
                    }
                    GUI.color = colorCache;

                    //若该音频正在播放 计算其播放进度 
                    string progress = players.ContainsKey(data) ? ToTimeFormat(players[data].time) : "00:00";
                    GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, players.ContainsKey(data) ? .9f : .5f);
                    //显示信息：播放进度 / 音频时长 (00:00 / 00:00)
                    GUILayout.Label($"({progress} / {(data.clip != null ? ToTimeFormat(data.clip.length) : "00:00")})",
                        new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerRight, fontSize = 8, fontStyle = FontStyle.Italic }, GUILayout.Width(60f));
                    GUI.color = colorCache;

                    //播放按钮
                    if (GUILayout.Button(EditorGUIUtility.IconContent("PlayButton"), GUILayout.Width(20f)))
                    {
                        if (!players.ContainsKey(data))
                        {
                            //创建一个物体并添加AudioSource组件 
                            var source = EditorUtility.CreateGameObjectWithHideFlags("Audio Player", HideFlags.HideAndDontSave).AddComponent<AudioSource>();
                            source.clip = data.clip;
                            source.outputAudioMixerGroup = Target.outputAudioMixerGroup;
                            source.Play();
                            players.Add(data, source);
                        }
                    }
                    //停止播放按钮
                    if (GUILayout.Button(EditorGUIUtility.IconContent("PauseButton"), GUILayout.Width(20f)))
                    {
                        if (players.ContainsKey(data))
                        {
                            DestroyImmediate(players[data].gameObject);
                            players.Remove(data);
                        }
                    }
                    //删除按钮 点击后删除该项音频数据
                    if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus"), GUILayout.Width(20f)))
                    {
                        Undo.RecordObject(Target, "Delete");
                        Target.datasets.Remove(data);
                        if (players.ContainsKey(data))
                        {
                            DestroyImmediate(players[data].gameObject);
                            players.Remove(data);
                        }
                        EditorUtility.SetDirty(Target);
                        Repaint();
                    }
                    GUILayout.EndHorizontal();
                }

                EditorGUILayout.Space();

                //以下代码块中绘制了一个矩形区域，将AudioClip资产拖到该区域则添加一项音频数据
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(GUIContent.none, GUILayout.ExpandWidth(true));
                    Rect lastRect = GUILayoutUtility.GetLastRect();
                    var dropRect = new Rect(lastRect.x + 2f, lastRect.y - 2f, 120f, 20f);
                    bool containsMouse = dropRect.Contains(Event.current.mousePosition);
                    if (containsMouse)
                    {
                        switch (Event.current.type)
                        {
                            case EventType.DragUpdated:
                                bool containsAudioClip = DragAndDrop.objectReferences.OfType<AudioClip>().Any();
                                DragAndDrop.visualMode = containsAudioClip ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.Rejected;
                                Event.current.Use();
                                Repaint();
                                break;
                            case EventType.DragPerform:
                                IEnumerable<AudioClip> audioClips = DragAndDrop.objectReferences.OfType<AudioClip>();
                                foreach (var audioClip in audioClips)
                                {
                                    if (Target.datasets.Find(m => m.clip == audioClip) == null)
                                    {
                                        Undo.RecordObject(Target, "Add");
                                        Target.datasets.Add(new AudioData() { name = audioClip.name, clip = audioClip });
                                        EditorUtility.SetDirty(Target);
                                    }
                                }
                                Event.current.Use();
                                Repaint();
                                break;
                        }
                    }
                    Color color = GUI.color;
                    GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, containsMouse ? .9f : .5f);
                    GUI.Box(dropRect, "Drop AudioClips Here", new GUIStyle(GUI.skin.box) { fontSize = 10 });
                    GUI.color = color;
                }
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndFadeGroup();
        }
        private void OnDestroy()
        {
            EditorApplication.update -= Update;
            foreach (var player in players)
            {
                DestroyImmediate(player.Value.gameObject);
            }
            players.Clear();
        }
        private void Update()
        {
            Repaint();
            foreach (var player in players)
            {
                if (!player.Value.isPlaying)
                {
                    DestroyImmediate(player.Value.gameObject);
                    players.Remove(player.Key);
                    break;
                }
            }
        }

        //将秒数转换为00：00时间格式字符串
        private string ToTimeFormat(float time)
        {
            int seconds = (int)time;
            int minutes = seconds / 60;
            seconds %= 60;
            return string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }
    }
}