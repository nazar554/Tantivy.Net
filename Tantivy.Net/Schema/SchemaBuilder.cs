namespace Tantivy.Net.Schema
{
    using System;

    public class SchemaBuilder : IDisposable
    {
        internal readonly Native.Types.SchemaBuilder _impl;

        public SchemaBuilder()
        {
            _impl = Native.Types.SchemaBuilder.Create();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public uint AddU64Field(string fieldName, IntOptions options)
        {
            return _impl.AddU64Field(fieldName, options._impl);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _impl.Dispose();
            }
        }
    }
}