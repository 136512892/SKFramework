using UnityEngine;

namespace SK.Framework
{
    public static class CameraExtension
    {
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