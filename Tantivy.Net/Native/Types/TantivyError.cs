namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;
    using Helpers;

    internal sealed class TantivyError : Abstract.SafeHandleZeroIsInvalid<TantivyError>
    {
        private TantivyError()
        {
        }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_drop_error", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_get_error_display_string", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern unsafe void GetErrorDisplayStringImpl(TantivyError error, byte* buffer, ref UIntPtr length);

        public override string ToString()
        {
            if (IsInvalid || IsClosed)
            {
                throw new ObjectDisposedException(nameof(TantivyError));
            }
            unsafe
            {
                return MarshalHelper.ReadUtf8String(
                    (byte* buffer, ref UIntPtr length) => GetErrorDisplayStringImpl(this, buffer, ref length)
                 );
            }
        }
    }
}