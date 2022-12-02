using UnityEngine;

namespace SK.Framework.Debugger
{
    [ComponentInspector(typeof(Canvas))]
    public class CanvasInspector : ComponentInspector
    {
        protected override void OnDraw(Component component)
        {
            Canvas canvas = component as Canvas;

            GUILayout.BeginHorizontal();
            GUILayout.Label("Render Mode", GUILayout.Width(125f));
            if (GUILayout.Toggle(canvas.renderMode == RenderMode.ScreenSpaceOverlay, "Screen Space - Overlay"))
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            if (GUILayout.Toggle(canvas.renderMode == RenderMode.ScreenSpaceCamera, "Screen Space - Camera"))
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
            if (GUILayout.Toggle(canvas.renderMode == RenderMode.WorldSpace, "World Space"))
                canvas.renderMode = RenderMode.WorldSpace;
            GUILayout.EndHorizontal();

            if (canvas.renderMode != RenderMode.WorldSpace)
            {
                canvas.pixelPerfect = DrawToggle(20f, "Pixel Prefect", canvas.pixelPerfect, 109f);
            }

            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                DrawText(20f, "Render Camera", canvas.worldCamera.name, 109f);

                canvas.planeDistance = DrawFloat(20f, "Plane Distance", canvas.planeDistance, 109f);
            }
            else if (canvas.renderMode == RenderMode.WorldSpace)
            {
                DrawText(20f, "Event Camera", canvas.worldCamera.name, 109f);
            }

            if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace)
            {
                DrawText(20f, "Sorting Layer", canvas.sortingLayerName, 109f);
            }

            canvas.sortingOrder = DrawInt(20f, "Sort Order", canvas.sortingOrder, 109f);

            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {

                DrawText(20f, "Target Display", canvas.targetDisplay.ToString(), 111f);
            }

            DrawText("Additional Shader Channels", canvas.additionalShaderChannels.ToString(), 175f);
        }
    }
}