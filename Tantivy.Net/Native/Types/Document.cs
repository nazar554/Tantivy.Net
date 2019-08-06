namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;

    internal sealed class Document : Abstract.SafeHandleZeroIsInvalid<Document>
    {
        private Document()
        {
        }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_new_document", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern Document Create();

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_drop_document", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/
    }
}