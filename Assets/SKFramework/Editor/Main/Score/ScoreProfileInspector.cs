using UnityEditor;

namespace SK.Framework
{
    [CustomEditor(typeof(ScoreProfile))]
    [CSDNUrl("https://coderz.blog.csdn.net/article/details/124960835?spm=1001.2014.3001.5502")]
    public class ScoreProfileInspector : AbstractEditor<ScoreProfile>
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