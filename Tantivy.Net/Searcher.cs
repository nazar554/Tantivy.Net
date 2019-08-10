namespace Tantivy.Net
{
    using System;

    public sealed class Searcher : IDisposable
    {
        internal readonly Native.Types.LeasedSearcher _impl;

        internal Searcher(Native.Types.LeasedSearcher impl)
        {
            _impl = impl ?? throw new ArgumentNullException(nameof(impl));
        }

        public void Dispose() => _impl.Dispose();
    }
}