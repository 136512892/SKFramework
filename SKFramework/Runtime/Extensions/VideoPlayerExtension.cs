/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine.Video;

namespace SK.Framework
{
    public static class VideoPlayerExtension
    {
        public static VideoPlayer Play(this VideoPlayer player, string url) 
        {
            player.url = url;
            player.Play();
            return player;
        }
    }
}