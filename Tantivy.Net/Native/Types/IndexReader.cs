namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;

    internal sealed class IndexReader : Abstract.SafeHandleZeroIsInvalid<BuiltSchema>
    {
        private IndexReader()
        {
        }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_drop_index_reader", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/
    }
}