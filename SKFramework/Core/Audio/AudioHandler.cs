using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace SK.Framework.Audio
{
    public class AudioHandler : MonoBehaviour
    {
        private AudioSource source;

        private bool isPaused;

        private Transform followTarget;

        private bool autoRecycle = true;

        public bool IsRecycled { get; private set; }

        public float Progress { get; set; }

        public bool IsPaused
        {
            get
            {
                return isPaused;
            }
            set
            {
                if (isPaused == value) return;
                isPaused = value;
                if (isPaused) source.Pause();
                else source.UnPause();
            }
        }

        public float Volume
        {
            get
            {
                return source.volume;
            }
        }

        public bool IsPlaying
        {
            get
            {
                return source.isPlaying;
            }
        }

        private void Update()
        {
            if (followTarget != null)
            {
                transform.position = followTarget.position;
            }
            if (source != null && source.clip != null && source.isPlaying)
            {
                Progress = Mathf.Clamp01(source.time / source.clip.length);
            }
            if (source != null && !source.isPlaying && !source.loop)
            {
                Stop();
            }
        }

        public void Stop()
        {
            source.Stop();
            if (autoRecycle)
            {
                Recycle(this);
            }
        }

        public void OnRecycle()
        {
            name = "AudioHandler(Cache)";
            gameObject.SetActive(false);
            followTarget = null;
            transform.position = Vector3.zero;
            source.clip = null;
            source.outputAudioMixerGroup = null;
            source.loop = false;
            source.volume = 1f;
            source.priority = 128;
            source.pitch = 1f;
            source.panStereo = 0f;
            source.spatialBlend = 0f;
            source.minDistance = 1f;
            source.maxDistance = 500f;
        }

        public AudioHandler Play()
        {
            name = source.clip ? source.clip.name : "Null";
            source.Play();
            return this;
        }
        public AudioHandler SetSource(AudioSource audioSource)
        {
            source = audioSource;
            return this;
        }
        public AudioHandler SetClip(AudioClip audioClip)
        {
            source.clip = audioClip;
            return this;
        }
        public AudioHandler SetOutput(AudioMixerGroup audioMixerGroup)
        {
            source.outputAudioMixerGroup = audioMixerGroup;
            return this;
        }
        public AudioHandler SetLoop(bool isLoop)
        {
            source.loop = isLoop;
            return this;
        }
        public AudioHandler SetVolume(float volume)
        {
            source.volume = volume;
            return this;
        }
        public AudioHandler SetPitch(float pitch)
        {
            source.pitch = pitch;
            return this;
        }
        public AudioHandler SetSpatialBlend(float spatialBlend)
        {
            source.spatialBlend = spatialBlend;
            return this;
        }
        public AudioHandler SetFollowTarget(Transform followTarget)
        {
            this.followTarget = followTarget;
            return this;
        }
        public AudioHandler SetPriority(int priority)
        {
            source.priority = priority;
            return this;
        }
        public AudioHandler SetStereoPan(float panStereo)
        {
            source.panStereo = panStereo;
            return this;
        }
        public AudioHandler SetMute(bool isMute)
        {
            source.mute = isMute;
            return this;
        }
        public AudioHandler SetPause(bool isPause)
        {
            IsPaused = isPause;
            return this;
        }
        public AudioHandler SetPoint(Vector3 pos)
        {
            transform.position = pos;
            return this;
        }
        public AudioHandler SetMinDistance(float minDistance)
        {
            source.minDistance = minDistance;
            return this;
        }
        public AudioHandler SetMaxDistance(float maxDistance)
        {
            source.maxDistance = maxDistance;
            return this;
        }
        public AudioHandler SetAutoRecycle(bool autoRecycle)
        {
            this.autoRecycle = autoRecycle;
            return this;
        }

        #region >> ObjectPool
        private readonly static Stack<AudioHandler> pool = new Stack<AudioHandler>();
        public static int CacheCount
        {
            get
            {
                return pool.Count;
            }
        }
        public static AudioHandler Allocate()
        {
            AudioHandler retHandler;
            if (pool.Count > 0)
            {
                retHandler = pool.Pop();
                retHandler.IsRecycled = false;
                retHandler.gameObject.SetActive(true);
            }
            else
            {
                retHandler = new GameObject("[AudioHandler]").AddComponent<AudioHandler>();
                retHandler.SetSource(retHandler.gameObject.AddComponent<AudioSource>());
            }
            return retHandler;
        }
        public static void Recycle(AudioHandler handler)
        {
            handler.OnRecycle();
            handler.IsRecycled = true;
            pool.Push(handler);
        }
        public static void Release()
        {
            foreach (var handler in pool)
            {
                Destroy(handler.gameObject);
            }
            pool.Clear();
        }
        #endregion
    }
}