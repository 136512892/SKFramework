/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework
{
    public static class Vector3Utility
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
                min = Vector3.Min(min, vector3List[i]);
            return min;
        }

        public static Vector3 GetMax(List<Vector3> self)
        {
            Vector3 max = self[0];
            for (int i = 1; i < self.Count; i++)
                max = Vector3.Max(max, self[i]);
            return max;
        }

        public static Vector3[] GetPositions(List<Transform> transformList)
        {
            Vector3[] retArray = new Vector3[transformList.Count];
            for (int i = 0; i < transformList.Count; i++)
                retArray[i] = transformList[i].position;
            return retArray;
        }

        public static Vector3[] GetPositions(Transform[] transformArray)
        {
            Vector3[] retArray = new Vector3[transformArray.Length];
            for (int i = 0; i < transformArray.Length; i++)
                retArray[i] = transformArray[i].position;
            return retArray;
        }

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
    }
}