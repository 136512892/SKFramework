using UnityEngine;
using UnityEditor;

namespace SK.Framework
{
    [CustomEditor(typeof(InputMaster))]
    [CSDNUrl("https://coderz.blog.csdn.net/article/details/124884667?spm=1001.2014.3001.5502")]
    public class InputMasterInspector : AbstractEditor<InputMaster>
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