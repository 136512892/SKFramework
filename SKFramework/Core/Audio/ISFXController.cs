using UnityEngine;
using UnityEngine.Audio;

namespace SK.Framework.Audio
{
    public interface ISFXController
    {
        /// <summary>
        /// 是否静默
        /// </summary>
        bool IsMuted { get; set; }

        /// <summary>
        /// 是否暂停
        /// </summary>
        bool IsPaused { get; set; }

        /// <summary>
        /// 音量
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// 终止所有音效
        /// </summary>
        void Stop();

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="clip">音频</param>
        /// <param name="output"></param>
        /// <param name="pitch"></param>
        /// <param name="autoRecycle"></param>
        /// <returns></returns>
        AudioHandler Play(AudioClip clip, AudioMixerGroup output, float pitch, bool autoRecycle);

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="clip">音频</param>
        /// <param name="position"></param>
        /// <param name="pitch"></param>
        /// <param name="autoRecycle"></param>
        /// <returns></returns>
        AudioHandler Play(AudioClip clip, Vector3 position, AudioMixerGroup output, float pitch, bool autoRecycle);

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="clip">音频</param>
        /// <param name="followTarget"></param>
        /// <param name="pitch"></param>
        /// <param name="autoRecycle"></param>
        /// <returns></returns>
        AudioHandler Play(AudioClip clip, Transform followTarget, AudioMixerGroup output, float pitch, bool autoRecycle);

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="clip">音频</param>
        /// <param name="position"></param>
        /// <param name="minDistance"></param>
        /// <param name="maxDistance"></param>
        /// <param name="pitch"></param>
        /// <param name="autoRecycle"></param>
        /// <returns></returns>
        AudioHandler Play(AudioClip clip, Vector3 position, float minDistance, float maxDistance, AudioMixerGroup output, float pitch, bool autoRecycle);

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="clip">音频</param>
        /// <param name="followTarget"></param>
        /// <param name="minDistance"></param>
        /// <param name="maxDistance"></param>
        /// <param name="pitch"></param>
        /// <param name="autoRecycle"></param>
        /// <returns></returns>
        AudioHandler Play(AudioClip clip, Transform followTarget, float minDistance, float maxDistance, AudioMixerGroup output, float pitch, bool autoRecycle);
    }
}