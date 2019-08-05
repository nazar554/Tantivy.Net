namespace Tantivy.Net.Native.Helpers
{
    using System;
    using System.Buffers;
    using System.Text;

    internal static class MarshalHelper
    {
        public const int StackAllocMaxBytes = 100;

        public static string ConvertStringSpan(in ReadOnlySpan<byte> span)
        {
            if (span.IsEmpty)
            {
                return string.Empty;
            }

            unsafe
            {
                fixed (byte* bytes = &span.GetPinnableReference())
                {
                    return Encoding.UTF8.GetString(bytes, span.Length);
                }
            }
        }

        public static void Utf8Call(string value, Action<IntPtr, UIntPtr> action, bool clearArray = false)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (value.Length == 0)
            {
                action(IntPtr.Zero, UIntPtr.Zero);
                return;
            }

            int count = Encoding.UTF8.GetByteCount(value);

            if (count < StackAllocMaxBytes)
            {
                Utf8CallStackAlloc(value, count, action);
            }
            else
            {
                Utf8CallArrayPool(value, count, action, clearArray);
            }
        }

        private static void Utf8CallStackAlloc(string value, int count, Action<IntPtr, UIntPtr> action)
        {
            unsafe
            {
                byte* bytes = stackalloc byte[count];
                fixed (char* chars = value)
                {
                    Encoding.UTF8.GetBytes(chars, value.Length, bytes, count);
                }
                action(new IntPtr(bytes), new UIntPtr((uint)count));
            }
        }

        private static void Utf8CallArrayPool(string value, int count, Action<IntPtr, UIntPtr> action, bool clearArray)
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
                        action(new IntPtr(bytes), new UIntPtr((uint)count));
                    }
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(array, clearArray);
            }
        }

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

            if (value.Length == 0)
            {
                return action(IntPtr.Zero, UIntPtr.Zero);
            }

            int count = Encoding.UTF8.GetByteCount(value);

            return count < StackAllocMaxBytes
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