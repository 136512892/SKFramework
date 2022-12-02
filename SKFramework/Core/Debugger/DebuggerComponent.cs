using UnityEngine;

namespace SK.Framework.Debugger
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/Debugger")]
    public class DebuggerComponent : MonoBehaviour
    {
        public enum WorkingType
        {
            AlwaysOpen,
            OnlyOpenWhenDevelopmentBuild,
            OnlyOpenInEditor,
            AlwaysClose
        }

        [SerializeField] private WorkingType workingType;

        private Rect expandRect;
        private Rect retractRect;
        private Rect dragableRect;

        private bool isExpand;

        private int fps;
        private float lastShowFPSTime;
        private Color fpsColor = Color.white;

        [HideInInspector] public GameObject currentSelected;

        [SerializeField] private ConsoleWindow consoleWindow;
        private HierarchyWindow hierarchyWindow;
        private InspectorWindow inspectorWindow;

        private IDebuggerWIndow currentWindow;

        public GUIStyle MiniButtonStyle { get; private set; }

        private void Start()
        {
            switch (workingType)
            {
                case WorkingType.AlwaysOpen: enabled = true; break;
                case WorkingType.OnlyOpenWhenDevelopmentBuild: enabled = Debug.isDebugBuild; break;
                case WorkingType.OnlyOpenInEditor: enabled = Application.isEditor; break;
                case WorkingType.AlwaysClose: enabled = false; break;
            }
            if (!enabled) return;

            expandRect = new Rect(Screen.width * .7f, 0f, Screen.width * .3f, Screen.height * .5f);
            retractRect = new Rect(Screen.width - 100f, 0f, 100f, 60f);
            dragableRect = new Rect(0, 0, Screen.width * .3f, 20f);

            hierarchyWindow = new HierarchyWindow();
            inspectorWindow = new InspectorWindow();

            consoleWindow.OnInitilization();
            hierarchyWindow.OnInitilization();
            inspectorWindow.OnInitilization();

            currentWindow = consoleWindow;
        }

        private void OnDestroy()
        {
            consoleWindow?.OnTermination();
            hierarchyWindow?.OnTermination();
            inspectorWindow?.OnTermination();

            consoleWindow = null;
            hierarchyWindow = null;
            inspectorWindow = null;
        }

        private void OnGUI()
        {
            if (MiniButtonStyle == null)
            {
                MiniButtonStyle = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleRight };
                var margin = MiniButtonStyle.margin;
                margin.top = 7;
                MiniButtonStyle.margin = margin;
            }
            if (isExpand)
            {
                expandRect = GUI.Window(0, expandRect, OnExpandGUI, "DEBUGGER");
                expandRect.x = Mathf.Clamp(expandRect.x, 0, Screen.width * .7f);
                expandRect.y = Mathf.Clamp(expandRect.y, 0, Screen.height * .5f);
                dragableRect = new Rect(0, 0, Screen.width * .3f, 20f);
            }
            else
            {
                retractRect = GUI.Window(0, retractRect, OnRetractGUI, "DEBUGGER");
                retractRect.x = Mathf.Clamp(retractRect.x, 0, Screen.width - 100f);
                retractRect.y = Mathf.Clamp(retractRect.y, 0, Screen.height - 60f);
                dragableRect = new Rect(0, 0, 100f, 20f);
            }
            if (Time.realtimeSinceStartup - lastShowFPSTime >= 1)
            {
                fps = Mathf.RoundToInt(1f / Time.deltaTime);
                lastShowFPSTime = Time.realtimeSinceStartup;
                fpsColor = consoleWindow.ErrorCount > 0 ? Color.red : consoleWindow.WarnCount > 0 ? Color.yellow : Color.white;
            }
        }

        private void OnExpandGUI(int windowId)
        {
            GUI.DragWindow(dragableRect);
            GUI.contentColor = fpsColor;
            if (GUILayout.Button(string.Format("FPS：{0}", fps), GUILayout.Height(20f)))
            {
                isExpand = false;
            }
            GUI.contentColor = Color.white;
            GUILayout.BeginHorizontal();
            {
                GUI.contentColor = currentWindow == consoleWindow ? Color.white : Color.grey;
                if (GUILayout.Button("Console")) currentWindow = consoleWindow;
                GUI.contentColor = currentWindow == hierarchyWindow ? Color.white : Color.grey;
                if (GUILayout.Button("Hierarchy")) currentWindow = hierarchyWindow;
                GUI.contentColor = currentWindow == inspectorWindow ? Color.white : Color.grey;
                if (GUILayout.Button("Inspector"))
                {
                    currentWindow = inspectorWindow;
                    inspectorWindow.OnEnter();
                }
                GUI.contentColor = Color.white;
            }
            GUILayout.EndHorizontal();
            currentWindow.OnWindowGUI();
        }
        private void OnRetractGUI(int windowId)
        {
            GUI.DragWindow(dragableRect);
            GUI.contentColor = fpsColor;
            if (GUILayout.Button(string.Format("FPS：{0}",fps), GUILayout.Height(30f)))
            {
                isExpand = true;
            }
            GUI.contentColor = Color.white;
        }
    }
}