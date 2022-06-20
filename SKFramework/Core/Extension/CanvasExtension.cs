using UnityEngine;

namespace SK.Framework
{
    public static class CanvasExtension
    {
        public static Canvas SetRenderMode(this Canvas self, RenderMode renderMode)
        {
            self.renderMode = renderMode;
            return self;
        }
        public static Canvas SetPixelPerfect(this Canvas self, bool pixelPrefect)
        {
            self.pixelPerfect = pixelPrefect;
            return self;
        }
        public static Canvas SetSortOrder(this Canvas self, int sortOrder)
        {
            self.sortingOrder = sortOrder;
            return self;
        }
        public static Canvas SetTargetDisplay(this Canvas self, int targetDisplay)
        {
            self.targetDisplay = targetDisplay;
            return self;
        }
        public static Canvas SetWorldCamera(this Canvas self, Camera worldCamera)
        {
            self.worldCamera = worldCamera;
            return self;
        }
        public static Canvas SetPlaneDistance(this Canvas self, float planeDistance)
        {
            self.planeDistance = planeDistance;
            return self;
        }
    }
}