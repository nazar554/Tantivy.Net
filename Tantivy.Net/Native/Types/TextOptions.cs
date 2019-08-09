namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    internal sealed class TextOptions : Abstract.SafeHandleZeroIsInvalid<TextOptions>
    {
        private TextOptions()
        {
        }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public TextFieldIndexing IndexingOptions
        {
            get => GetIndexingOptionsImpl(this);
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                SetIndexingOptionsImpl(this, value);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStored => IsStoredImpl(this);

        public void SetStored() => SetStoredImpl(this);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_new_text_options", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern TextOptions Create();

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_drop_text_options", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_text_options_get_indexing_options", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern TextFieldIndexing GetIndexingOptionsImpl(TextOptions options);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_text_options_set_indexing_options", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void SetIndexingOptionsImpl(TextOptions options, TextFieldIndexing indexingOptions);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_text_options_is_stored", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool IsStoredImpl(TextOptions options);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_text_options_set_stored", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void SetStoredImpl(TextOptions options);
    }
}