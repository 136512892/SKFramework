using DG.Tweening;

namespace SK.Framework.UI
{
    public class TweenAnimation
    {
        public bool toggle;

        public float duration = 1f;

        public float delay = 0f;

        public Ease ease = Ease.Linear;

        public bool isCustom;

        public float Length
        {
            get
            {
                return duration + delay;
            }
        }
    }
}