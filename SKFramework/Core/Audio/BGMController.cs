using UnityEngine;
using UnityEngine.Audio;

namespace SK.Framework.Audio
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
                    }
                    else
                    {
                        source.UnPause();
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
        }
        public void Stop()
        {
            source.Stop();
        }
        #endregion
    }
}