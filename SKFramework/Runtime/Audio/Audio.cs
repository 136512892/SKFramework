/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;

namespace SK.Framework.Audio
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/SKFramework.Audio")]
    public class Audio : ModuleBase
    {
        private AudioListener m_AudioListener;
        private Transform m_ListenerTrans;
        private BGMController m_BGMController;
        private SFXController m_SFXController;

        protected internal override void OnInitialization()
        {
            base.OnInitialization();

            AudioListener listener = FindObjectOfType<AudioListener>();
            if (listener != null)
                Destroy(listener);
            listener = new GameObject(typeof(AudioListener).Name).AddComponent<AudioListener>();
            listener.SetParent(transform);
            m_AudioListener = listener;
            m_BGMController = new GameObject("BGM").AddComponent<BGMController>().SetParent(transform);
            m_SFXController = new GameObject("SFX").AddComponent<SFXController>().SetParent(transform);
        }

        public BGMController BGM
        {
            get
            {
                return m_BGMController;
            }
        }

        public SFXController SFX
        {
            get
            {
                return m_SFXController;
            }
        }

        private void Update()
        {
            if (m_ListenerTrans != null)
                m_AudioListener.transform.position = m_ListenerTrans.position;
        }

        public void SetListener(Transform listenerTrans)
        {
            m_ListenerTrans = listenerTrans;
        }
    }
}