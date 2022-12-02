using UnityEngine;

namespace SK.Framework.Debugger
{
    [ComponentInspector(typeof(SphereCollider))]
    public class SphereColliderInspector : ComponentInspector
    {
        protected override void OnDraw(Component component)
        {
            SphereCollider sphereCollider = component as SphereCollider;

            sphereCollider.isTrigger = DrawToggle("Is Trigger", sphereCollider.isTrigger, 125f);

            DrawText("Material", sphereCollider.sharedMaterial != null ? sphereCollider.sharedMaterial.name : "None(Physic Material)", 125f);

            sphereCollider.center = DrawVector3("Center", sphereCollider.center, 125f, Screen.width * .03f);

            sphereCollider.radius = DrawFloat("Radius", sphereCollider.radius, 125f);
        }
    }
}