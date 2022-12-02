using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace SK.Framework.Audio
{
    [DisallowMultipleComponent]
    public class SFXController : MonoBehaviour
    {
        private bool isMuted;

        private bool isPaused;
        
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
        private AudioHandler GetHandler()
        {
            var handler = AudioHandler.Allocate();
            handler.transform.SetParent(transform);
            handlers.Add(handler);
            return handler;
        }

        public void OnUpdate()
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

        public AudioHandler Play(AudioClip clip)
        {
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, AudioMixerGroup output)
        {
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetOutput(output)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, Vector3 position)
        {
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetPoint(position)
                .SetSpatialBlend(1f)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, Vector3 position, AudioMixerGroup output)
        {
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetPoint(position)
                .SetSpatialBlend(1f)
                .SetOutput(output)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, Transform followTarget)
        {
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetFollowTarget(followTarget)
                .SetSpatialBlend(1f)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, Transform followTarget, AudioMixerGroup output)
        {
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetFollowTarget(followTarget)
                .SetSpatialBlend(1f)
                .SetOutput(output)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, float volume)
        {
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetVolume(volume)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, float volume, Vector3 position)
        {
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetVolume(volume)
                .SetPoint(position)
                .SetSpatialBlend(1f)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, float volume, Transform followTarget)
        {
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetVolume(volume)
                .SetFollowTarget(followTarget)
                .SetSpatialBlend(1f)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, float volume, float pitch, Vector3 position)
        {
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetVolume(volume)
                .SetPitch(pitch)
                .SetPoint(position)
                .SetSpatialBlend(1f)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, float volume, float pitch, Transform followTarget)
        {
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetVolume(volume)
                .SetPitch(pitch)
                .SetFollowTarget(followTarget)
                .SetSpatialBlend(1f)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, float volume, Vector3 position, float minDistance, float maxDistance)
        {
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetVolume(volume)
                .SetPoint(position)
                .SetMinDistance(minDistance)
                .SetMaxDistance(maxDistance)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, float volume, Transform followTarget, float minDistance, float maxDistance)
        {
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetVolume(volume)
                .SetFollowTarget(followTarget)
                .SetMinDistance(minDistance)
                .SetMaxDistance(maxDistance)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, float volume, Vector3 position, float minDistance, float maxDistance, bool autoRecycle)
        {
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetVolume(volume)
                .SetPoint(position)
                .SetMinDistance(minDistance)
                .SetMaxDistance(maxDistance)
                .SetAutoRecycle(autoRecycle)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, float volume, Transform followTarget, float minDistance, float maxDistance, bool autoRecycle)
        {
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetVolume(volume)
                .SetFollowTarget(followTarget)
                .SetMinDistance(minDistance)
                .SetMaxDistance(maxDistance)
                .SetAutoRecycle(autoRecycle)
                .Play();
        }
    }
}