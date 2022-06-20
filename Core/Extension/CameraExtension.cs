using UnityEngine;

namespace SK.Framework
{
    public static class CameraExtension
    {
        public static Texture2D Capture(this Camera self, int width, int height)
        {
            Rect rect = new Rect(0, 0, width, height);
            RenderTexture current = self.targetTexture;
            RenderTexture rt = new RenderTexture(width, height, 0);
            self.targetTexture = rt;
            self.Render();
            RenderTexture.active = rt;
            Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();
            self.targetTexture = current;
            RenderTexture.active = null;
            Object.Destroy(rt);
            return screenShot;
        }
        public static Texture2D Capture(this Camera self, Vector2 resolution)
        {
            return Capture(self, (int)resolution.x, (int)resolution.y);
        }
        public static Camera SetFieldOfView(this Camera self, float fieldOfView)
        {
            self.fieldOfView = fieldOfView;
            return self;
        }
        public static Camera SetDepth(this Camera self, int depth)
        {
            self.depth = depth;
            return self;
        }
    }
}