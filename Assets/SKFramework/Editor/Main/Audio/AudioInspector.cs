using UnityEditor;

namespace SK.Framework
{
    [CustomEditor(typeof(Audio))]
    [CSDNUrl("https://coderz.blog.csdn.net/article/details/124712128?spm=1001.2014.3001.5502")]
    public class AudioInspector : AbstractEditor<Audio>
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