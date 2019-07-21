namespace Tantivy.Net.Schema
{
    using System;

    public class SchemaBuilder : Abstract.DisposableBase
    {
        internal readonly Native.Types.SchemaBuilder _impl;

        public SchemaBuilder()
        {
            _impl = Native.Types.SchemaBuilder.Create();
        }

        public virtual uint AddU64Field(string fieldName, IntOptions options)
        {
            return _impl.AddU64Field(fieldName, options._impl);
        }

        public virtual uint AddI64Field(string fieldName, IntOptions options)
        {
            return _impl.AddI64Field(fieldName, options._impl);
        }

        public virtual uint AddDateField(string fieldName, IntOptions options)
        {
            return _impl.AddDateField(fieldName, options._impl);
        }

        public virtual uint AddFacetField(string fieldName)
        {
            return _impl.AddFacetField(fieldName);
        }

        public virtual uint AddBytesField(string fieldName)
        {
            return _impl.AddBytesField(fieldName);
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