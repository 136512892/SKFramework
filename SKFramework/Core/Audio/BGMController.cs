/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

namespace SK.Framework.Audio
{
    public class BGMController : MonoBehaviour
    {
        private AudioSource m_Source;
        private Coroutine m_PauseCoroutine;

        private float m_Volume = 1f;
        private bool m_IsPaused = false;
        private bool m_IsPausing = false;

        public float volume
        {
            get
            {
                return m_Volume;
            }
            set
            {
                if (m_Volume != value)
                {
                    m_Volume = value;
                    if (!m_IsPausing)
                        m_Source.volume = m_Volume;
                }
            }
        }

        public int priority
        {
            get
            {
                return m_Source.priority;
            }
            set
            {
                if (m_Source.priority != value)
                    m_Source.priority = value;
            }
        }

        public float pitch
        {
            get
            {
                return m_Source.pitch;
            }
            set
            {
                if (m_Source.pitch != value)
                    m_Source.pitch = value;
            }
        }

        public bool isMuted
        {
            get
            {
                return m_Source.mute;
            }
            set
            {
                if (m_Source.mute != value)
                    m_Source.mute = value;
            }
        }

        public bool isPaused
        {
            get
            {
                return m_IsPaused;
            }
            set
            {
                if (m_IsPaused != value)
                {
                    m_IsPaused = value;
                    if (m_IsPaused)
                        m_Source.Pause();
                    else
                        m_Source.UnPause();
                }
            }
        }

        public bool isPlaying
        {
            get
            {
                return m_Source.isPlaying;
            }
        }

        public bool isLoop
        {
            get
            {
                return m_Source.loop;
            }
            set
            {
                if (m_Source.loop != value)
                    m_Source.loop = value;
            }
        }

        public float progress
        {
            get
            {
                return m_Source.isPlaying ? m_Source.time / m_Source.clip.length : 0f;
            }
        }

        public float time
        {
            get
            {
                return m_Source.time;
            }
        }

        public AudioMixerGroup output
        {
            get
            {
                return m_Source.outputAudioMixerGroup;
            }
            set
            {
                m_Source.outputAudioMixerGroup = value;
            }
        }

        private void Awake()
        {
            m_Source = gameObject.AddComponent<AudioSource>();
            m_Source.playOnAwake = false;
            m_Volume = m_Source.volume;
        }

        public void Play(AudioClip bgm)
        {
            if (m_Source.isPlaying)
                m_Source.Stop();
            m_Source.clip = bgm;
            m_Source.Play();
            m_IsPaused = false;
        }

        public void Stop()
        {
            if (m_Source.isPlaying)
                m_Source.Stop();
        }

        public void Pause()
        {
            if (m_IsPaused) return;
            if (m_PauseCoroutine != null)
                StopCoroutine(m_PauseCoroutine);
            m_PauseCoroutine = StartCoroutine(PauseCoroutine(false));
        }

        public void Unpause()
        {
            if (!m_IsPaused) return;
            if (m_PauseCoroutine != null)
                StopCoroutine(m_PauseCoroutine);
            m_PauseCoroutine = StartCoroutine(PauseCoroutine(true));
        }

        private IEnumerator PauseCoroutine(bool unpause)
        {
            m_IsPaused = !unpause;
            m_IsPausing = true;
            float duration = .5f;
            float beginTime = Time.time;
            if (!unpause) 
                m_Source.UnPause();
            for (; Time.time - beginTime <= duration;)
            {
                float t = (Time.time - beginTime) / duration;
                m_Source.volume = Mathf.Lerp(unpause 
                    ? 0f : m_Volume, unpause ? m_Volume : 0f, t);
                yield return null;
            }
            m_Source.volume = unpause ? m_Volume : 0f;
            if (unpause) 
                m_Source.UnPause();
            m_PauseCoroutine = null;
            m_IsPausing = false;
        }
    }
}