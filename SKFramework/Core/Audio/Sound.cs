using System;
using UnityEngine;

namespace SK.Framework.Audio
{
    /// <summary>
    /// 声音
    /// </summary>
    [Serializable]
    public class Sound
    {
        /// <summary>
        /// 声音来源
        /// </summary>
        public SoundSource source = SoundSource.AudioClip;
        /// <summary>
        /// 音频片段
        /// </summary>
        public AudioClip audioClip;
        /// <summary>
        /// 音频库名称(来源为音频库时起作用)
        /// </summary>
        public string databaseName;
        /// <summary>
        /// 音频数据名称(来源为音频库时起作用)
        /// </summary>
        public string audioDataName;
        /// <summary>
        /// 是否可播放
        /// </summary>
        public bool IsPlayable
        {
            get
            {
                switch (source)
                {
                    case SoundSource.AudioClip:
                        return audioClip != null;
                    case SoundSource.Datebase:
                        AudioDatabase database = Audio.Database.Get(databaseName);
                        if (database == null) Audio.Database.Load(databaseName, out database);
                        return database != null && database.GetClip(audioDataName) != null;
                }
                return false;
            }
        }

        /// <summary>
        /// 获取音频
        /// </summary>
        /// <returns></returns>
        public AudioClip GetAudioClip()
        {
            switch (source)
            {
                case SoundSource.AudioClip:  
                    return audioClip;
                case SoundSource.Datebase:
                    AudioDatabase database = Audio.Database.Get(databaseName);
                    if (database == null) Audio.Database.Load(databaseName, out database);
                    return database != null ? database.GetClip(audioDataName) : null;
            }
            return null;
        }
        /// <summary>
        /// 播放
        /// </summary>
        public void Play()
        {
            switch (source)
            {
                case SoundSource.AudioClip:
                    if (audioClip != null)
                    {
                        Audio.SFX.Play(audioClip);
                    }
                    break;
                case SoundSource.Datebase:
                    AudioDatabase database = Audio.Database.Get(databaseName);
                    if (database == null)
                    {
                        Audio.Database.Load(databaseName, out database);
                    }
                    if (database != null)
                    {
                        AudioData data = database[audioDataName];
                        if (data != null && data.clip != null)
                        {
                            Audio.SFX.Play(data.clip, database.outputAudioMixerGroup);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}