using UnityEngine;
using UnityEngine.Audio;

namespace SK.Framework
{
    /// <summary>
    /// 背景音乐控制器
    /// </summary>
    public class BGMController
    {
        #region Private Variables
        private readonly AudioSource source;
        private bool isPaused;
        #endregion

        #region Public Properties
        public float Volume
        {
            get
            {
                return source.volume;
            }
            set
            {
                if (source.volume != value)
                {
                    source.volume = value;
                    Log.Info(Module.Audio, string.Format("背景音乐音量调整为 {0}", source.volume));
                }
            }
        }
        public int Priority
        {
            get
            {
                return source.priority;
            }
            set
            {
                if (source.priority != value)
                {
                    source.priority = value;
                    Log.Info(Module.Audio, string.Format("背景音乐优先级调整为 {0}", source.priority));
                }
            }
        }
        public float Pitch
        {
            get
            {
                return source.pitch;
            }
            set
            {
                if (source.pitch != value)
                {
                    source.pitch = value;
                    Log.Info(Module.Audio, string.Format("背景音乐音高调整为 {0}", source.pitch));
                }
            }
        }
        public bool IsMuted
        {
            get
            {
                return source.mute;
            }
            set
            {
                if (source.mute != value)
                {
                    source.mute = value;
                    Log.Info(Module.Audio, string.Format("背景音乐{0}静音", value ? "设置" : "取消"));
                }
            }
        }
        public bool IsPaused
        {
            get
            {
                return isPaused;
            }
            set
            {
                if (isPaused != value)
                {
                    isPaused = value;
                    if (isPaused)
                    {
                        source.Pause();
                        Log.Info(Module.Audio, "暂停背景音乐");
                    }
                    else
                    {
                        source.UnPause();
                        Log.Info(Module.Audio, "恢复背景音乐");
                    }
                }
            }
        }
        public bool IsPlaying
        {
            get
            {
                return source.isPlaying;
            }
        }
        public bool IsLoop
        {
            get
            {
                return source.loop;
            }
            set
            {
                if (source.loop != value)
                {
                    source.loop = value;
                    Log.Info(Module.Audio, string.Format("背景音乐循环 {0}", source.loop));
                }
            }
        }
        public float Progress
        {
            get
            {
                return source.isPlaying ? source.time / source.clip.length : 0f;
            }
        }
        public float Time
        {
            get
            {
                return source.time;
            }
        }
        public AudioClip Clip
        {
            get
            {
                return source.clip;
            }
        }
        public AudioMixerGroup Output
        {
            get
            {
                return source.outputAudioMixerGroup;
            }
            set
            {
                source.outputAudioMixerGroup = value;
            }
        }
        #endregion

        #region Public Methods
        public BGMController()
        {
            source = new GameObject("[BGM]").AddComponent<AudioSource>();
            source.loop = true;
            source.transform.SetParent(Audio.Instance.transform);
        }
        public void Play(AudioClip bgm)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
            source.clip = bgm;
            source.Play();
            isPaused = false;
            Log.Info(Module.Audio, string.Format("播放背景音乐 {0}", bgm != null ? bgm.name : "--"));
        }
        public void Stop()
        {
            source.Stop();
            Log.Info(Module.Audio, "终止背景音乐");
        }
        #endregion
    }
}