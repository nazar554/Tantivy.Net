namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;
    using Helpers;

    internal sealed class QueryParser : Abstract.SafeHandleZeroIsInvalid<QueryParser>
    {
        private QueryParser()
        {
        }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        public static QueryParser ForIndex(Index index, in ReadOnlySpan<uint> fields)
        {
            if (index == null)
            {
                throw new ArgumentNullException(nameof(index));
            }

            unsafe
            {
                if (fields.IsEmpty)
                {
                    return ForIndexImpl(index, null, UIntPtr.Zero);
                }

                fixed (uint* ptr = fields)
                {
                    return ForIndexImpl(index, ptr, new UIntPtr((uint)fields.Length));
                }
            }
        }

        public BoxedDynQuery ParseQuery(string query)
        {
            unsafe
            {
                return MarshalHelper.Utf8Call(query, (buffer, ptr) =>
                {
                    BoxedDynQuery parsed = ParseQueryImpl(this, buffer, ptr, out var error);
                    if (parsed.IsInvalid)
                    {
                        throw new TantivyException(error);
                    }
                    return parsed;
                });
            }
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_drop_query_parser", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_query_parser_for_index", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static unsafe extern QueryParser ForIndexImpl(Index index, uint* fields, UIntPtr fieldsLength);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_query_parser_parse_query", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static unsafe extern BoxedDynQuery ParseQueryImpl(QueryParser parser, byte* query, UIntPtr queryLength, out QueryParserError error);
    }
}