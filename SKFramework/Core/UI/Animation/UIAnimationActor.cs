using System;
using UnityEngine;

namespace SK.Framework
{
    [Serializable]
    public class UIAnimationActor
    {
        public MonoBehaviour executer;

        public RectTransform actor;

        public UIAnimation animation;
    }
}