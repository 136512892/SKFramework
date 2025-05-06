/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace SK.Framework.Audio
{
    public class SFXController : MonoBehaviour
    {
        private bool m_IsMuted;

        private bool m_IsPaused;

        private float m_Volume = 1f;

        private readonly List<AudioHandler> m_Handlers = new List<AudioHandler>(4);

        public bool isMuted
        {
            get
            {
                return m_IsMuted;
            }
            set
            {
                if (m_IsMuted != value)
                {
                    m_IsMuted = value;
                    for (int i = 0; i < m_Handlers.Count; i++)
                        m_Handlers[i].SetMute(m_IsMuted);
                }
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
                    for (int i = 0; i < m_Handlers.Count; i++)
                        m_Handlers[i].isPaused = m_IsPaused;
                }
            }
        }

        public float volume
        {
            get
            {
                return m_Volume;
            }
            set
            {
                if (!Mathf.Approximately(value, m_Volume))
                {
                    m_Volume = value;
                    for (int i = 0; i < m_Handlers.Count; i++)
                        m_Handlers[i].SetVolume(m_Volume);
                }
            }
        }

        private AudioHandler GetHandler()
        {
            var handler = SKFramework.Module<ObjectPool.ObjectPool>().Get<AudioHandler>().Allocate();
            handler.SetSource(handler.gameObject.AddComponent<AudioSource>());
            handler.transform.SetParent(transform);
            m_Handlers.Add(handler);
            return handler;
        }

        private void LateUpdate()
        {
            for (int i = 0; i < m_Handlers.Count; i++)
            {
                if (!m_Handlers[i].gameObject.activeSelf)
                {
                    m_Handlers.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Stop()
        {
            for (int i = 0; i < m_Handlers.Count; i++)
                m_Handlers[i].Stop();
        }

        public AudioHandler Play(AudioClip clip, AudioMixerGroup output = null, float pitch = 1f, bool autoRecycle = true)
        {
            return GetHandler()
                .SetMute(m_IsMuted)
                .SetPause(m_IsPaused)
                .SetClip(clip)
                .SetVolume(m_Volume)
                .SetPitch(pitch)
                .SetOutput(output)
                .SetAutoRecycle(autoRecycle)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, Vector3 position, AudioMixerGroup output = null, float pitch = 1f, bool autoRecycle = true)
        {
            return GetHandler()
                .SetMute(m_IsMuted)
                .SetPause(m_IsPaused)
                .SetClip(clip)
                .SetVolume(m_Volume)
                .SetPitch(pitch)
                .SetPoint(position)
                .SetSpatialBlend(1f)
                .SetOutput(output)
                .SetAutoRecycle(autoRecycle)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, Transform followTarget, AudioMixerGroup output = null, float pitch = 1f, bool autoRecycle = true)
        {
            return GetHandler()
                .SetMute(m_IsMuted)
                .SetPause(m_IsPaused)
                .SetClip(clip)
                .SetVolume(m_Volume)
                .SetPitch(pitch)
                .SetFollowTarget(followTarget)
                .SetSpatialBlend(1f)
                .SetOutput(output)
                .SetAutoRecycle(autoRecycle)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, Vector3 position, float minDistance, float maxDistance, AudioMixerGroup output = null, float pitch = 1f, bool autoRecycle = true)
        {
            return GetHandler()
                .SetMute(m_IsMuted)
                .SetPause(m_IsPaused)
                .SetClip(clip)
                .SetVolume(m_Volume)
                .SetPitch(pitch)
                .SetPoint(position)
                .SetMinDistance(minDistance)
                .SetMaxDistance(maxDistance)
                .SetOutput(output)
                .SetAutoRecycle(autoRecycle)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, Transform followTarget, float minDistance, float maxDistance, AudioMixerGroup output = null, float pitch = 1f, bool autoRecycle = true)
        {
            return GetHandler()
                .SetMute(m_IsMuted)
                .SetPause(m_IsPaused)
                .SetClip(clip)
                .SetVolume(m_Volume)
                .SetPitch(pitch)
                .SetFollowTarget(followTarget)
                .SetMinDistance(minDistance)
                .SetMaxDistance(maxDistance)
                .SetOutput(output)
                .SetAutoRecycle(autoRecycle)
                .Play();
        }
    }
}