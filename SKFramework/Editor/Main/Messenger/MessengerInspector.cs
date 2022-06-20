using UnityEditor;

namespace SK.Framework
{
    [CustomEditor(typeof(Messenger))]
    [CSDNUrl("https://coderz.blog.csdn.net/article/details/124800452?spm=1001.2014.3001.5502")]
    public class MessengerInspector : AbstractEditor<Messenger>
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