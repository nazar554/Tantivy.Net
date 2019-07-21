namespace Tantivy.Net.Schema
{
    public class TextOptions : Abstract.DisposableBase
    {
        internal readonly Native.Types.TextOptions _impl;

        public TextOptions()
        {
            _impl = Native.Types.TextOptions.Create();
        }

        public virtual TextFieldIndexing IndexingOptions
        {
            get
            {
                var indexingOptionsImpl = _impl.GetIndexingOptions();
                return indexingOptionsImpl.IsInvalid
                    ? null
                    : new TextFieldIndexing(this, indexingOptionsImpl);
            }
        }

        public virtual bool IsStored => _impl.IsStored;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _impl.Dispose();
            }
        }
    }
}