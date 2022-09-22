namespace SK.Framework.Utility
{
    public static class MathUtility
    {
        public static float Max(params float[] floatArray)
        {
            float max = floatArray[0];
            for (int i = 1; i < floatArray.Length; i++)
            {
                float compare = floatArray[i];
                max = max > compare ? max : compare;
            }
            return max;
        }

        public static int Max(params int[] intArray)
        {
            int max = intArray[0];
            for (int i = 1; i < intArray.Length; i++)
            {
                int compare = intArray[i];
                max = max > compare ? max : compare;
            }
            return max;
        }
    }
}