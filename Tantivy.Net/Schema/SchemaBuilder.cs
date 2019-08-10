namespace Tantivy.Net.Schema
{
    using System;

    public sealed class SchemaBuilder : IDisposable
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

        public BuiltSchema Build()
        {
            var schema = _impl.Build();
            return new BuiltSchema(schema);
        }

        public void Dispose() => _impl.Dispose();
    }
}