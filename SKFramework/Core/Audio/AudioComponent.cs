using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace SK.Framework.Audio
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/Audio")]
    public class AudioComponent : MonoBehaviour
    {
        [SerializeField] private List<AudioDatabase> databases = new List<AudioDatabase>(0);

        private AudioListener audioListener;
        //AudioListener跟随的Transform
        private Transform listenerTrans;

        public IBGMController BGM { get; private set; }
        public ISFXController SFX { get; private set; }

        private void Awake()
        {
            //如果当前有AudioListener组件 先将其销毁
            var hasedListener = FindObjectOfType<AudioListener>();
            if (hasedListener) Destroy(hasedListener);
            //创建一个新的物体设为子级并为其添加AudioListener组件
            var listener = new GameObject("Listener");
            listener.transform.SetParent(transform);
            audioListener = listener.AddComponent<AudioListener>();

            BGM = GetComponentInChildren<BGMController>();
            SFX = GetComponentInChildren<SFXController>();
        }

        private void Update()
        {
            if (listenerTrans != null)
                audioListener.transform.position = listenerTrans.position;
        }

        public void SetListener(Transform listenerTrans)
        {
            this.listenerTrans = listenerTrans;
        }

        public AudioClip FromDatabase(string databaseName, string clipName)
        {
            AudioDatabase database = databases.Find(m => m.name == databaseName);
            return database != null ? database[clipName] : null;
        }

        public AudioClip FromDatabase(string databaseName, string clipName, out AudioMixerGroup outputAudioMixerGroup)
        {
            outputAudioMixerGroup = null;
            AudioDatabase database = databases.Find(m => m.name == databaseName);
            if (database != null)
            {
                outputAudioMixerGroup = database.outputAudioMixerGroup;
                return database[clipName];
            }
            return null;
        }
    }
}