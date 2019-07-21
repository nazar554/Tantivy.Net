namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;

    internal sealed class TextFieldIndexing : Abstract.SafeHandleZeroIsInvalid
    {
        public TextFieldIndexing(IntPtr handle) : base(handle, false)
        {
            IsReadOnly = true;
        }

        public bool IsReadOnly { get; }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_new_text_field_indexing", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern TextFieldIndexing Create();

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_drop_text_field_indexing", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/
    }
}