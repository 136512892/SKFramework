using UnityEditor;

namespace SK.Framework
{
    [CustomEditor(typeof(ActionMaster))]
    [CSDNUrl("https://coderz.blog.csdn.net/article/details/124925366?spm=1001.2014.3001.5502")]
    public class ActionMasterInspector : AbstractEditor<ActionMaster>
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