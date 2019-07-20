namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    internal sealed class SchemaBuilder : Abstract.SafeHandleZeroIsInvalid
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
            Span<byte> bytes = Encoding.UTF8.GetBytes(fieldName);
            unsafe
            {
                fixed (byte* ptr = bytes)
                {
                    return AddU64FieldImpl(this, new NativeByteSpan(ptr, bytes.Length), options);
                }
            }
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_new_schema_builder", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern SchemaBuilder Create();

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_drop_schema_builder", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_builder_add_u64_field", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern uint AddU64FieldImpl(SchemaBuilder builder, NativeByteSpan fieldName, IntOptions options);
    }
}