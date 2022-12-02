namespace SK.Framework.Utility
{
    public class ArrayUtility
    {
        public static T[] Merge<T>(T[] array1, T[] array2)
        {
            T[] retArray = new T[array1.Length + array2.Length];
            for (int i = 0; i < array1.Length; i++)
            {
                retArray[i] = array1[i];
            }
            for (int i = 0; i < array2.Length; i++)
            {
                retArray[i + array1.Length] = array2[i];
            }
            return retArray;
        }
    }
}