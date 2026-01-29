/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using UnityEngine;
using UnityEngine.Audio;
using SK.Framework.ObjectPool;

namespace SK.Framework.Audio
{
    public class AudioHandler : MonoBehaviour, IPoolable
    {
        private AudioSource m_Source;
        private bool m_IsPaused;
        private Transform m_FollowTarget;
        private bool m_AutoRecycle = true;

        bool IPoolable.isRecycled { get; set; }

        DateTime IPoolable.entryTime { get; set; }

        public float progress { get; internal set; }

        public bool isPaused
        {
            get
            {
                return m_IsPaused;
            }
            set
            {
                if (m_IsPaused == value) 
                    return;
                m_IsPaused = value;
                if (m_IsPaused) 
                    m_Source.Pause();
                else 
                    m_Source.UnPause();
            }
        }

        public float volume
        {
            get
            {
                return m_Source.volume;
            }
        }

        public bool isPlaying
        {
            get
            {
                return m_Source.isPlaying;
            }
        }


        private void Update()
        {
            if (m_FollowTarget != null)
                transform.position = m_FollowTarget.position;
            if (m_Source != null && m_Source.clip != null && m_Source.isPlaying)
                progress = Mathf.Clamp01(m_Source.time / m_Source.clip.length);
            if (m_Source != null && !m_Source.isPlaying && !m_Source.loop)
                Stop();
        }

        public void Stop()
        {
            m_Source.Stop();
            if (m_AutoRecycle)
                SKFramework.Module<ObjectPool.ObjectPool>().Get<AudioHandler>().Recycle(this);
        }

        void IPoolable.OnRecycled()
        {
            name = "AudioHandler(Cache)";
            gameObject.SetActive(false);
            m_FollowTarget = null;
            transform.position = Vector3.zero;
            m_Source.clip = null;
            m_Source.outputAudioMixerGroup = null;
            m_Source.loop = false;
            m_Source.volume = 1f;
            m_Source.priority = 128;
            m_Source.pitch = 1f;
            m_Source.panStereo = 0f;
            m_Source.spatialBlend = 0f;
            m_Source.minDistance = 1f;
            m_Source.maxDistance = 500f;
        }

        public AudioHandler Play()
        {
            name = m_Source.clip ? m_Source.clip.name : "Null";
            m_Source.Play();
            return this;
        }
        public AudioHandler SetSource(AudioSource audioSource)
        {
            m_Source = audioSource;
            return this;
        }
        public AudioHandler SetClip(AudioClip audioClip)
        {
            m_Source.clip = audioClip;
            return this;
        }
        public AudioHandler SetOutput(AudioMixerGroup audioMixerGroup)
        {
            m_Source.outputAudioMixerGroup = audioMixerGroup;
            return this;
        }
        public AudioHandler SetLoop(bool isLoop)
        {
            m_Source.loop = isLoop;
            return this;
        }
        public AudioHandler SetVolume(float volume)
        {
            m_Source.volume = volume;
            return this;
        }
        public AudioHandler SetPitch(float pitch)
        {
            m_Source.pitch = pitch;
            return this;
        }
        public AudioHandler SetSpatialBlend(float spatialBlend)
        {
            m_Source.spatialBlend = spatialBlend;
            return this;
        }
        public AudioHandler SetFollowTarget(Transform followTarget)
        {
            m_FollowTarget = followTarget;
            return this;
        }
        public AudioHandler SetPriority(int priority)
        {
            m_Source.priority = priority;
            return this;
        }
        public AudioHandler SetStereoPan(float panStereo)
        {
            m_Source.panStereo = panStereo;
            return this;
        }
        public AudioHandler SetMute(bool isMute)
        {
            m_Source.mute = isMute;
            return this;
        }
        public AudioHandler SetPause(bool isPause)
        {
            isPaused = isPause;
            return this;
        }
        public AudioHandler SetPoint(Vector3 pos)
        {
            transform.position = pos;
            return this;
        }
        public AudioHandler SetMinDistance(float minDistance)
        {
            m_Source.minDistance = minDistance;
            return this;
        }
        public AudioHandler SetMaxDistance(float maxDistance)
        {
            m_Source.maxDistance = maxDistance;
            return this;
        }
        public AudioHandler SetAutoRecycle(bool autoRecycle)
        {
            m_AutoRecycle = autoRecycle;
            return this;
        }
    }
}