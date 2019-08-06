namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Helpers;

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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Length
        {
            get
            {
                checked
                {
                    return (int)LengthImpl(this);
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsEmpty => IsEmptyImpl(this);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public delegate bool FieldFilter(uint field);

        public void FilterFields(FieldFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            lock (this)
            {
                FilterFieldsImpl(this, filter);
            }
        }

        public void AddText(uint field, string text)
        {
            unsafe
            {
                MarshalHelper.Utf8Call(text, (buffer, length) =>
                {
                    lock (this)
                    {
                        AddTextImpl(this, field, buffer, length);
                    }
                });
            }
        }

        public void AddU64(uint field, ulong value)
        {
            lock (this)
            {
                unsafe
                {
                    AddU64Impl(this, field, value);
                }
            }
        }

        public void AddI64(uint field, long value)
        {
            lock (this)
            {
                unsafe
                {
                    AddI64Impl(this, field, value);
                }
            }
        }

        public void AddDate(uint field, DateTimeOffset date) => AddDate(field, date.ToNanosecondsSinceEpoch());

        public void AddDate(uint field, DateTime date) => AddDate(field, date.ToNanosecondsSinceEpoch());

        public void AddBytes(uint field, in ReadOnlySpan<byte> bytes)
        {
            unsafe
            {
                lock (this)
                {
                    if (bytes.IsEmpty)
                    {
                        AddBytesImpl(this, field, null, UIntPtr.Zero);
                    }
                    else
                    {
                        fixed (byte* buffer = bytes)
                        {
                            AddBytesImpl(this, field, buffer, new UIntPtr((uint)bytes.Length));
                        }
                    }
                }
            }
        }

        private void AddDate(uint field, long value)
        {
            lock (this)
            {
                unsafe
                {
                    AddDateImpl(this, field, value);
                }
            }
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_new_document", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern Document Create();

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_drop_document", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_document_len", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern UIntPtr LengthImpl(Document document);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_document_is_empty", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool IsEmptyImpl(Document document);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_document_filter_fields", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void FilterFieldsImpl(Document document, [MarshalAs(UnmanagedType.FunctionPtr)] FieldFilter filter);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_document_add_text", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern unsafe void AddTextImpl(Document document, uint field, byte* buffer, UIntPtr length);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_document_add_u64", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void AddU64Impl(Document document, uint field, ulong value);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_document_add_i64", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void AddI64Impl(Document document, uint field, long value);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_document_add_date", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void AddDateImpl(Document document, uint field, long value);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_document_add_bytes", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern unsafe void AddBytesImpl(Document document, uint field, byte* buffer, UIntPtr length);
    }
}