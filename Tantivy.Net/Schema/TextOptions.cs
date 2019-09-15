namespace Tantivy.Net.Schema
{
    using System;

    public sealed class TextOptions : IDisposable
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

        public TextOptions Stored()
        {
            _impl.SetStored();
            return this;
        }

        public void Dispose() => _impl.Dispose();
    }
}