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

        public uint AddU64Field(string fieldName, IntOptions options)
        {
            return _impl.AddU64Field(fieldName, options._impl);
        }

        public uint AddI64Field(string fieldName, IntOptions options)
        {
            return _impl.AddI64Field(fieldName, options._impl);
        }

        public uint AddDateField(string fieldName, IntOptions options)
        {
            return _impl.AddDateField(fieldName, options._impl);
        }

        public uint AddFacetField(string fieldName)
        {
            return _impl.AddFacetField(fieldName);
        }

        public uint AddBytesField(string fieldName)
        {
            return _impl.AddBytesField(fieldName);
        }

        public uint AddTextField(string fieldName, TextOptions options)
        {
            return _impl.AddTextField(fieldName, options._impl);
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