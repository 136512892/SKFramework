/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;

namespace SK.Framework
{
    public static class CameraUtility
    {
        public static Texture2D Capture(Camera camera, int width, int height)
        {
            RenderTexture current = camera.targetTexture;
            RenderTexture rt = new RenderTexture(width, height, 0);
            camera.targetTexture = rt;
            camera.Render();
            RenderTexture.active = rt;
            Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenShot.Apply();
            camera.targetTexture = current;
            RenderTexture.active = null;
            Object.Destroy(rt);
            return screenShot;
        }
    }
}