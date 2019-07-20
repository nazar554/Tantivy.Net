namespace Tantivy.Net.Native.Types.Abstract
{
    using System;
    using System.Runtime.InteropServices;

    internal abstract class SafeHandleZeroIsInvalid : SafeHandle
    {
        protected SafeHandleZeroIsInvalid() : base(IntPtr.Zero, true)
        {
        }

        protected SafeHandleZeroIsInvalid(bool ownsHandle) : base(IntPtr.Zero, ownsHandle)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;
    }
}