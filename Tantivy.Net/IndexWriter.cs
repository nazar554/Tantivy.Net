namespace Tantivy.Net
{
    using System;

    public class IndexWriter : Abstract.DisposableBase
    {
        internal Native.Types.IndexWriter _impl;

        internal IndexWriter(Native.Types.IndexWriter impl)
        {
            _impl = impl ?? throw new ArgumentNullException(nameof(impl));
        }

        public ulong AddDocument(Schema.Document document, bool copy = false)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return _impl.AddDocument(document._impl, copy);
        }

        public ulong Commit() => _impl.Commit();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _impl.Dispose();
            }
        }
    }
}