namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;

    internal sealed class TextOptions : Abstract.SafeHandleZeroIsInvalid<TextOptions>
    {
        private TextOptions()
        {
        }

        protected override bool ReleaseHandle()
        {
            if (_cachedIndexingOptions != null)
            {
                _cachedIndexingOptions.SetHandleAsInvalid();
            }
            Destroy(handle);
            return true;
        }

        private TextFieldIndexing _cachedIndexingOptions;

        public TextFieldIndexing IndexingOptions
        {
            get
            {
                lock (this)
                {
                    if (_cachedIndexingOptions == null)
                    {
                        _cachedIndexingOptions = new TextFieldIndexing(GetIndexingOptionsImpl(this));
                    }
                    return _cachedIndexingOptions;
                }
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                lock (this)
                {
                    SetIndexingOptionsImpl(this, value);
                    if (_cachedIndexingOptions != null)
                    {
                        _cachedIndexingOptions.SetHandleAsInvalid();
                        _cachedIndexingOptions = null;
                    }
                }
            }
        }

        public bool IsStored => IsStoredImpl(this);

        public void SetStored()
        {
            lock (this)
            {
                SetStoredImpl(this);
            }
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_new_text_options", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern TextOptions Create();

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_drop_text_options", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_text_options_get_indexing_options", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern IntPtr GetIndexingOptionsImpl(TextOptions options);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_text_options_set_indexing_options", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void SetIndexingOptionsImpl(TextOptions options, TextFieldIndexing indexingOptions);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_text_options_is_stored", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool IsStoredImpl(TextOptions options);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_text_options_set_stored", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void SetStoredImpl(TextOptions options);
    }
}