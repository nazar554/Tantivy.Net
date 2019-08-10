namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;

    internal sealed class IndexReader : Abstract.SafeHandleZeroIsInvalid<IndexReader>
    {
        private IndexReader()
        {
        }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        public LeasedSearcher Searcher() => SearcherImpl(this);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_drop_index_reader", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_reader_searcher", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern LeasedSearcher SearcherImpl(IndexReader reader);
    }
}