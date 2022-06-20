using UnityEditor;

namespace SK.Framework
{
    [CustomEditor(typeof(Timer))]
    [CSDNUrl("https://coderz.blog.csdn.net/article/details/124767340?spm=1001.2014.3001.5502")]
    public class TimerInspector : AbstractEditor<Timer>
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