/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;

namespace SK.Framework.Actions
{
    public class AnimatorAction : AbstactAction
    {
        private readonly Animator m_Animator;

        private readonly string m_StateName;

        private readonly int m_LayerIndex;

        private bool m_IsPlaying;

        public AnimatorAction(Animator animator, string stateName, int layerIndex)
        {
            m_Animator = animator;
            m_StateName = stateName;
            m_LayerIndex = layerIndex;
        }

        protected override void OnInvoke()
        {
            if (!m_IsPlaying)
            {
                m_IsPlaying = true;
                m_Animator.Play(m_StateName);
                return;
            }
            m_IsCompleted = !m_Animator.GetCurrentAnimatorStateInfo(
                m_LayerIndex).IsName(m_StateName);
        }
    }
}