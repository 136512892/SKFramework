using UnityEngine;

namespace SK.Framework.Utility
{
    public class CameraUtility
    {
        public static Texture2D Capture(Camera camera, int width, int height)
        {
            Rect rect = new Rect(0, 0, width, height);
            RenderTexture current = camera.targetTexture;
            RenderTexture rt = new RenderTexture(width, height, 0);
            camera.targetTexture = rt;
            camera.Render();
            RenderTexture.active = rt;
            Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();
            camera.targetTexture = current;
            RenderTexture.active = null;
            Object.Destroy(rt);
            return screenShot;
        }
    }
}