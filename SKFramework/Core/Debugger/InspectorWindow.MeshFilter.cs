using UnityEngine;

namespace SK.Framework.Debugger
{
    [ComponentInspector(typeof(MeshFilter))]
    public class MeshFilterInspector : ComponentInspector
    {
        protected override void OnDraw(Component component)
        {
            MeshFilter meshFilter = component as MeshFilter;
            DrawText("Mesh", meshFilter.sharedMesh != null ? meshFilter.sharedMesh.name : "None(Mesh)", 125f);
        }
    }
}