namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;
    using Helpers;

    internal sealed class SchemaBuilder : Abstract.SafeHandleZeroIsInvalid<SchemaBuilder>
    {
        private SchemaBuilder()
        {
        }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        public uint AddU64Field(string fieldName, IntOptions options)
        {
            EnsureNotClosed();
            return MarshalHelper.Utf8Call(fieldName, (buffer, length) =>
            {
                lock (this)
                {
                    unsafe
                    {
                        return AddU64FieldImpl(this, buffer, length, options);
                    }
                }
            });
        }

        public uint AddI64Field(string fieldName, IntOptions options)
        {
            EnsureNotClosed();
            return MarshalHelper.Utf8Call(fieldName, (buffer, length) =>
            {
                lock (this)
                {
                    unsafe
                    {
                        return AddI64FieldImpl(this, buffer, length, options);
                    }
                }
            });
        }

        public uint AddDateField(string fieldName, IntOptions options)
        {
            EnsureNotClosed();
            return MarshalHelper.Utf8Call(fieldName, (buffer, length) =>
            {
                lock (this)
                {
                    unsafe
                    {
                        return AddDateFieldImpl(this, buffer, length, options);
                    }
                }
            });
        }

        public uint AddFacetField(string fieldName)
        {
            EnsureNotClosed();
            return MarshalHelper.Utf8Call(fieldName, (buffer, length) =>
            {
                lock (this)
                {
                    unsafe
                    {
                        return AddFacetFieldImpl(this, buffer, length);
                    }
                }
            });
        }

        public uint AddBytesField(string fieldName)
        {
            EnsureNotClosed();
            return MarshalHelper.Utf8Call(fieldName, (buffer, length) =>
            {
                lock (this)
                {
                    unsafe
                    {
                        return AddBytesFieldImpl(this, buffer, length);
                    }
                }
            });
        }

        public uint AddTextField(string fieldName, TextOptions options)
        {
            EnsureNotClosed();
            return MarshalHelper.Utf8Call(fieldName, (buffer, length) =>
            {
                lock (this)
                {
                    unsafe
                    {
                        return AddTextFieldImpl(this, buffer, length, options);
                    }
                }
            });
        }

        public BuiltSchema Build()
        {
            lock (this)
            {
                var schema = BuildImpl(this);
                SetHandleAsInvalid();
                return schema;
            }
        }

        private void EnsureNotClosed()
        {
            if (IsClosed)
            {
                throw new ObjectDisposedException(nameof(SchemaBuilder));
            }
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_new_schema_builder", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SchemaBuilder Create();

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_drop_schema_builder", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_add_u64_field", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern unsafe uint AddU64FieldImpl(SchemaBuilder builder, byte* fieldName, UIntPtr fieldNameLength, IntOptions options);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_add_i64_field", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern unsafe uint AddI64FieldImpl(SchemaBuilder builder, byte* fieldName, UIntPtr fieldNameLength, IntOptions options);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_add_date_field", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern unsafe uint AddDateFieldImpl(SchemaBuilder builder, byte* fieldName, UIntPtr fieldNameLength, IntOptions options);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_add_facet_field", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern unsafe uint AddFacetFieldImpl(SchemaBuilder builder, byte* fieldName, UIntPtr fieldNameLength);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_add_bytes_field", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern unsafe uint AddBytesFieldImpl(SchemaBuilder builder, byte* fieldName, UIntPtr fieldNameLength);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_add_text_field", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern unsafe uint AddTextFieldImpl(SchemaBuilder builder, byte* fieldName, UIntPtr fieldNameLength, TextOptions options);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_build", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern BuiltSchema BuildImpl(SchemaBuilder builder);
    }
}