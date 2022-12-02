using UnityEngine;

namespace SK.Framework.Debugger
{
    [ComponentInspector(typeof(CanvasGroup))]
    public class CanvasGroupInspector : ComponentInspector
    {
        protected override void OnDraw(Component component)
        {
            CanvasGroup canvasGroup = component as CanvasGroup;

            canvasGroup.alpha = DrawHorizontalSlider("Alpha", canvasGroup.alpha, 0f, 1f, 150f, 30f);

            canvasGroup.interactable = DrawToggle("Interactable", canvasGroup.interactable, 150f);

            canvasGroup.blocksRaycasts = DrawToggle("Blocks Raycasts", canvasGroup.blocksRaycasts, 150f);

            canvasGroup.ignoreParentGroups = DrawToggle("Ignore Parent Groups", canvasGroup.ignoreParentGroups, 150f);
        }
    }
}