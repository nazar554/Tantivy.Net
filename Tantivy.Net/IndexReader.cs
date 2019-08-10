namespace Tantivy.Net
{
    using System;

    public sealed class IndexReader : IDisposable
    {
        internal Native.Types.IndexReader _impl;

        internal IndexReader(Native.Types.IndexReader impl)
        {
            _impl = impl ?? throw new ArgumentNullException(nameof(impl));
        }

        public Searcher Searcher() => new Searcher(_impl.Searcher());

        public void Dispose() => _impl.Dispose();
    }
}