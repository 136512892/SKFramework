using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace SK.Framework.Audios
{
    /// <summary>
    /// 音频处理器
    /// </summary>
    public class AudioHandler : MonoBehaviour
    {
        #region 对象池
        //对象池
        private readonly static Stack<AudioHandler> pool = new Stack<AudioHandler>();
        /// <summary>
        /// 对象池中缓存的数量
        /// </summary>
        public static int CacheCount
        {
            get
            {
                return pool.Count;
            }
        }
        /// <summary>
        /// 分配音频处理器
        /// </summary>
        /// <returns>音频处理器</returns>
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
        /// <summary>
        /// 回收音频处理器
        /// </summary>
        /// <param name="handler">音频处理器</param>
        public static void Recycle(AudioHandler handler)
        {
            handler.OnRecycle();
            handler.IsRecycled = true;
            pool.Push(handler);
        }
        /// <summary>
        /// 释放对象池
        /// </summary>
        public static void Release()
        {
            foreach (var handler in pool)
            {
                Destroy(handler.gameObject);
            }
            pool.Clear();
        }
        #endregion

        private AudioSource source;
        //是否暂停
        private bool isPaused;
        //跟随对象
        private Transform followTarget;
        //是否自动回收
        private bool autoRecycle = true;
        //是否已回收
        public bool IsRecycled { get; private set; }
        /// <summary>
        /// 播放进度
        /// </summary>
        public float Progress { get; set; }
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
                if (isPaused == value) return;
                isPaused = value;
                if (isPaused) source.Pause();
                else source.UnPause();
            }
        }
        /// <summary>
        /// 音量
        /// </summary>
        public float Volume
        {
            get
            {
                return source.volume;
            }
        }
        /// <summary>
        /// 是否正在播放
        /// </summary>
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
        /// <summary>
        /// 停止播放
        /// </summary>
        public void Stop()
        {
            source.Stop();
            if (autoRecycle)
            {
                Recycle(this);
            }
        }
        /// <summary>
        /// 回收事件
        /// </summary>
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
            name = $"AudioHandler - {(source.clip ? source.clip.name : "null")}";
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

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.Label(transform.position, name);
        }
#endif
    }
}