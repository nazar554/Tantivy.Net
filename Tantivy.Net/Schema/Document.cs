namespace Tantivy.Net.Schema
{
    public class Document : Abstract.DisposableBase
    {
        internal readonly Native.Types.Document _impl;

        public Document()
        {
            _impl = Native.Types.Document.Create();
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