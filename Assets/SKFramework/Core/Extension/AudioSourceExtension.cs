using UnityEngine;

namespace SK.Framework
{
    public static class AudioSourceExtension
    {
        public static AudioSource SetClip(this AudioSource source, AudioClip clip)
        {
            source.clip = clip;
            return source;
        }
        public static AudioSource SetPitch(this AudioSource source, float pitch)
        {
            source.pitch = pitch;
            return source;
        }
        public static AudioSource SetPriority(this AudioSource source, int priority)
        {
            source.priority = priority;
            return source;
        }
        public static AudioSource SetLoop(this AudioSource source, bool loop)
        {
            source.loop = loop;
            return source;
        }
        public static AudioSource SetPlayOnAwake(this AudioSource source, bool playOnAwake)
        {
            source.playOnAwake = playOnAwake;
            return source;
        }
        public static AudioSource SetVolume(this AudioSource source, float volume)
        {
            source.volume = volume;
            return source;
        }
        public static AudioSource SetPanStereo(this AudioSource source, float panStereo)
        {
            source.panStereo = panStereo;
            return source;
        }
        public static AudioSource SetSpatialBlend(this AudioSource source, float spatialBlend)
        {
            source.spatialBlend = spatialBlend;
            return source;
        }
        public static AudioSource Play(this AudioSource source, AudioClip clip)
        {
            source.clip = clip;
            source.Play();
            return source;
        }
    }
}