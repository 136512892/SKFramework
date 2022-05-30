using UnityEngine;

namespace SK.Framework
{
    [AddComponentMenu("")]
    public class ActionMaster : MonoBehaviour 
    {
        private static ActionMaster instance;

        public static ActionMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("[SKFramework.Action]").AddComponent<ActionMaster>();
                    DontDestroyOnLoad(instance);
                }
                return instance;
            }
        }
    }
}