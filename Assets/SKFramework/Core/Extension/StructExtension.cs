using System;
using System.Runtime.InteropServices;

namespace SK.Framework
{
    public static class StructExtension
    {
        public static byte[] ToBytes<T>(this T self) where T : struct
        {
            int size = Marshal.SizeOf(self);
            IntPtr intPtr = Marshal.AllocHGlobal(size);
            byte[] retValue;
            try
            {
                Marshal.StructureToPtr(self, intPtr, false);
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
        public static object ToStruct<T>(this byte[] self) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            if (size > self.Length) return null;
            IntPtr intPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(self, 0, intPtr, size);
            object retValue = Marshal.PtrToStructure(intPtr, typeof(T));
            Marshal.FreeHGlobal(intPtr);
            return retValue;
        }
    }
}