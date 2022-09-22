namespace SK.Framework.Utility
{
    public class PlatformUtility 
    {
        public static bool IsAndroid
        {
            get
            {
#if UNITY_ANDROID
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsIOS
        {
            get
            {
#if UNITY_IOS
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsStandlone
        {
            get
            {
#if UNITY_STANDALONE
                return true;
#else
                return false;          
#endif
            }
        }

        public static bool IsStandloneWin
        {
            get
            {
#if UNITY_STANDALONE_WIN
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsStandloneMac
        {
            get
            {
#if UNITY_STANDALONE_OSX
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsEditor
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsEditorWin
        {
            get
            {
#if UNITY_EDITOR_WIN
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsEditorMac
        {
            get
            {
#if UNITY_EDITOR_OSX
                return true;
#else
                return false;
#endif
            }
        }
    }
}