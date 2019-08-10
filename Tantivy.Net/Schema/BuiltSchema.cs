namespace Tantivy.Net.Schema
{
    using System;

    public sealed class BuiltSchema : IDisposable
    {
        internal readonly Native.Types.BuiltSchema _impl;

        internal BuiltSchema(Native.Types.BuiltSchema impl)
        {
            _impl = impl ?? throw new ArgumentNullException(nameof(impl));
        }

        public string GetFieldName(uint field) => _impl.GetFieldName(field);

        public void Dispose() => _impl.Dispose();
    }
}