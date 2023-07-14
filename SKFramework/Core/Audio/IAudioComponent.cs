using UnityEngine;

namespace SK.Framework.Audio
{
    public interface IAudioComponent
    {
        /// <summary>
        /// 背景音乐控制器
        /// </summary>
        IBGMController BGM { get; }

        /// <summary>
        /// 音效控制器
        /// </summary>
        ISFXController SFX { get; }

        /// <summary>
        /// 设置AudioListener跟随的物体
        /// </summary>
        /// <param name="listener"></param>
        void SetListener(Transform listener);
    }
}