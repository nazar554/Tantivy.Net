namespace Tantivy.Net.Schema
{
    using System;

    public class TextFieldIndexing : Abstract.DisposableBase
    {
        internal readonly Native.Types.TextFieldIndexing _impl;

        internal TextFieldIndexing(TextOptions textOptions, Native.Types.TextFieldIndexing impl)
        {
            TextOptions = textOptions ?? throw new ArgumentNullException(nameof(textOptions));
            _impl = impl ?? throw new ArgumentNullException(nameof(impl));
        }

        public TextFieldIndexing()
        {
            _impl = Native.Types.TextFieldIndexing.Create();
        }

        public TextOptions TextOptions { get; }

        public bool IsReadOnly => _impl.IsReadOnly;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _impl.Dispose();
            }
        }
    }
}