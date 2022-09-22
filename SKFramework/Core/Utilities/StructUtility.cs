using System;
using System.Runtime.InteropServices;

namespace SK.Framework.Utility
{
    public class StructUtility
    {
        public static byte[] ToBytes<T>(T st) where T : struct
        {
            int size = Marshal.SizeOf(st);
            IntPtr intPtr = Marshal.AllocHGlobal(size);
            byte[] retValue;
            try
            {
                Marshal.StructureToPtr(st, intPtr, false);
                byte[] array = new byte[size];
                Marshal.Copy(intPtr, array, 0, size);
                retValue = array;
            }
            finally
            {
                Marshal.FreeHGlobal(intPtr);
            }
            return retValue;
        }

        public static object ToStruct<T>(byte[] bytes) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            if (size > bytes.Length) return null;
            IntPtr intPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, intPtr, size);
            object retValue = Marshal.PtrToStructure(intPtr, typeof(T));
            Marshal.FreeHGlobal(intPtr);
            return retValue;
        }
    }
}