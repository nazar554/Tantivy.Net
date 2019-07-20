namespace Tantivy.Net.Native.Helpers
{
    using System;
    using System.Buffers;
    using System.Text;

    internal static class MarshalHelper
    {
        public static TReturn Utf8Call<TReturn>(string value, Func<IntPtr, UIntPtr, TReturn> action, bool clearArray = false)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            int count = Encoding.UTF8.GetByteCount(value);

            return count <= 128 * 1024
                ? Utf8CallStackAlloc(value, count, action)
                : Utf8CallArrayPool(value, count, action, clearArray);
        }

        private static TReturn Utf8CallStackAlloc<TReturn>(string value, int count, Func<IntPtr, UIntPtr, TReturn> action)
        {
            unsafe
            {
                byte* bytes = stackalloc byte[count];
                fixed (char* chars = value)
                {
                    Encoding.UTF8.GetBytes(chars, value.Length, bytes, count);
                }
                return action(new IntPtr(bytes), new UIntPtr((uint)count));
            }
        }

        private static TReturn Utf8CallArrayPool<TReturn>(string value, int count, Func<IntPtr, UIntPtr, TReturn> action, bool clearArray)
        {
            byte[] array = ArrayPool<byte>.Shared.Rent(count);

            try
            {
                unsafe
                {
                    fixed (byte* bytes = array)
                    {
                        fixed (char* chars = value)
                        {
                            Encoding.UTF8.GetBytes(chars, value.Length, bytes, count);
                        }
                        return action(new IntPtr(bytes), new UIntPtr((uint)count));
                    }
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(array, clearArray);
            }
        }
    }
}