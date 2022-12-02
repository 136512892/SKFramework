using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Utility
{
    public class Vector3Utility 
    {
        public static float[] ToArray(Vector3 vector3)
        {
            float[] retArray = new float[3];
            retArray[0] = vector3.x;
            retArray[1] = vector3.y;
            retArray[2] = vector3.z;
            return retArray;
        }

        public static Vector3 GetMin(List<Vector3> vector3List)
        {
            Vector3 min = vector3List[0];
            for (int i = 1; i < vector3List.Count; i++)
            {
                min = Vector3.Min(min, vector3List[i]);
            }
            return min;
        }

        public static Vector3 GetMax(List<Vector3> self)
        {
            Vector3 max = self[0];
            for (int i = 1; i < self.Count; i++)
            {
                max = Vector3.Max(max, self[i]);
            }
            return max;
        }

        public static Vector3[] GetPositions(List<Transform> transformList)
        {
            Vector3[] retArray = new Vector3[transformList.Count];
            for (int i = 0; i < transformList.Count; i++)
            {
                retArray[i] = transformList[i].position;
            }
            return retArray;
        }

        public static Vector3[] GetPositions(Transform[] transformArray)
        {
            Vector3[] retArray = new Vector3[transformArray.Length];
            for (int i = 0; i < transformArray.Length; i++)
            {
                retArray[i] = transformArray[i].position;
            }
            return retArray;
        }

        /// <summary>
        /// 生成多边形Mesh网格
        /// </summary>
        /// <param name="points">多边形顶点数组</param>
        /// <returns>网格</returns>
        public static Mesh GenerateMesh(Vector3[] points) 
        {
            Mesh retMesh = new Mesh();
            List<int> triangles = new List<int>();
            for (int i = 0; i < points.Length - 1; i++)
            {
                triangles.Add(i);
                triangles.Add(i + 1);
                triangles.Add(points.Length - i - 1);
            }
            retMesh.vertices = points;
            retMesh.triangles = triangles.ToArray();
            retMesh.RecalculateBounds();
            retMesh.RecalculateNormals();
            return retMesh;
        }

        /// <summary>
        /// 生成贝塞尔曲线
        /// </summary>
        /// <param name="ctrolPoint">控制点</param>
        /// <param name="startPoint">贝塞尔曲线起点</param>
        /// <param name="endPoint">贝塞尔曲线终点</param>
        /// <param name="count">贝塞尔曲线点个数</param>
        /// <returns>组成贝塞尔曲线的点集合</returns>
        public static Vector3[] GenerateBeizer(Vector3 ctrolPoint, Vector3 startPoint, Vector3 endPoint, int count)
        {
            Vector3[] retValue = new Vector3[count];
            for (int i = 1; i <= count; i++)
            {
                float t = i / (float)count;
                float u = 1 - t;
                float tt = Mathf.Pow(t, 2);
                float uu = Mathf.Pow(u, 2);
                Vector3 point = uu * startPoint;
                point += 2 * u * t * ctrolPoint;
                point += tt * endPoint;
                retValue[i - 1] = point;
            }
            return retValue;
        }

        /// <summary>
        /// 判断目标点是否在指定区域内
        /// </summary>
        /// <param name="target">目标点</param>
        /// <param name="points">区域各顶点</param>
        /// <param name="height">区域高度</param>
        /// <returns>是否在区域内</returns>
        public static bool IsInRange(Vector3 target, Vector3[] points, float height)
        {
            if (target.y > height || target.y < -height) return false;
            var comparePoint = (points[0] + points[1]) * 0.5f;
            comparePoint += (comparePoint - target).normalized * 10000;
            int count = 0;
            for (int i = 0; i < points.Length; i++)
            {
                var a = points[i % points.Length];
                var b = points[(i + 1) % points.Length];
                var crossA = Mathf.Sign(Vector3.Cross(comparePoint - target, a - target).y);
                var crossB = Mathf.Sign(Vector3.Cross(comparePoint - target, b - target).y);
                if (Mathf.Approximately(crossA, crossB)) continue;
                var crossC = Mathf.Sign(Vector3.Cross(b - a, target - a).y);
                var crossD = Mathf.Sign(Vector3.Cross(b - a, comparePoint - a).y);
                if (Mathf.Approximately(crossC, crossD)) continue;
                count++;
            }
            return count % 2 == 1;
        }
        /// <summary>
        /// 确定坐标是否在平面内
        /// </summary>
        /// <param name="target">坐标</param>
        /// <param name="points">平面各顶点数组</param>
        /// <returns>坐标在平面内返回true,否则返回false</returns>
        public static bool IsInPlane(Vector3 target, Vector3[] points)
        {
            float RadianValue = 0;
            Vector3 vecOld = Vector3.zero;
            Vector3 vecNew = Vector3.zero;
            for (int i = 0; i < points.Length; i++)
            {
                if (i == 0)
                {
                    vecOld = points[i] - target;
                }
                if (i == points.Length - 1)
                {
                    vecNew = points[0] - target;
                }
                else
                {
                    vecNew = points[i + 1] - target;
                }
                RadianValue += Mathf.Acos(Vector3.Dot(vecOld.normalized, vecNew.normalized)) * Mathf.Rad2Deg;
                vecOld = vecNew;
            }
            return Mathf.Abs(RadianValue - 360) < 0.1f;
        }

        /// <summary>
        /// 获取直线与平面的交点
        /// </summary>
        /// <param name="point">直线上的某一点</param>
        /// <param name="direct">直线的方向</param>
        /// <param name="planeNormal">垂直于平面的向量</param>
        /// <param name="planePoint">平面上的任意一点</param>
        /// <returns>交点</returns>
        public static Vector3 GetIntersectWithPlane(Vector3 point, Vector3 direct, Vector3 planeNormal, Vector3 planePoint)
        {
            float d = Vector3.Dot(planePoint - point, planeNormal) / Vector3.Dot(direct.normalized, planeNormal);
            d = d < 0 ? 0 : d;
            return d * direct.normalized + point;
        }
    }
}