using UnityEngine;

namespace SK.Framework.Actions
{
    public class AnimateAction : AbstractAction
    {
        private readonly Animator animator;
        private readonly string stateName;
        private readonly int layerIndex;
        private bool isPlay;
        private bool isBegan;
        private float duration;
        private float beginTime;

        public AnimateAction(Animator animator, string stateName, int layerIndex)
        {
            this.animator = animator;
            this.stateName = stateName;
            this.layerIndex = layerIndex;
        }

        protected override void OnInvoke()
        {
            if (!isPlay)
            {
                isPlay = true;
                animator.Play(stateName);
                return;
            }
            if (!isBegan)
            {
                isBegan = true;
                beginTime = Time.time;
                AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(layerIndex);
                duration = asi.length;
            }
            isCompleted = Time.time - beginTime >= duration;
        }

        protected override void OnReset()
        {
            isPlay = false;
            isBegan = false;
        }
    }
}