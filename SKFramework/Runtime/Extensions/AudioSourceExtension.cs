/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;

namespace SK.Framework
{
    public static class AudioSourceExtension
    {
        public static AudioSource SetClip(this AudioSource self, AudioClip clip)
        {
            self.clip = clip;
            return self;
        }

        public static AudioSource SetPitch(this AudioSource self, float pitch)
        {
            self.pitch = pitch;
            return self;
        }

        public static AudioSource SetPriority(this AudioSource self, int priority)
        {
            self.priority = priority;
            return self;
        }

        public static AudioSource SetLoop(this AudioSource self, bool loop)
        {
            self.loop = loop;
            return self;
        }

        public static AudioSource SetPlayOnAwake(this AudioSource self, bool playOnAwake)
        {
            self.playOnAwake = playOnAwake;
            return self;
        }

        public static AudioSource SetVolume(this AudioSource self, float volume)
        {
            self.volume = volume;
            return self;
        }

        public static AudioSource SetPanStereo(this AudioSource self, float panStereo)
        {
            self.panStereo = panStereo;
            return self;
        }

        public static AudioSource SetSpatialBlend(this AudioSource self, float spatialBlend)
        {
            self.spatialBlend = spatialBlend;
            return self;
        }

        public static AudioSource Play(this AudioSource self, AudioClip clip)
        {
            self.clip = clip;
            self.Play();
            return self;
        }
    }
}