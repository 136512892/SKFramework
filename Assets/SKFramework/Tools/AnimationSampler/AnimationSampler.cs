using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// 动画采样器
    /// </summary>
    [Package("AnimationSampler", "0.0.1", PackagePathConst.AnimationSampler)]
    public class AnimationSampler : MonoBehaviour
    {
        public int currentClipIndex;
        public float normalizedTime;
    }
}