namespace Tantivy.Net.Native.Helpers
{
    using System;
    using System.Buffers;
    using System.Text;

    internal static class MarshalHelper
    {
        public const int StackAllocMaxBytes = 100 * sizeof(int);

        public unsafe delegate void Utf8ReadStringAction(byte* buffer, ref UIntPtr length);

        public unsafe delegate void Utf8StringAction(byte* buffer, UIntPtr length);

        public unsafe delegate T Utf8StringFunc<out T>(byte* buffer, UIntPtr length);

        public unsafe static string ReadUtf8String(Utf8ReadStringAction action)
        {
            // for short error string try stackalloc first
            var length = new UIntPtr(StackAllocMaxBytes);
            var stackBuffer = stackalloc byte[StackAllocMaxBytes];
            action(stackBuffer, ref length);
            int byteCount;
            checked
            {
                byteCount = (int)length;
            }

            if (byteCount <= StackAllocMaxBytes)
            {
                // we got the entire string
                return Encoding.UTF8.GetString(stackBuffer, byteCount);
            }
            else
            {
                // we need more memory
                var array = ArrayPool<byte>.Shared.Rent(byteCount);
                try
                {
                    fixed (byte* buffer = array)
                    {
                        // length already contains the actual number of bytes
                        action(buffer, ref length);
                    }
                    return Encoding.UTF8.GetString(array);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(array);
                }
            }
        }

        public static string ReadUtf8StringSpan(in ReadOnlySpan<byte> span)
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

        public static void Utf8Call(string value, Utf8StringAction action, bool clearArray = false)
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
                unsafe
                {
                    action(null, UIntPtr.Zero);
                }
                return;
            }

            int count = Encoding.UTF8.GetByteCount(value);

            if (count < StackAllocMaxBytes)
            {
                unsafe
                {
                    byte* bytes = stackalloc byte[count];
                    fixed (char* chars = value)
                    {
                        Encoding.UTF8.GetBytes(chars, value.Length, bytes, count);
                    }
                    action(bytes, new UIntPtr((uint)count));
                }
            }
            else
            {
                byte[] array = ArrayPool<byte>.Shared.Rent(count);

                try
                {
                    Encoding.UTF8.GetBytes(value, 0, value.Length, array, 0);

                    unsafe
                    {
                        fixed (byte* bytes = array)
                        {
                            action(bytes, new UIntPtr((uint)count));
                        }
                    }
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(array, clearArray);
                }
            }
        }

        public static T Utf8Call<T>(string value, Utf8StringFunc<T> action, bool clearArray = false)
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
                unsafe
                {
                    return action(null, UIntPtr.Zero);
                }
            }

            int count = Encoding.UTF8.GetByteCount(value);

            if (count < StackAllocMaxBytes)
            {
                unsafe
                {
                    byte* bytes = stackalloc byte[count];
                    fixed (char* chars = value)
                    {
                        Encoding.UTF8.GetBytes(chars, value.Length, bytes, count);
                    }
                    return action(bytes, new UIntPtr((uint)count));
                }
            }
            else
            {
                byte[] array = ArrayPool<byte>.Shared.Rent(count);

                try
                {
                    Encoding.UTF8.GetBytes(value, 0, value.Length, array, 0);

                    unsafe
                    {
                        fixed (byte* bytes = array)
                        {
                            return action(bytes, new UIntPtr((uint)count));
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
}