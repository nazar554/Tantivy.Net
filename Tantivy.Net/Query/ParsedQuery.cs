namespace Tantivy.Net.Query
{
    using System;

    public sealed class ParsedQuery : IDisposable
    {
        internal readonly Native.Types.BoxedDynQuery _impl;

        internal ParsedQuery(Native.Types.BoxedDynQuery impl) => _impl = impl ?? throw new ArgumentNullException(nameof(impl));

        public void Dispose() => _impl.Dispose();
    }
}