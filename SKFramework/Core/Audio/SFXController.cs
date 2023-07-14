using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace SK.Framework.Audio
{
    [DisallowMultipleComponent]
    public class SFXController : MonoBehaviour, ISFXController
    {
        private bool isMuted;

        private bool isPaused;

        private float volume = 1f;
        
        private readonly List<AudioHandler> handlers = new List<AudioHandler>();

        public bool IsMuted
        {
            get
            {
                return isMuted;
            }
            set
            {
                if (isMuted != value)
                {
                    isMuted = value;
                    for (int i = 0; i < handlers.Count; i++)
                    {
                        handlers[i].SetMute(isMuted);
                    }
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
                    for (int i = 0; i < handlers.Count; i++)
                    {
                        handlers[i].IsPaused = isPaused;
                    }
                }
            }
        }

        public float Volume
        {
            get
            {
                return volume;
            }
            set
            {
                if (!Mathf.Approximately(value, volume))
                {
                    volume = value;
                    for (int i = 0; i < handlers.Count; i++)
                    {
                        handlers[i].SetVolume(volume);
                    }
                }
            }
        }

        private AudioHandler GetHandler()
        {
            var handler = AudioHandler.Allocate();
            handler.transform.SetParent(transform);
            handlers.Add(handler);
            return handler;
        }

        private void Update()
        {
            for (int i = 0; i < handlers.Count; i++)
            {
                if (handlers[i].IsRecycled)
                {
                    handlers.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Stop()
        {
            for (int i = 0; i < handlers.Count; i++)
            {
                handlers[i].Stop();
            }
        }

        public AudioHandler Play(AudioClip clip, AudioMixerGroup output = null, float pitch = 1f, bool autoRecycle = true)
        {
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetVolume(volume)
                .SetPitch(pitch)
                .SetOutput(output)
                .SetAutoRecycle(autoRecycle)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, Vector3 position, AudioMixerGroup output = null, float pitch = 1f, bool autoRecycle = true)
        {
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetVolume(volume)
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
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetVolume(volume)
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
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetVolume(volume)
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
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetVolume(volume)
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