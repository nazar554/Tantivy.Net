namespace Tantivy.Net.Native.Types.Abstract
{
    using System;
    using System.Runtime.InteropServices;

    internal abstract class SafeHandleZeroIsInvalid<T> : SafeHandle, IEquatable<T>
        where T : SafeHandle
    {
        protected SafeHandleZeroIsInvalid() : base(IntPtr.Zero, true)
        {
        }

        protected SafeHandleZeroIsInvalid(bool ownsHandle) : base(IntPtr.Zero, ownsHandle)
        {
        }

        protected SafeHandleZeroIsInvalid(IntPtr handle, bool ownsHandle) : base(handle, ownsHandle)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        public override bool Equals(object obj) => Equals(obj as T);

        public override int GetHashCode() => handle.GetHashCode();

        public virtual bool Equals(T other)
        {
            return other != null
                && handle == other.DangerousGetHandle();
        }
    }
}