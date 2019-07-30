namespace Tantivy.Net.Schema
{
    using System;

    public class TextOptions : Abstract.DisposableBase
    {
        internal readonly Native.Types.TextOptions _impl;

        public TextOptions()
        {
            _impl = Native.Types.TextOptions.Create();
        }

        public TextFieldIndexing IndexingOptions
        {
            get
            {
                var indexingOptionsImpl = _impl.IndexingOptions;
                return indexingOptionsImpl.IsInvalid ? null : new TextFieldIndexing(this, indexingOptionsImpl);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _impl.IndexingOptions = value._impl;
            }
        }

        public bool IsStored => _impl.IsStored;

        public TextOptions SetStored()
        {
            _impl.SetStored();
            return this;
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