using UnityEngine;
using UnityEngine.UI;

namespace SK.Framework
{
    public static class ImageExtension
    {
        public static T SetSprite<T>(this T self, Sprite sprite) where T : Image
        {
            self.sprite = sprite;
            return self;
        }
        public static T SetType<T>(this T self, Image.Type type) where T : Image
        {
            self.type = type;
            return self;
        }
        public static T SetUseSpriteMesh<T>(this T self, bool useSpriteMesh) where T : Image
        {
            self.useSpriteMesh = useSpriteMesh;
            return self;
        }
        public static T SetPreserveAspect<T>(this T self, bool preserveAspect) where T : Image
        {
            self.preserveAspect = preserveAspect;
            return self;
        }
        public static T SetPixelPerUnitMultiplier<T>(this T self, int pixelPerUnitMultiplier) where T : Image
        {
            self.pixelsPerUnitMultiplier = pixelPerUnitMultiplier;
            return self;
        }
        public static T SetFillMethod<T>(this T self, Image.FillMethod fillMethod) where T : Image
        {
            self.fillMethod = fillMethod;
            return self;
        }
        public static T SetFillOrigin<T>(this T self, int fillOrigin) where T : Image
        {
            self.fillOrigin = fillOrigin;
            return self;
        }
        public static T SetFillAmount<T>(this T self, float fillAmount) where T : Image
        {
            self.fillAmount = fillAmount;
            return self;
        }
        public static T SetClockwise<T>(this T self, bool clockwise) where T : Image
        {
            self.fillClockwise = clockwise;
            return self;
        }
    }
}