namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;
    using Tantivy.Net.Native.Helpers;

    internal sealed class BuiltSchema : Abstract.SafeHandleZeroIsInvalid<BuiltSchema>
    {
        private BuiltSchema()
        {
        }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        public string GetFieldName(uint field)
        {
            var span = GetFieldNameImpl(this, field);
            return MarshalHelper.ConvertStringSpan(span);
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_drop_schema", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_schema_get_field_name", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern NativeByteSpan GetFieldNameImpl(BuiltSchema schema, uint field);
    }
}