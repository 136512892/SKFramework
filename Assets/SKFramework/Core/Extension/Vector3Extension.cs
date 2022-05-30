using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework
{
    public static class Vector3Extension 
    {
        public static float[] ToArray(this Vector3 self)
        {
            float[] retArray = new float[3];
            retArray[0] = self.x;
            retArray[1] = self.y;
            retArray[2] = self.z;
            return retArray;
        }
        public static Quaternion ToQuaternion(this Vector3 self)
        {
            return Quaternion.Euler(self);
        }
        public static List<Vector3> GetMin(this List<Vector3> self, out Vector3 min)
        {
            min = self[0];
            for (int i = 1; i < self.Count; i++)
            {
                min = Vector3.Min(min, self[i]);
            }
            return self;
        }
        public static List<Vector3> GetMax(this List<Vector3> self, out Vector3 max)
        {
            max = self[0];
            for (int i = 1; i < self.Count; i++)
            {
                max = Vector3.Max(max, self[i]);
            }
            return self;
        }
        public static Vector3[] GetPositions(this List<Transform> self)
        {
            Vector3[] retArray = new Vector3[self.Count];
            for (int i = 0; i < self.Count; i++)
            {
                retArray[i] = self[i].position;
            }
            return retArray;
        }
        public static Vector3[] GetPositions(this Transform[] self)
        {
            Vector3[] retArray = new Vector3[self.Length];
            for (int i = 0; i < self.Length; i++)
            {
                retArray[i] = self[i].position;
            }
            return retArray;
        }
        /// <summary>
        /// 生成多边形Mesh网格
        /// </summary>
        /// <param name="self">多边形顶点数组</param>
        /// <returns>网格</returns>
        public static Mesh GenerateMesh(this Vector3[] self) 
        {
            Mesh retMesh = new Mesh();
            List<int> triangles = new List<int>();
            for (int i = 0; i < self.Length - 1; i++)
            {
                triangles.Add(i);
                triangles.Add(i + 1);
                triangles.Add(self.Length - i - 1);
            }
            retMesh.vertices = self;
            retMesh.triangles = triangles.ToArray();
            retMesh.RecalculateBounds();
            retMesh.RecalculateNormals();
            return retMesh;
        }
        /// <summary>
        /// 生成贝塞尔曲线
        /// </summary>
        /// <param name="self">控制点</param>
        /// <param name="startPoint">贝塞尔曲线起点</param>
        /// <param name="endPoint">贝塞尔曲线终点</param>
        /// <param name="count">贝塞尔曲线点个数</param>
        /// <returns>组成贝塞尔曲线的点集合</returns>
        public static Vector3[] GenerateBeizer(this Vector3 self, Vector3 startPoint, Vector3 endPoint, int count)
        {
            Vector3[] retValue = new Vector3[count];
            for (int i = 1; i <= count; i++)
            {
                float t = i / (float)count;
                float u = 1 - t;
                float tt = Mathf.Pow(t, 2);
                float uu = Mathf.Pow(u, 2);
                Vector3 point = uu * startPoint;
                point += 2 * u * t * self;
                point += tt * endPoint;
                retValue[i - 1] = point;
            }
            return retValue;
        }
        /// <summary>
        /// 判断目标点是否在指定区域内
        /// </summary>
        /// <param name="self">目标点</param>
        /// <param name="points">区域各顶点</param>
        /// <param name="height">区域高度</param>
        /// <returns>是否在区域内</returns>
        public static bool IsInRange(this Vector3 self, Vector3[] points, float height)
        {
            if (self.y > height || self.y < -height) return false;
            var comparePoint = (points[0] + points[1]) * 0.5f;
            comparePoint += (comparePoint - self).normalized * 10000;
            int count = 0;
            for (int i = 0; i < points.Length; i++)
            {
                var a = points[i % points.Length];
                var b = points[(i + 1) % points.Length];
                var crossA = Mathf.Sign(Vector3.Cross(comparePoint - self, a - self).y);
                var crossB = Mathf.Sign(Vector3.Cross(comparePoint - self, b - self).y);
                if (Mathf.Approximately(crossA, crossB)) continue;
                var crossC = Mathf.Sign(Vector3.Cross(b - a, self - a).y);
                var crossD = Mathf.Sign(Vector3.Cross(b - a, comparePoint - a).y);
                if (Mathf.Approximately(crossC, crossD)) continue;
                count++;
            }
            return count % 2 == 1;
        }
        /// <summary>
        /// 确定坐标是否在平面内
        /// </summary>
        /// <param name="self">坐标</param>
        /// <param name="points">平面各顶点数组</param>
        /// <returns>坐标在平面内返回true,否则返回false</returns>
        public static bool IsInPlane(this Vector3 self, Vector3[] points)
        {
            float RadianValue = 0;
            Vector3 vecOld = Vector3.zero;
            Vector3 vecNew = Vector3.zero;
            for (int i = 0; i < points.Length; i++)
            {
                if (i == 0)
                {
                    vecOld = points[i] - self;
                }
                if (i == points.Length - 1)
                {
                    vecNew = points[0] - self;
                }
                else
                {
                    vecNew = points[i + 1] - self;
                }
                RadianValue += Mathf.Acos(Vector3.Dot(vecOld.normalized, vecNew.normalized)) * Mathf.Rad2Deg;
                vecOld = vecNew;
            }
            return Mathf.Abs(RadianValue - 360) < 0.1f;
        }
        /// <summary>
        /// 获取直线与平面的交点
        /// </summary>
        /// <param name="self">直线上的某一点</param>
        /// <param name="direct">直线的方向</param>
        /// <param name="planeNormal">垂直于平面的向量</param>
        /// <param name="planePoint">平面上的任意一点</param>
        /// <returns>交点</returns>
        public static Vector3 GetIntersectWithPlane(this Vector3 self, Vector3 direct, Vector3 planeNormal, Vector3 planePoint)
        {
            float d = Vector3.Dot(planePoint - self, planeNormal) / Vector3.Dot(direct.normalized, planeNormal);
            d = d < 0 ? 0 : d;
            return d * direct.normalized + self;
        }
    }
}