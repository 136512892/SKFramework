using UnityEngine;

namespace SK.Framework.Audio
{
    /// <summary>
    /// 音频
    /// </summary>
    public class Audio : MonoBehaviour
    {
        private static Audio instance;
        private BGMController bgm;
        private SFXController sfx;
        private AudioDatabaseController database;

        public static Audio Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("[SKFramework.Audio]").AddComponent<Audio>();
                    instance.bgm = new BGMController();
                    instance.sfx = new SFXController();
                    instance.database = new AudioDatabaseController();
                    DontDestroyOnLoad(instance);
                }
                return instance;
            }
        }

        /// <summary>
        /// 背景音乐控制器
        /// </summary>
        public static BGMController BGM
        {
            get
            {
                return Instance.bgm;
            }
        }
        /// <summary>
        /// 音效控制器
        /// </summary>
        public static SFXController SFX
        {
            get
            {
                return Instance.sfx;
            }
        }
        /// <summary>
        /// 音频库控制器
        /// </summary>
        public static AudioDatabaseController Database
        {
            get
            {
                return Instance.database;
            }
        }

        private void Update()
        {
            sfx.Update();
        }
        private void OnDestroy()
        {
            instance = null;
        }
    }
}