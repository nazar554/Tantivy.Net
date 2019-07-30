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
            return MarshalHelper.Utf8Call(fieldName, (ptr, length) =>
            {
                lock (this)
                {
                    return AddU64FieldImpl(this, ptr, length, options);
                }
            });
        }

        public uint AddI64Field(string fieldName, IntOptions options)
        {
            return MarshalHelper.Utf8Call(fieldName, (ptr, length) =>
            {
                lock (this)
                {
                    return AddI64FieldImpl(this, ptr, length, options);
                }
            });
        }

        public uint AddDateField(string fieldName, IntOptions options)
        {
            return MarshalHelper.Utf8Call(fieldName, (ptr, length) =>
            {
                lock (this)
                {
                    return AddDateFieldImpl(this, ptr, length, options);
                }
            });
        }

        public uint AddFacetField(string fieldName)
        {
            return MarshalHelper.Utf8Call(fieldName, (ptr, length) =>
            {
                lock (this)
                {
                    return AddFacetFieldImpl(this, ptr, length);
                }
            });
        }

        public uint AddBytesField(string fieldName)
        {
            return MarshalHelper.Utf8Call(fieldName, (ptr, length) =>
            {
                lock (this)
                {
                    return AddBytesFieldImpl(this, ptr, length);
                }
            });
        }

        public uint AddTextField(string fieldName, TextOptions options)
        {
            return MarshalHelper.Utf8Call(fieldName, (ptr, length) =>
            {
                lock (this)
                {
                    return AddTextFieldImpl(this, ptr, length, options);
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

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_new_schema_builder", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SchemaBuilder Create();

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_drop_schema_builder", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_add_u64_field", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern uint AddU64FieldImpl(SchemaBuilder builder, IntPtr fieldName, UIntPtr fieldNameLength, IntOptions options);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_add_i64_field", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern uint AddI64FieldImpl(SchemaBuilder builder, IntPtr fieldName, UIntPtr fieldNameLength, IntOptions options);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_add_date_field", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern uint AddDateFieldImpl(SchemaBuilder builder, IntPtr fieldName, UIntPtr fieldNameLength, IntOptions options);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_add_facet_field", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern uint AddFacetFieldImpl(SchemaBuilder builder, IntPtr fieldName, UIntPtr fieldNameLength);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_add_bytes_field", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern uint AddBytesFieldImpl(SchemaBuilder builder, IntPtr fieldName, UIntPtr fieldNameLength);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_add_text_field", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern uint AddTextFieldImpl(SchemaBuilder builder, IntPtr fieldName, UIntPtr fieldNameLength, TextOptions options);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_build", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern BuiltSchema BuildImpl(SchemaBuilder builder);
    }
}