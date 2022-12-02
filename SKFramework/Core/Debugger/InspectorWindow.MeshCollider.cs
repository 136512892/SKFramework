using UnityEngine;

namespace SK.Framework.Debugger
{
    [ComponentInspector(typeof(MeshCollider))]
    public class MeshColliderInspector : ComponentInspector
    {
        protected override void OnDraw(Component component)
        {
            MeshCollider meshCollider = component as MeshCollider;

            boolValue = DrawToggle("Convex", meshCollider.convex, 125f);
            if (meshCollider.convex != boolValue)
            {
                if (!boolValue)
                {
                    meshCollider.isTrigger = false;
                }
                meshCollider.convex = boolValue;
            }

            GUI.enabled = meshCollider.convex;
            meshCollider.isTrigger = DrawToggle(30f, "Is Trigger", meshCollider.isTrigger, 99f);
            GUI.enabled = true;

            DrawText("Material", meshCollider.sharedMaterial != null ? meshCollider.sharedMaterial.name : "None(Physic Material)", 125f);
            
            DrawText("Mesh", meshCollider.sharedMesh != null ? meshCollider.sharedMesh.name : "None(Mesh)", 125f);
        }
    }
}