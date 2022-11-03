using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using UnityEditor.AnimatedValues;
#endif

namespace SK.Framework.Audios
{
    /// <summary>
    /// 音频库
    /// </summary>
    [CreateAssetMenu(fileName = "New Audio Database", order = 215)]
    public class AudioDatabase : ScriptableObject
    {
        /// <summary>
        /// 音频库名称
        /// </summary>
        public string databaseName;
        /// <summary>
        /// 输出混音器组
        /// </summary>
        public AudioMixerGroup outputAudioMixerGroup;
        /// <summary>
        /// 音频数据列表
        /// </summary>
        public List<AudioData> datasets = new List<AudioData>(0);

        public AudioData this[int index]
        {
            get
            {
                return datasets[index];
            }
        }
        public AudioData this[string dataName]
        {
            get
            {
                return datasets.Find(m => m.name == dataName);
            }
        }

        public AudioClip GetClip(string dataName)
        {
            return datasets.Find(m => m.name == dataName)?.clip;
        }

        public void PlayAsBGM(string dataName)
        {
            Audio.BGM.Output = outputAudioMixerGroup;
            Audio.BGM.Play(GetClip(dataName));
        }
        public AudioHandler PlayAsSFX(string dataName)
        {
            var clip = GetClip(dataName);
            if (clip != null)
            {
                return Audio.SFX.Play(clip, outputAudioMixerGroup);
            }
            return null;
        }
        public AudioHandler PlayAsSFX(string dataName, Vector3 position)
        {
            var clip = GetClip(dataName);
            if (clip != null)
            {
                return Audio.SFX.Play(clip, position, outputAudioMixerGroup);
            }
            return null;
        }
        public AudioHandler PlayAsSFX(string dataName, Transform followTarget)
        {
            var clip = GetClip(dataName);
            if (clip != null)
            {
                return Audio.SFX.Play(clip, followTarget, outputAudioMixerGroup);
            }
            return null;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AudioDatabase))]
    public class AudioDatabaseEditor : Editor
    {
        private AudioDatabase Target;
        private AnimBool foldout;
        private int currentIndex = -1;
        private Dictionary<AudioData, AudioSource> players;

        private void OnEnable()
        {
            Target = target as AudioDatabase;
            foldout = new AnimBool(false, Repaint);
            players = new Dictionary<AudioData, AudioSource>();
            EditorApplication.update += Update;
        }

        public override void OnInspectorGUI()
        {
            //音频库名称
            var newName = EditorGUILayout.TextField("Name", Target.databaseName);
            if(newName != Target.databaseName)
            {
                Undo.RecordObject(Target, "DatabaseName");
                Target.databaseName = newName;
                EditorUtility.SetDirty(Target);
            }

            //音频库输出混音器
            var namg = EditorGUILayout.ObjectField("Output AudioMixerGroup", Target.outputAudioMixerGroup, typeof(AudioMixerGroup), false) as AudioMixerGroup;
            if (namg != Target.outputAudioMixerGroup)
            {
                Undo.RecordObject(Target, "OutputAudioMixerGroup");
                Target.outputAudioMixerGroup = namg;
                EditorUtility.SetDirty(Target);
            }

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
                    var newDataName = EditorGUILayout.TextField(data.name);
                    if (newDataName != data.name)
                    {
                        Undo.RecordObject(Target, "AudioData Name");
                        data.name = newDataName;
                        EditorUtility.SetDirty(Target);
                    }
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
#endif
}