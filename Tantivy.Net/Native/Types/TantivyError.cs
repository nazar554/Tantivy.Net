namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Buffers;
    using System.Runtime.InteropServices;
    using System.Text;
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

            var length = new UIntPtr(MarshalHelper.StackAllocMaxBytes);
            unsafe
            {
                // for short error string try stackalloc first
                var stackBuffer = stackalloc byte[MarshalHelper.StackAllocMaxBytes];
                GetErrorDisplayStringImpl(this, stackBuffer, ref length);
                checked
                {
                    int byteCount = (int)length;

                    if (byteCount <= MarshalHelper.StackAllocMaxBytes)
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
                                GetErrorDisplayStringImpl(this, buffer, ref length);
                            }
                            return Encoding.UTF8.GetString(array);
                        }
                        finally
                        {
                            ArrayPool<byte>.Shared.Return(array);
                        }
                    }
                }
            }
        }
    }
}