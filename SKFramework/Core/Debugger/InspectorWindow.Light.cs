using UnityEngine;

namespace SK.Framework.Debugger
{
    [ComponentInspector(typeof(Light))]
    public class LightInspector : ComponentInspector
    {
        protected override void OnDraw(Component component)
        {
            Light light = component as Light;

            GUILayout.BeginHorizontal();
            GUILayout.Label("Type", GUILayout.Width(155f));
            if (GUILayout.Toggle(light.type == LightType.Spot, "Spot"))
                light.type = LightType.Spot;
            if (GUILayout.Toggle(light.type == LightType.Directional, "Directional"))
                light.type = LightType.Directional;
            if (GUILayout.Toggle(light.type == LightType.Point, "Point"))
                light.type = LightType.Point;
            if (GUILayout.Toggle(light.type == LightType.Area || light.type == LightType.Rectangle || light.type == LightType.Disc, "Area"))
                light.type = LightType.Area;
            GUILayout.EndHorizontal();

            if (light.type == LightType.Spot)
            {
                light.range = DrawFloat("Range", light.range, 155f);
                light.spotAngle = DrawHorizontalSlider("Spot Angle", light.spotAngle, 1f, 179f, 155f, 45f);
            }
            else if (light.type == LightType.Point)
            {
                light.range = DrawFloat("Range", light.range, 155f);
            }

            light.color = DrawColor("Color", light.color, 155f);

            light.intensity = DrawFloat("Intensity", light.intensity, 155f);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Shadow Type", GUILayout.Width(155f));
            if (GUILayout.Toggle(light.shadows == LightShadows.None, "No Shadows"))
                light.shadows = LightShadows.None;
            if (GUILayout.Toggle(light.shadows == LightShadows.Hard, "Hard Shadows"))
                light.shadows = LightShadows.Hard;
            if (GUILayout.Toggle(light.shadows == LightShadows.Soft, "Soft Shadows"))
                light.shadows = LightShadows.Soft;
            GUILayout.EndHorizontal();

            DrawText("Cookie", light.cookie != null ? light.cookie.name : "None(Texture)", 155f);

            light.cookieSize = DrawFloat("Cookie Size", light.cookieSize, 155f);

            DrawText("Flare", light.flare != null ? light.flare.name : "None(Flare)", 155f);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Render Mode", GUILayout.Width(155f));
            if (GUILayout.Toggle(light.renderMode == LightRenderMode.Auto, "Auto"))
                light.renderMode = LightRenderMode.Auto;
            if (GUILayout.Toggle(light.renderMode == LightRenderMode.ForcePixel, "Important"))
                light.renderMode = LightRenderMode.ForcePixel;
            if (GUILayout.Toggle(light.renderMode == LightRenderMode.ForceVertex, "Not Important"))
                light.renderMode = LightRenderMode.ForceVertex;
            GUILayout.EndHorizontal();

            DrawText("Culling Mask", light.cullingMask.ToString(), 155f);
        }
    }
}