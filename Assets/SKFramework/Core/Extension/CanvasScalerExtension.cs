using UnityEngine;
using UnityEngine.UI;

namespace SK.Framework
{
    public static class CanvasScalerExtension 
    {
        public static CanvasScaler SetScaleMode(this CanvasScaler self, CanvasScaler.ScaleMode scaleMode)
        {
            self.uiScaleMode = scaleMode;
            return self;
        }
        public static CanvasScaler SetScaleFactor(this CanvasScaler self, float scaleFactor)
        {
            self.scaleFactor = scaleFactor;
            return self;
        }
        public static CanvasScaler SetReferencePixelsPerUnit(this CanvasScaler self, float referencePixelsPerUnit)
        {
            self.referencePixelsPerUnit = referencePixelsPerUnit;
            return self;
        }
        public static CanvasScaler SetDynamicPixelsPerUnit(this CanvasScaler self, float dynamicPixelsPerUnit)
        {
            self.dynamicPixelsPerUnit = dynamicPixelsPerUnit;
            return self;
        }
        public static CanvasScaler SetReferenceResolution(this CanvasScaler self, Vector2 referenceResolution)
        {
            self.referenceResolution = referenceResolution;
            return self;
        }
        public static CanvasScaler SetReferenceResolution(this CanvasScaler self, float x, float y)
        {
            self.referenceResolution = new Vector2(x, y);
            return self;
        }
        public static CanvasScaler SetScreenMatchMode(this CanvasScaler self, CanvasScaler.ScreenMatchMode screenMatchMode)
        {
            self.screenMatchMode = screenMatchMode;
            return self;
        }
        public static CanvasScaler SetMatchWidthOrHeight(this CanvasScaler self, float matchWidthOrHeight)
        {
            self.matchWidthOrHeight = matchWidthOrHeight;
            return self;
        }
        public static CanvasScaler SetPhysicalUnit(this CanvasScaler self, CanvasScaler.Unit physicalUnit)
        {
            self.physicalUnit = physicalUnit;
            return self;
        }
        public static CanvasScaler SetFallbackScreenDPI(this CanvasScaler self, float fallbackScreenDPI)
        {
            self.fallbackScreenDPI = fallbackScreenDPI;
            return self;
        }
        public static CanvasScaler SetDefaultSpriteDPI(this CanvasScaler self, float defaultSpriteDPI)
        {
            self.defaultSpriteDPI = defaultSpriteDPI;
            return self;
        }
    }
}