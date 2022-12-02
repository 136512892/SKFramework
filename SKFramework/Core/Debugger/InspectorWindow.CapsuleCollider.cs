using UnityEngine;

namespace SK.Framework.Debugger
{
    [ComponentInspector(typeof(CapsuleCollider))]
    public class CapsuleColliderInspector : ComponentInspector
    {
        protected override void OnDraw(Component component)
        {
            CapsuleCollider capsuleCollider = component as CapsuleCollider;

            capsuleCollider.isTrigger = DrawToggle("Is Trigger", capsuleCollider.isTrigger, 125f);

            DrawText("Material", capsuleCollider.sharedMaterial != null ? capsuleCollider.sharedMaterial.name : "None(Physic Material)", 125f);

            capsuleCollider.center = DrawVector3("Center", capsuleCollider.center, 125f, Screen.width * .03f);

            capsuleCollider.radius = DrawFloat("Radius", capsuleCollider.radius, 125f, GUILayout.ExpandWidth(true));

            capsuleCollider.height = DrawFloat("Height", capsuleCollider.height, 125f, GUILayout.ExpandWidth(true));

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Direction", GUILayout.Width(125f));
                if (GUILayout.Toggle(capsuleCollider.direction == 0, "X-Axis"))
                    capsuleCollider.direction = 0;
                if (GUILayout.Toggle(capsuleCollider.direction == 1, "Y-Axis"))
                    capsuleCollider.direction = 1;
                if (GUILayout.Toggle(capsuleCollider.direction == 2, "Z-Axis"))
                    capsuleCollider.direction = 2;
            }
            GUILayout.EndHorizontal();
        }
    }
}