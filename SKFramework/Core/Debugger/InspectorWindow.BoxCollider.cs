using UnityEngine;

namespace SK.Framework.Debugger
{
    [ComponentInspector(typeof(BoxCollider))]
    public class BoxColliderInspector : ComponentInspector
    {
        protected override void OnDraw(Component component)
        {
            BoxCollider boxCollider = component as BoxCollider;
            
            boxCollider.isTrigger = DrawToggle("Is Trigger", boxCollider.isTrigger, 125f);
            
            DrawText("Material", boxCollider.sharedMaterial != null ? boxCollider.sharedMaterial.name : "None(Physic Material)", 125f);
            
            boxCollider.center = DrawVector3("Center", boxCollider.center, 125f, Screen.width * .03f);
            
            boxCollider.size = DrawVector3("Size", boxCollider.size, 125f, Screen.width * .03f);
        }
    }
}