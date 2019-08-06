namespace Tantivy.Net
{
    using System;

    public class IndexReader : Abstract.DisposableBase
    {
        internal Native.Types.IndexReader _impl;

        internal IndexReader(Native.Types.IndexReader impl)
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