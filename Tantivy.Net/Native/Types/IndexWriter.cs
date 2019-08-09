namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;

    internal sealed class IndexWriter : Abstract.SafeHandleZeroIsInvalid<IndexWriter>
    {
        private IndexWriter()
        {
        }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        public ulong AddDocument(Document document, bool copy = false)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (copy)
            {
                return AddDocumentCopiedImpl(this, document);
            }
            ulong opStamp = AddDocumentMovedImpl(this, document);
            document.SetHandleAsInvalid();
            return opStamp;
        }

        public ulong Commit()
        {
            ulong opStamp = CommitImpl(this, out var error);
            if (!error.IsInvalid)
            {
                throw new TantivyException(error);
            }
            return opStamp;
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_drop_index_writer", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_writer_add_document_copy", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern ulong AddDocumentCopiedImpl(IndexWriter writer, Document document);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_writer_add_document_move", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern ulong AddDocumentMovedImpl(IndexWriter writer, Document document);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_writer_commit", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern ulong CommitImpl(IndexWriter writer, out TantivyError error);
    }
}