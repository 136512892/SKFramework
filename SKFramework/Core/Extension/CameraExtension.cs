/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;

namespace SK.Framework
{
    public static class CameraExtension
    {
        public static Camera SetDepth(this Camera self, int depth)
        {
            self.depth = depth;
            return self;
        }

        public static Camera SetFieldOfView(this Camera self, float fieldOfView)
        {
            self.fieldOfView = fieldOfView;
            return self;
        }
    }
}