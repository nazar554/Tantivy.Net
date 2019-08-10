namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;
    using Helpers;

    internal sealed class QueryParserError : Abstract.SafeHandleZeroIsInvalid<QueryParserError>
    {
        private QueryParserError()
        {
        }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_drop_query_parser_error", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_get_query_parser_error_display_string", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern unsafe void GetQueryParserErrorDisplayStringImpl(QueryParserError error, byte* buffer, ref UIntPtr length);

        public override string ToString()
        {
            if (IsInvalid || IsClosed)
            {
                throw new ObjectDisposedException(nameof(QueryParserError));
            }
            unsafe
            {
                return MarshalHelper.ReadUtf8String(
                    (byte* buffer, ref UIntPtr length) => GetQueryParserErrorDisplayStringImpl(this, buffer, ref length)
                 );
            }
        }
    }
}