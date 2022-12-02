using System;
using UnityEngine;

namespace SK.Framework.Audio
{
    [Serializable]
    public class Sound
    {
        public enum Source
        {
            AudioClip,
            AudioDatabase
        }

        public Source source;

        public AudioClip audioClip;

        public string databaseName;

        public string clipName;

        public AudioClip Clip
        {
            get
            {
                switch (source)
                {
                    case Source.AudioClip: return audioClip;
                    case Source.AudioDatabase: return Main.Audio.FromDatabase(databaseName, clipName);
                    default: return null;
                }
            }
        }
    }
}