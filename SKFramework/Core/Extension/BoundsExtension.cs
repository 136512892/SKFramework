using UnityEngine;

namespace SK.Framework
{
    public static class BoundsExtension
    {
        /// <summary>
        /// Transforms 'bounds' using the specified transform matrix.
        /// 网格Mesh的Bounds边界盒是在网格局部坐标空间,不会受到transform的影响,可使用该函数进行变换.
        /// </summary>
        /// <remarks>
        /// Transforming a 'Bounds' instance means that the function will construct a new 'Bounds' 
        /// instance which has its center translated using the translation information stored in
        /// the specified matrix and its size adjusted to account for rotation and scale. The size
        /// of the new 'Bounds' instance will be calculated in such a way that it will contain the
        /// old 'Bounds'.
        /// </remarks>
        /// <param name="self">
        /// The 'Bounds' instance which must be transformed.
        /// </param>
        /// <param name="transformMatrix">
        /// The specified 'Bounds' instance will be transformed using this transform matrix. The function
        /// assumes that the matrix doesn't contain any projection or skew transformation.
        /// </param>
        /// <returns>
        /// The transformed 'Bounds' instance.
        /// </returns>
        public static Bounds Transform(this Bounds self, Matrix4x4 transformMatrix)
        {
            // We will need access to the right, up and look vector which are encoded inside the transform matrix
            Vector3 rightAxis = transformMatrix.GetColumn(0);
            Vector3 upAxis = transformMatrix.GetColumn(1);
            Vector3 lookAxis = transformMatrix.GetColumn(2);

            // We will 'imagine' that we want to rotate the bounds' extents vector using the rotation information
            // stored inside the specified transform matrix. We will need these when calculating the new size if
            // the transformed bounds.
            Vector3 rotatedExtentsRight = rightAxis * self.extents.x;
            Vector3 rotatedExtentsUp = upAxis * self.extents.y;
            Vector3 rotatedExtentsLook = lookAxis * self.extents.z;

            // Calculate the new bounds size along each axis. The size on each axis is calculated by summing up the 
            // corresponding vector component values of the rotated extents vectors. We multiply by 2 because we want
            // to get a size and curently we are working with extents which represent half the size.
            float newSizeX = (Mathf.Abs(rotatedExtentsRight.x) + Mathf.Abs(rotatedExtentsUp.x) + Mathf.Abs(rotatedExtentsLook.x)) * 2.0f;
            float newSizeY = (Mathf.Abs(rotatedExtentsRight.y) + Mathf.Abs(rotatedExtentsUp.y) + Mathf.Abs(rotatedExtentsLook.y)) * 2.0f;
            float newSizeZ = (Mathf.Abs(rotatedExtentsRight.z) + Mathf.Abs(rotatedExtentsUp.z) + Mathf.Abs(rotatedExtentsLook.z)) * 2.0f;

            // Construct the transformed 'Bounds' instance
            var transformedBounds = new Bounds
            {
                center = transformMatrix.MultiplyPoint(self.center),
                size = new Vector3(newSizeX, newSizeY, newSizeZ)
            };

            // Return the instance to the caller
            return transformedBounds;
        }
    }
}