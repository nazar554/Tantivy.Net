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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _impl.Dispose();
            }
        }
    }
}