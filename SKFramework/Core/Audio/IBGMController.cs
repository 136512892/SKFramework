using UnityEngine;
using UnityEngine.Audio;

namespace SK.Framework.Audio
{
    public interface IBGMController
    {
        /// <summary>
        /// 音量
        /// </summary>
        public float Volume { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 音高
        /// </summary>
        public float Pitch { get; set; }

        /// <summary>
        /// 是否静默
        /// </summary>
        public bool IsMuted { get; set; }

        /// <summary>
        /// 是否暂停
        /// </summary>
        public bool IsPaused { get; set; }

        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlaying { get; }

        /// <summary>
        /// 是否循环
        /// </summary>
        public bool IsLoop { get; set; }

        /// <summary>
        /// 当前播放进度
        /// </summary>
        public float Progress { get; }

        /// <summary>
        /// 当前播放时间点
        /// </summary>
        public float Time { get; }

        /// <summary>
        /// 混音器组
        /// </summary>
        public AudioMixerGroup Output { get; set; }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="bgm">背景音乐</param>
        void Play(AudioClip bgm);

        /// <summary>
        /// 暂停
        /// </summary>
        void Pause();

        /// <summary>
        /// 恢复
        /// </summary>
        void Unpause();

        /// <summary>
        /// 终止
        /// </summary>
        void Stop();
    }
}