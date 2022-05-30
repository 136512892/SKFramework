using UnityEngine;

namespace SK.Framework
{
    public static class JsonUtilityExtension 
    {
        public static string ToJson(this object self)
        {
            return JsonUtility.ToJson(self);
        }
        public static T ToObject<T>(this string self)
        {
            return JsonUtility.FromJson<T>(self);
        }
    }
}