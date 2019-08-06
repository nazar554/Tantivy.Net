namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;
    using Tantivy.Net.Native.Helpers;
    using Tantivy.Net.Schema;

    internal sealed class TextFieldIndexing : Abstract.SafeHandleZeroIsInvalid<TextFieldIndexing>
    {
        private TextFieldIndexing()
        {
        }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        public string Tokenizer
        {
            get
            {
                var span = GetTokenizer(this);
                return MarshalHelper.ReadUtf8StringSpan(span);
            }
            set
            {
                unsafe
                {
                    MarshalHelper.Utf8Call(value, (buffer, length) =>
                    {
                        lock (this)
                        {
                            SetTokenizer(this, buffer, length);
                        }
                    });
                }
            }
        }

        public IndexRecordOption IndexRecordOptions
        {
            get => EnumHelper.VerifyEnum(GetIndexOption(this));
            set
            {
                if (value < IndexRecordOption.Basic || value > IndexRecordOption.WithFreqsAndPositions)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                lock (this)
                {
                    SetIndexOption(this, value);
                }
            }
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_new_text_field_indexing", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern TextFieldIndexing Create();

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_drop_text_field_indexing", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_text_field_indexing_tokenizer", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern NativeByteSpan GetTokenizer(TextFieldIndexing handle);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_text_field_indexing_set_tokenizer", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static unsafe extern void SetTokenizer(TextFieldIndexing handle, byte* tokenizerName, UIntPtr tokenizerNameLength);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_text_field_indexing_index_option", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern IndexRecordOption GetIndexOption(TextFieldIndexing handle);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_text_field_indexing_set_index_option", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void SetIndexOption(TextFieldIndexing handle, IndexRecordOption option);
    }
}