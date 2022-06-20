using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// 音频库
    /// </summary>
    [CreateAssetMenu(fileName = "New Audio Database", order = 215)]
    public class AudioDatabase : ScriptableObject
    {
        /// <summary>
        /// 音频库名称
        /// </summary>
        public string databaseName;
        /// <summary>
        /// 输出混音器组
        /// </summary>
        public AudioMixerGroup outputAudioMixerGroup;
        /// <summary>
        /// 音频数据列表
        /// </summary>
        public List<AudioData> datasets = new List<AudioData>(0);

        public AudioData this[int index]
        {
            get
            {
                return datasets[index];
            }
        }
        public AudioData this[string dataName]
        {
            get
            {
                return datasets.Find(m => m.name == dataName);
            }
        }

        public AudioClip GetClip(string dataName)
        {
            return datasets.Find(m => m.name == dataName)?.clip;
        }

        public void PlayAsBGM(string dataName)
        {
            Audio.BGM.Output = outputAudioMixerGroup;
            Audio.BGM.Play(GetClip(dataName));
        }
        public AudioHandler PlayAsSFX(string dataName)
        {
            var clip = GetClip(dataName);
            if (clip != null)
            {
                return Audio.SFX.Play(clip, outputAudioMixerGroup);
            }
            return null;
        }
        public AudioHandler PlayAsSFX(string dataName, Vector3 position)
        {
            var clip = GetClip(dataName);
            if (clip != null)
            {
                return Audio.SFX.Play(clip, position, outputAudioMixerGroup);
            }
            return null;
        }
        public AudioHandler PlayAsSFX(string dataName, Transform followTarget)
        {
            var clip = GetClip(dataName);
            if (clip != null)
            {
                return Audio.SFX.Play(clip, followTarget, outputAudioMixerGroup);
            }
            return null;
        }
    }
}