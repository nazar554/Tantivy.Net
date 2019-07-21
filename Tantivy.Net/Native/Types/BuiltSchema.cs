namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;

    internal sealed class BuiltSchema : Abstract.SafeHandleZeroIsInvalid
    {
        private BuiltSchema()
        {
        }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_drop_schema", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/
    }
}