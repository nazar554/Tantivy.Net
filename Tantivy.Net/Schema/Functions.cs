namespace Tantivy.Net.Schema
{
    using System;
    using System.Runtime.InteropServices;
    using Tantivy.Net.Native;
    using Tantivy.Net.Native.Helpers;

    public static class Functions
    {
        public static bool IsValidFieldName(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return false;
            }

            return MarshalHelper.Utf8Call(fieldName, IsValidFieldNameImpl);
        }

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_is_valid_field_name", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool IsValidFieldNameImpl(IntPtr fieldName, UIntPtr fieldNameLength);
    }
}