using UnityEngine;

namespace SK.Framework
{
    public sealed class InputMaster : MonoBehaviour
    {
        #region Private Variables
        private static InputMaster instance;
        private KeyInputController keyInputController;
        private MouseInputController mouseButtonInputController;
        private AxisInputController axisInputController;
        private bool toggle = true;
        #endregion

        #region Public Properties
        public static InputMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("[SKFramework.Input]").AddComponent<InputMaster>();
                    instance.keyInputController = new KeyInputController();
                    instance.mouseButtonInputController = new MouseInputController();
                    instance.axisInputController = new AxisInputController();
                    DontDestroyOnLoad(instance);
                }
                return instance;
            }
        }

        public static KeyInputController Key
        {
            get
            {
                return Instance.keyInputController;
            }
        }
        public static MouseInputController Mouse
        {
            get
            {
                return Instance.mouseButtonInputController;
            }
        }
        public static AxisInputController Axis
        {
            get
            {
                return Instance.axisInputController;
            }
        }
        public static bool Toggle
        {
            get
            {
                return Instance.toggle;
            }
            set
            {
                if (Instance.toggle != value)
                {
                    Instance.toggle = value;
                    if (!Instance.toggle)
                    {
                        Key.Reset();
                        Mouse.Reset();
                        Axis.Reset();
                    }
                    Log.Info(Module.Input, string.Format("{0}输入监听", value ? "打开" : "关闭"));
                }
            }
        }

        public static bool IsAnyKey
        {
            get
            {
                return Toggle && Input.anyKey;
            }
        }
        public static bool IsAnyKeyDown
        {
            get
            {
                return Toggle && Input.anyKeyDown;
            }
        }
        #endregion

        #region Private Methods
        private void Update()
        {
            if (toggle)
            {
                keyInputController.Update();
                mouseButtonInputController.Update();
                axisInputController.Update();
            }
        }
        #endregion

        #region Public Methods
        public bool GetKeyDown(KeyCode keyCode)
        {
            return toggle && Input.GetKeyDown(keyCode);
        }
        public bool GetKey(KeyCode keyCode)
        {
            return toggle && Input.GetKey(keyCode);
        }
        public bool GetKeyUp(KeyCode keyCode)
        {
            return toggle && Input.GetKeyUp(keyCode);
        }

        public bool GetMouseButtonDown(int mouseButton)
        {
            return toggle && Input.GetMouseButtonDown(mouseButton);
        }
        public bool GetMouseButton(int mouseButton)
        {
            return toggle && Input.GetMouseButton(mouseButton);
        }
        public bool GetMouseButtonUp(int mouseButton)
        {
            return toggle && Input.GetMouseButtonUp(mouseButton);
        }

        public float GetAxis(string axisName)
        {
            return toggle ? Input.GetAxis(axisName) : 0f;
        }
        public float GetAxisRaw(string axisName)
        {
            return toggle ? Input.GetAxisRaw(axisName) : 0f;
        }
        #endregion
    }
}