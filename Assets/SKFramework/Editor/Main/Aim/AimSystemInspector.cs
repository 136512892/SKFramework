using UnityEditor;

namespace SK.Framework
{
    [CustomEditor(typeof(AimSystem))]
    [CSDNUrl("https://coderz.blog.csdn.net/article/details/124897949?spm=1001.2014.3001.5502")]
    public class AimSystemInspector : AbstractEditor<AimSystem>
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