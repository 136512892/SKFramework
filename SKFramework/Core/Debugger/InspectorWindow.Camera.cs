using UnityEngine;

namespace SK.Framework.Debugger
{
    [ComponentInspector(typeof(Camera))]
    public class CameraInspector : ComponentInspector
    {
        protected override void OnDraw(Component component)
        {
            Camera camera = component as Camera;

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Clear Flags", GUILayout.Width(125f));
                if (GUILayout.Toggle(camera.clearFlags == CameraClearFlags.Skybox, "Skybox"))
                    camera.clearFlags = CameraClearFlags.Skybox;
                if (GUILayout.Toggle(camera.clearFlags == CameraClearFlags.SolidColor, "SolidColor"))
                    camera.clearFlags = CameraClearFlags.SolidColor;
                if (GUILayout.Toggle(camera.clearFlags == CameraClearFlags.Depth, "Depth"))
                    camera.clearFlags = CameraClearFlags.Depth;
                if (GUILayout.Toggle(camera.clearFlags == CameraClearFlags.Nothing, "DontClear"))
                    camera.clearFlags = CameraClearFlags.Nothing;
            }
            GUILayout.EndHorizontal();

            camera.backgroundColor = DrawColor("Background", camera.backgroundColor, 125f);

            camera.cullingMask = DrawInt("Culling Mask", camera.cullingMask, 125f);

            camera.fieldOfView = DrawHorizontalSlider("Field Of View", camera.fieldOfView, 1f, 179f, 125f, 40f);

            camera.depth = DrawFloat("Depth", camera.depth, 125f);
        }
    }
}