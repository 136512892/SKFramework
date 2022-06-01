using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// 音效控制器
    /// </summary>
    public class SFXController
    {
        #region Private Variables
        private bool isMuted;
        private bool isPaused;
        private readonly List<AudioHandler> handlers;
        #endregion

        #region Public Properties
        /// <summary>
        /// 是否静音
        /// </summary>
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
                    Log.Info(Module.Audio, string.Format("音效{0}静音", value ? "设置" : "取消"));
                }
            }
        }
        /// <summary>
        /// 是否暂停
        /// </summary>
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
                Log.Info(Module.Audio, string.Format("音效{0}", value ? "暂停" : "恢复"));
            }
        }
        #endregion

        #region Private Methods
        private AudioHandler GetHandler()
        {
            var handler = AudioHandler.Allocate();
            handler.transform.SetParent(Audio.Instance.transform);
            handlers.Add(handler);
            return handler;
        }
        #endregion

        #region Public Methods
        public SFXController()
        {
            handlers = new List<AudioHandler>();
        }
        public void Update()
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
            Log.Info(Module.Audio, "终止音效播放");
        }

        public AudioHandler Play(AudioClip clip)
        {
            Log.Info(Module.Audio, string.Format("播放音效 {0}", clip != null ? clip.name : "--"));
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, AudioMixerGroup output)
        {
            Log.Info(Module.Audio, string.Format("播放音效 {0}", clip != null ? clip.name : "--"));
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetOutput(output)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, Vector3 position)
        {
            Log.Info(Module.Audio, string.Format("播放音效 {0}", clip != null ? clip.name : "--"));
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
            Log.Info(Module.Audio, string.Format("播放音效 {0}", clip != null ? clip.name : "--"));
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
            Log.Info(Module.Audio, string.Format("播放音效 {0}", clip != null ? clip.name : "--"));
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
            Log.Info(Module.Audio, string.Format("播放音效 {0}", clip != null ? clip.name : "--"));
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
            Log.Info(Module.Audio, string.Format("播放音效 {0}", clip != null ? clip.name : "--"));
            return GetHandler()
                .SetMute(isMuted)
                .SetPause(isPaused)
                .SetClip(clip)
                .SetVolume(volume)
                .Play();
        }
        public AudioHandler Play(AudioClip clip, float volume, Vector3 position)
        {
            Log.Info(Module.Audio, string.Format("播放音效 {0}", clip != null ? clip.name : "--"));
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
            Log.Info(Module.Audio, string.Format("播放音效 {0}", clip != null ? clip.name : "--"));
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
            Log.Info(Module.Audio, string.Format("播放音效 {0}", clip != null ? clip.name : "--"));
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
            Log.Info(Module.Audio, string.Format("播放音效 {0}", clip != null ? clip.name : "--"));
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
            Log.Info(Module.Audio, string.Format("播放音效 {0}", clip != null ? clip.name : "--"));
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
            Log.Info(Module.Audio, string.Format("播放音效 {0}", clip != null ? clip.name : "--"));
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
            Log.Info(Module.Audio, string.Format("播放音效 {0}", clip != null ? clip.name : "--"));
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
            Log.Info(Module.Audio, string.Format("播放音效 {0}", clip != null ? clip.name : "--"));
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
        #endregion
    }
}