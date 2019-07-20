namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal readonly ref struct NativeByteSpan
    {
        public unsafe NativeByteSpan(byte* val, int length)
        {
#if DEBUG
            if (val == null)
            {
                throw new ArgumentNullException(nameof(val));
            }

            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
#endif
            _ptr = new IntPtr(val);
            _len = new UIntPtr((uint)length);
        }

        private readonly IntPtr _ptr;

        private readonly UIntPtr _len;

        public static implicit operator ReadOnlySpan<byte>(NativeByteSpan span)
        {
            int length;
            checked
            {
                length = (int)span._len.ToUInt32();
            }
            unsafe
            {
                return new ReadOnlySpan<byte>(span._ptr.ToPointer(), length);
            }
        }
    }
}