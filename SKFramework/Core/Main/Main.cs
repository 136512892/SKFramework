using UnityEngine;

using SK.Framework.UI;
using SK.Framework.Log;
using SK.Framework.FSM;
using SK.Framework.Audio;
using SK.Framework.Refer;
using SK.Framework.Events;
using SK.Framework.Actions;
using SK.Framework.Debugger;
using SK.Framework.Resource;
using SK.Framework.ObjectPool;
using SK.Framework.Networking;

namespace SK.Framework
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/Main")]
    public class Main : MonoBehaviour
    {
        public static ActionsComponent Actions { get; private set; }

        public static AudioComponent Audio { get; private set; }

        public static CustomComponent Custom { get; private set; }

        public static DebuggerComponent Debugger { get; private set; }

        public static EventComponent Events { get; private set; }

        public static FSMComponent FSM { get; private set; }

        public static LogComponent Log { get; private set; }

        public static ObjectPoolComponent ObjectPool { get; private set; }

        public static ReferComponent Refer { get; private set; }

        public static ResourceComponent Resource { get; private set; }

        public static UIComponent UI { get; private set; }

        public static WebRequestComponent WebRequest { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(this);

            Actions = GetComponentInChildren<ActionsComponent>();
            Audio = GetComponentInChildren<AudioComponent>();
            Custom = GetComponentInChildren<CustomComponent>();
            Debugger = GetComponentInChildren<DebuggerComponent>();
            Events = GetComponentInChildren<EventComponent>();
            FSM = GetComponentInChildren<FSMComponent>();
            Log = GetComponentInChildren<LogComponent>();
            ObjectPool = GetComponentInChildren<ObjectPoolComponent>();
            Refer = GetComponentInChildren<ReferComponent>();
            Resource = GetComponentInChildren<ResourceComponent>();
            UI = GetComponentInChildren<UIComponent>();
            WebRequest = GetComponentInChildren<WebRequestComponent>();
        }

        private void OnDestroy()
        {
            Actions = null;
            Audio = null;
            Custom = null;
            Debugger = null;
            Events = null;
            FSM = null;
            Log = null;
            ObjectPool = null;
            Refer = null;
            Resource = null;
            UI = null;
            WebRequest = null;
        }
    }
}