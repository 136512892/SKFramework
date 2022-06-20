using UnityEngine;

namespace SK.Framework
{
    public static class RectTransformExtension
    {
        public static RectTransform SetAnchoredPosition(this RectTransform self, Vector2 anchoredPosition)
        {
            self.anchoredPosition = anchoredPosition;
            return self;
        }
        public static RectTransform SetAnchoredPosition(this RectTransform self, float x, float y)
        {
            Vector2 anchoredPosition = self.anchoredPosition;
            anchoredPosition.x = x;
            anchoredPosition.y = y;
            self.anchoredPosition = anchoredPosition;
            return self;
        }
        public static RectTransform SetAnchoredPositionX(this RectTransform self, float x)
        {
            Vector2 anchoredPosition = self.anchoredPosition;
            anchoredPosition.x = x;
            self.anchoredPosition = anchoredPosition;
            return self;
        }
        public static RectTransform SetAnchoredPositionY(this RectTransform self, float y)
        {
            Vector2 anchoredPosition = self.anchoredPosition;
            anchoredPosition.y = y;
            self.anchoredPosition = anchoredPosition;
            return self;
        }
        public static RectTransform SetOffsetMax(this RectTransform self, Vector2 offsetMax)
        {
            self.offsetMax = offsetMax;
            return self;
        }
        public static RectTransform SetOffsetMax(this RectTransform self, float x, float y)
        {
            Vector2 offsetMax = self.offsetMax;
            offsetMax.x = x;
            offsetMax.y = y;
            self.offsetMax = offsetMax;
            return self;
        }
        public static RectTransform SetOffsetMaxX(this RectTransform self, float x)
        {
            Vector2 offsetMax = self.offsetMax;
            offsetMax.x = x;
            self.offsetMax = offsetMax;
            return self;
        }
        public static RectTransform SetOffsetMaxY(this RectTransform self, float y)
        {
            Vector2 offsetMax = self.offsetMax;
            offsetMax.y = y;
            self.offsetMax = offsetMax;
            return self;
        }
        public static RectTransform SetOffsetMin(this RectTransform self, Vector2 offsetMin)
        {
            self.offsetMin = offsetMin;
            return self;
        }
        public static RectTransform SetOffsetMin(this RectTransform self, float x, float y)
        {
            Vector2 offsetMin = self.offsetMin;
            offsetMin.x = x;
            offsetMin.y = y;
            self.offsetMin = offsetMin;
            return self;
        }
        public static RectTransform SetOffsetMinX(this RectTransform self, float x)
        {
            Vector2 offsetMin = self.offsetMin;
            offsetMin.x = x;
            self.offsetMin = offsetMin;
            return self;
        }
        public static RectTransform SetOffsetMinY(this RectTransform self, float y)
        {
            Vector2 offsetMin = self.offsetMin;
            offsetMin.y = y;
            self.offsetMin = offsetMin;
            return self;
        }
        public static RectTransform SetAnchoredPosition3D(this RectTransform self, Vector3 anchoredPosition3D)
        {
            self.anchoredPosition3D = anchoredPosition3D;
            return self;
        }
        public static RectTransform SetAnchoredPosition3D(this RectTransform self, float x, float y)
        {
            Vector3 anchoredPosition3D = self.anchoredPosition3D;
            anchoredPosition3D.x = x;
            anchoredPosition3D.y = y;
            self.anchoredPosition3D = anchoredPosition3D;
            return self;
        }
        public static RectTransform SetAnchoredPosition3DX(this RectTransform self, float x)
        {
            Vector3 anchoredPosition3D = self.anchoredPosition3D;
            anchoredPosition3D.x = x;
            self.anchoredPosition3D = anchoredPosition3D;
            return self;
        }
        public static RectTransform SetAnchoredPosition3DY(this RectTransform self, float y)
        {
            Vector3 anchoredPosition3D = self.anchoredPosition3D;
            anchoredPosition3D.y = y;
            self.anchoredPosition3D = anchoredPosition3D;
            return self;
        }
        public static RectTransform SetAnchorMin(this RectTransform self, Vector2 anchorMin)
        {
            self.anchorMin = anchorMin;
            return self;
        }
        public static RectTransform SetAnchorMin(this RectTransform self, float x, float y)
        {
            Vector2 anchorMin = self.anchorMin;
            anchorMin.x = x;
            anchorMin.y = y;
            self.anchorMin = anchorMin;
            return self;
        }
        public static RectTransform SetAnchorMinX(this RectTransform self, float x)
        {
            Vector2 anchorMin = self.anchorMin;
            anchorMin.x = x;
            self.anchorMin = anchorMin;
            return self;
        }
        public static RectTransform SetAnchorMinY(this RectTransform self, float y)
        {
            Vector2 anchorMin = self.anchorMin;
            anchorMin.y = y;
            self.anchorMin = anchorMin;
            return self;
        }
        public static RectTransform SetAnchorMax(this RectTransform self, Vector2 anchorMax)
        {
            self.anchorMax = anchorMax;
            return self;
        }
        public static RectTransform SetAnchorMax(this RectTransform self, float x, float y)
        {
            Vector2 anchorMax = self.anchorMax;
            anchorMax.x = x;
            anchorMax.y = y;
            self.anchorMax = anchorMax;
            return self;
        }
        public static RectTransform SetAnchorMaxX(this RectTransform self, float x)
        {
            Vector2 anchorMax = self.anchorMax;
            anchorMax.x = x;
            self.anchorMax = anchorMax;
            return self;
        }
        public static RectTransform SetAnchorMaxY(this RectTransform self, float y)
        {
            Vector2 anchorMax = self.anchorMax;
            anchorMax.y = y;
            self.anchorMax = anchorMax;
            return self;
        }
        public static RectTransform SetPivot(this RectTransform self, Vector2 pivot)
        {
            self.pivot = pivot;
            return self;
        }
        public static RectTransform SetPivot(this RectTransform self, float x, float y)
        {
            Vector2 pivot = self.pivot;
            pivot.x = x;
            pivot.y = y;
            self.pivot = pivot;
            return self;
        }
        public static RectTransform SetPivotX(this RectTransform self, float x)
        {
            Vector2 pivot = self.pivot;
            pivot.x = x;
            self.pivot = pivot;
            return self;
        }
        public static RectTransform SetPivotY(this RectTransform self, float y)
        {
            Vector2 pivot = self.pivot;
            pivot.y = y;
            self.pivot = pivot;
            return self;
        }
        public static RectTransform SetSizeDelta(this RectTransform self, Vector2 sizeDelta)
        {
            self.sizeDelta = sizeDelta;
            return self;
        }
        public static RectTransform SetSizeDelta(this RectTransform self, float x, float y)
        {
            Vector2 sizeDelta = self.sizeDelta;
            sizeDelta.x = x;
            sizeDelta.y = y;
            self.sizeDelta = sizeDelta;
            return self;
        }
        public static RectTransform SetSizeDeltaX(this RectTransform self, float x)
        {
            Vector2 sizeDelta = self.sizeDelta;
            sizeDelta.x = x;
            self.sizeDelta = sizeDelta;
            return self;
        }
        public static RectTransform SetSizeDeltaY(this RectTransform self, float y)
        {
            Vector2 sizeDelta = self.sizeDelta;
            sizeDelta.y = y;
            self.sizeDelta = sizeDelta;
            return self;
        }
        public static RectTransform SetWidthWithCurrentAnchors(this RectTransform self, float width)
        {
            self.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            return self;
        }
        public static RectTransform SetHeightWithCurrentAnchors(this RectTransform self, float height)
        {
            self.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            return self;
        }
    }
}