using UnityEngine;

namespace SK.Framework.Debugger
{
    [ComponentInspector(typeof(RectTransform))]
    public class RectTransfromInspector : ComponentInspector
    {
        protected override void OnDraw(Component component)
        {
            RectTransform rectTransform = component as RectTransform;

            rectTransform.localPosition = DrawVector3("Position", rectTransform.localPosition, 125f, Screen.width * .03f);

            rectTransform.sizeDelta = DrawVector2("Size Delta", rectTransform.sizeDelta, 125f, "Width", "Height", 40f, Screen.width * .03f);

            GUILayout.Label("Anchors");
            
            rectTransform.anchorMin = DrawVector2(20f, "Min", rectTransform.anchorMin, 109f, Screen.width * .03f);

            rectTransform.anchorMax = DrawVector2(20f, "Max", rectTransform.anchorMax, 109f, Screen.width * .03f);

            rectTransform.pivot = DrawVector2("Pivot", rectTransform.pivot, 125f, Screen.width * .03f);

            rectTransform.localEulerAngles = DrawVector3("Rotation", rectTransform.localEulerAngles, 125f, Screen.width * .03f);

            rectTransform.localScale = DrawVector3("Scale", rectTransform.localScale, 125f, Screen.width * .03f);
        }
    }
}