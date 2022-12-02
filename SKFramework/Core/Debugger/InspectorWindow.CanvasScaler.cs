using UnityEngine;
using UnityEngine.UI;

namespace SK.Framework.Debugger
{
    [ComponentInspector(typeof(CanvasScaler))]
    public class CanvasScalerInspector : ComponentInspector
    {
        protected override void OnDraw(Component component)
        {
            CanvasScaler canvasScaler = component as CanvasScaler;

            GUILayout.BeginHorizontal();
            GUILayout.Label("UI Scale Mode", GUILayout.Width(175f));
            if (GUILayout.Toggle(canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ConstantPixelSize, "ConstantPixelSize"))
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            if (GUILayout.Toggle(canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize, "ScaleWithScreenSize"))
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            if (GUILayout.Toggle(canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ConstantPhysicalSize, "ConstantPhysicalSize"))
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPhysicalSize;
            GUILayout.EndHorizontal();

            if (canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ConstantPixelSize)
            {
                canvasScaler.scaleFactor = DrawFloat("Scale Factor", canvasScaler.scaleFactor, 175f);
            }
            else if (canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
            {
                canvasScaler.referenceResolution = DrawVector2("Reference Resolution", canvasScaler.referenceResolution, 175f, Screen.width * .04f);

                GUILayout.BeginHorizontal();
                GUILayout.Label("Screen Match Mode", GUILayout.Width(175f));
                if (GUILayout.Toggle(canvasScaler.screenMatchMode == CanvasScaler.ScreenMatchMode.MatchWidthOrHeight, "Match Width Or Height"))
                    canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                if (GUILayout.Toggle(canvasScaler.screenMatchMode == CanvasScaler.ScreenMatchMode.Expand, "Expand"))
                    canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
                if (GUILayout.Toggle(canvasScaler.screenMatchMode == CanvasScaler.ScreenMatchMode.Shrink, "Shrink"))
                    canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Shrink;
                GUILayout.EndHorizontal();

                if (canvasScaler.screenMatchMode == CanvasScaler.ScreenMatchMode.MatchWidthOrHeight)
                {
                    canvasScaler.matchWidthOrHeight = DrawHorizontalSlider("Match", canvasScaler.matchWidthOrHeight, 0f, 1f, 175f, 30f);
                }
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Physical Unit", GUILayout.Width(175f));
                if (GUILayout.Toggle(canvasScaler.physicalUnit == CanvasScaler.Unit.Centimeters, "Centimeters"))
                    canvasScaler.physicalUnit = CanvasScaler.Unit.Centimeters;
                if (GUILayout.Toggle(canvasScaler.physicalUnit == CanvasScaler.Unit.Millimeters, "Millimeters"))
                    canvasScaler.physicalUnit = CanvasScaler.Unit.Millimeters;
                if (GUILayout.Toggle(canvasScaler.physicalUnit == CanvasScaler.Unit.Inches, "Inches"))
                    canvasScaler.physicalUnit = CanvasScaler.Unit.Inches;
                if (GUILayout.Toggle(canvasScaler.physicalUnit == CanvasScaler.Unit.Points, "Points"))
                    canvasScaler.physicalUnit = CanvasScaler.Unit.Points;
                if (GUILayout.Toggle(canvasScaler.physicalUnit == CanvasScaler.Unit.Picas, "Picas"))
                    canvasScaler.physicalUnit = CanvasScaler.Unit.Picas;
                GUILayout.EndHorizontal();

                canvasScaler.fallbackScreenDPI = DrawFloat("Fallback Screen DPI", canvasScaler.fallbackScreenDPI, 175f);
                canvasScaler.defaultSpriteDPI = DrawFloat("Default Sprite DPI", canvasScaler.defaultSpriteDPI, 175f);
            }
            canvasScaler.referencePixelsPerUnit = DrawFloat("Reference Pixels Per Unit", canvasScaler.referencePixelsPerUnit, 175f);
        }
    }
}