using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

namespace SK.Framework.Audio
{
    [DisallowMultipleComponent]
    public class BGMController : MonoBehaviour, IBGMController
    {
        private AudioSource source;
        private Coroutine pauseCoroutine;

        private float volume = 1f;
        private bool isPaused = false;
        private bool isPausing = false;

        public float Volume
        {
            get
            {
                return volume;
            }
            set
            {
                if (volume != value)
                {
                    volume = value;
                    if (!isPausing)
                    {
                        source.volume = volume;
                    }
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

        private void Start()
        {
            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            volume = source.volume;
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
            if (source.isPlaying)
            {
                source.Stop();
            }
        }

        public void Pause()
        {
            if (isPaused) return;
            if (pauseCoroutine != null)
                StopCoroutine(pauseCoroutine);
            pauseCoroutine = StartCoroutine(PauseCoroutine(false));
        }

        public void Unpause()
        {
            if (!isPaused) return;
            if (pauseCoroutine != null)
                StopCoroutine(pauseCoroutine);
            pauseCoroutine = StartCoroutine(PauseCoroutine(true));
        }

        //暂停协程 用于声音暂停/恢复时音量渐小渐大效果
        private IEnumerator PauseCoroutine(bool unpause)
        {
            isPaused = !unpause;
            isPausing = true;
            float duration = .5f;
            float beginTime = UnityEngine.Time.time;
            if (!unpause) source.UnPause();
            for (; UnityEngine.Time.time - beginTime <= duration;)
            {
                float t = (UnityEngine.Time.time - beginTime) / duration;
                source.volume = Mathf.Lerp(unpause ? 0f : volume, unpause ? volume : 0f, t);
                yield return null;
            }
            source.volume = unpause ? volume : 0f;
            if (unpause) source.UnPause();
            pauseCoroutine = null;
            isPausing = false;
        }
    }
}