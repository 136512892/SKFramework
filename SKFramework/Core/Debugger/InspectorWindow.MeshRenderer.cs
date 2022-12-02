using UnityEngine;
using UnityEngine.Rendering;

namespace SK.Framework.Debugger
{
    [ComponentInspector(typeof(MeshRenderer))]
    public class MeshRendererInspector : ComponentInspector
    {
        protected override void OnDraw(Component component)
        {
            MeshRenderer meshRenderer = component as MeshRenderer;

            GUILayout.Label("Meterials", GUILayout.Width(125f));
            for (int i = 0; i < meshRenderer.sharedMaterials.Length; i++)
            {
                DrawText(20f, string.Format("Element {0}", i), meshRenderer.sharedMaterials[i] 
                    != null ? meshRenderer.sharedMaterials[i].name : "None(Material)", 99f);
            }

            GUILayout.Label("Lighting");
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20f);
                GUILayout.Label("Cast Shadows");
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(30f);
                if (GUILayout.Toggle(meshRenderer.shadowCastingMode == ShadowCastingMode.On, "On"))
                    meshRenderer.shadowCastingMode = ShadowCastingMode.On;
                if (GUILayout.Toggle(meshRenderer.shadowCastingMode == ShadowCastingMode.Off, "Off"))
                    meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
                if (GUILayout.Toggle(meshRenderer.shadowCastingMode == ShadowCastingMode.TwoSided, "TwoSided"))
                    meshRenderer.shadowCastingMode = ShadowCastingMode.TwoSided;
                if (GUILayout.Toggle(meshRenderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly, "ShadowsOnly"))
                    meshRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20f);
                meshRenderer.receiveShadows = GUILayout.Toggle(meshRenderer.receiveShadows, "Receive Shadows");
            }
            GUILayout.EndHorizontal();
        }
    }
}