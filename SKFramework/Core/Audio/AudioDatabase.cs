using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace SK.Framework.Audio
{
    [Serializable]
    public class AudioDatabase
    {
        public string name;

        public AudioMixerGroup outputAudioMixerGroup;

        public List<AudioClip> clips = new List<AudioClip>(0);

        public AudioClip this[int index]
        {
            get
            {
                return clips[index];
            }
        }

        public AudioClip this[string name]
        {
            get
            {
                return clips.Find(m => m.name == name);
            }
        }
    }
}