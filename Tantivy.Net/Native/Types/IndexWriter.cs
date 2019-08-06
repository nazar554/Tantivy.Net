namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;

    internal sealed class IndexWriter : Abstract.SafeHandleZeroIsInvalid<IndexWriter>
    {
        private IndexWriter()
        {
        }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_drop_index_writer", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/
    }
}