using UnityEngine.Playables;

namespace SK.Framework
{
    public static class PlayableDirectorExtension 
    {
        public static PlayableDirector SetAsset(this PlayableDirector self, PlayableAsset playableAsset)
        {
            self.playableAsset = playableAsset;
            return self;
        }
        public static PlayableDirector SetUpdateMethod(this PlayableDirector self, DirectorUpdateMode directorUpdateMode)
        {
            self.timeUpdateMode = directorUpdateMode;
            return self;
        }
        public static PlayableDirector SetPlayOnAwake(this PlayableDirector self, bool playOnAwake)
        {
            self.playOnAwake = playOnAwake;
            return self;
        }
        public static PlayableDirector SetWrapMode(this PlayableDirector self, DirectorWrapMode wrapMode)
        {
            self.extrapolationMode = wrapMode;
            return self;
        }
        public static PlayableDirector SetInitialTime(this PlayableDirector self, float initialTime)
        {
            self.initialTime = initialTime;
            return self;
        }
        public static PlayableDirector Play(this PlayableDirector self)
        {
            self.Play();
            return self;
        }
    }
}