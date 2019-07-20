namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;
    using Helpers;

    internal sealed class SchemaBuilder : Abstract.SafeHandleZeroIsInvalid
    {
        private readonly object _syncRoot;

        private SchemaBuilder()
        {
            _syncRoot = new object();
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
                lock (_syncRoot)
                {
                    return AddU64FieldImpl(this, ptr, length, options);
                }
            });
        }

        public uint AddI64Field(string fieldName, IntOptions options)
        {
            return MarshalHelper.Utf8Call(fieldName, (ptr, length) =>
            {
                lock (_syncRoot)
                {
                    return AddI64FieldImpl(this, ptr, length, options);
                }
            });
        }

        public uint AddDateField(string fieldName, IntOptions options)
        {
            return MarshalHelper.Utf8Call(fieldName, (ptr, length) =>
            {
                lock (_syncRoot)
                {
                    return AddDateFieldImpl(this, ptr, length, options);
                }
            });
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_new_schema_builder", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SchemaBuilder Create();

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_drop_schema_builder", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_add_u64_field", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static unsafe extern uint AddU64FieldImpl(SchemaBuilder builder, IntPtr fieldName, UIntPtr fieldNameLength, IntOptions options);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_add_i64_field", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static unsafe extern uint AddI64FieldImpl(SchemaBuilder builder, IntPtr fieldName, UIntPtr fieldNameLength, IntOptions options);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_add_date_field", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static unsafe extern uint AddDateFieldImpl(SchemaBuilder builder, IntPtr fieldName, UIntPtr fieldNameLength, IntOptions options);
    }
}