using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Audio
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/Audio")]
    public class AudioComponent : MonoBehaviour
    {
        [SerializeField] private AudioListener audioListener;
        [SerializeField] private BGMController bgmController;
        [SerializeField] private SFXController sfxController;
        [SerializeField] private List<AudioDatabase> databases = new List<AudioDatabase>(0);
        private Transform listenerTrans;

        public BGMController BGM { get { return bgmController; } }
        public SFXController SFX { get { return sfxController; } }

        private void Update()
        {
            sfxController.OnUpdate();

            if (listenerTrans != null)
            {
                audioListener.transform.position = listenerTrans.position;
            }
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
    }
}