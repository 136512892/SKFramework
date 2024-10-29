/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

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

        public static Canvas SetPixelPerfect(this Canvas self, bool pixelPerfect)
        {
            self.pixelPerfect = pixelPerfect;
            return self;
        }

        public static Canvas SetSortOrder(this Canvas self, int sortingOrder)
        {
            self.sortingOrder = sortingOrder;
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