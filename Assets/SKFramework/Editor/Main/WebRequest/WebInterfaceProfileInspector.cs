using UnityEditor;

namespace SK.Framework
{
    [CustomEditor(typeof(WebInterfaceProfile))]
    [CSDNUrl("https://coderz.blog.csdn.net/article/details/124808861?spm=1001.2014.3001.5502")]
    public class WebInterfaceProfileInspector : AbstractEditor<WebInterfaceProfile>
    {
        protected override bool IsEnableBaseInspectorGUI
        {
            get
            {
                return true;
            }
        }
    }
}