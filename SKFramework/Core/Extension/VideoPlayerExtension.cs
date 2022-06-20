using UnityEngine;
using UnityEngine.Video;

namespace SK.Framework 
{
    public static class VideoPlayerExtension
    {
        public static VideoPlayer SetSource(this VideoPlayer self, VideoSource source)
        {
            self.source = source;
            return self;
        }
        public static VideoPlayer SetClip(this VideoPlayer self, VideoClip videoClip)
        {
            self.clip = videoClip;
            return self;
        }
        public static VideoPlayer SetURL(this VideoPlayer self, string url)
        {
            self.url = url;
            return self;
        }
        public static VideoPlayer SetPlayOnAwake(this VideoPlayer self, bool playOnAwake)
        {
            self.playOnAwake = playOnAwake;
            return self;
        }
        public static VideoPlayer SetWaitForFirstFrame(this VideoPlayer self, bool waitForFirstFrame)
        {
            self.waitForFirstFrame = waitForFirstFrame;
            return self;
        }
        public static VideoPlayer SetSkipOnDrop(this VideoPlayer self, bool skipOnDrop)
        {
            self.skipOnDrop = skipOnDrop;
            return self;
        }
        public static VideoPlayer SetLoop(this VideoPlayer self, bool loop)
        {
            self.isLooping = loop;
            return self;
        }
        public static VideoPlayer SetPlaybackSpeed(this VideoPlayer self, float playbackSpeed)
        {
            self.playbackSpeed = playbackSpeed;
            return self;
        }
        public static VideoPlayer SetRenderMode(this VideoPlayer self, VideoRenderMode renderMode)
        {
            self.renderMode = renderMode;
            return self;
        }
        public static VideoPlayer SetTargetTexture(this VideoPlayer self, RenderTexture renderTexture)
        {
            self.targetTexture = renderTexture;
            return self;
        }
        public static VideoPlayer SetAspectRatio(this VideoPlayer self, VideoAspectRatio aspectRatio)
        {
            self.aspectRatio = aspectRatio;
            return self;
        }
        public static VideoPlayer SetAudioOutputMode(this VideoPlayer self, VideoAudioOutputMode audioOutputMode) 
        {
            self.audioOutputMode = audioOutputMode;
            return self;
        }
    }
}